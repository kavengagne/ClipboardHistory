using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;


namespace ClipboardHistoryApp.Scheduler
{
    public class ActionScheduler<TItem>
    {
        private readonly int _resolution;
        private readonly ConcurrentDictionary<string, ActionSchedulerItem<TItem>> _items;
        private readonly Stopwatch _stopwatch;
        private bool _isRunning;


        public ActionScheduler(TimeSpan resolutionMilliseconds) : this((int)resolutionMilliseconds.TotalMilliseconds)
        {
        }

        public ActionScheduler(int resolutionMilliseconds = 1)
        {
            _resolution = resolutionMilliseconds;
            _items = new ConcurrentDictionary<string, ActionSchedulerItem<TItem>>();
            _stopwatch = Stopwatch.StartNew();
            Start();
        }


        #region Public Methods
        public void Start()
        {
            _isRunning = true;
            Task.Run((Action)RunnerLoop);
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void AddOrUpdate(string uniqueId, Action<TItem> action, TItem item, TimeSpan delay)
        {
            AddOrUpdate(uniqueId, action, item, (int)delay.TotalMilliseconds);
        }

        public void AddOrUpdate(string uniqueId, Action<TItem> action, TItem item, long delay)
        {
            Debug.WriteLine($"ActionScheduler: {uniqueId}");
            Debug.WriteLine($"Added at {_stopwatch.ElapsedMilliseconds}");
            Debug.WriteLine($"Delay: {delay}");
            Debug.WriteLine($"Item: {item}");
            Debug.WriteLine("");

            if (!IsActionValid(action) || !IsDelayValid(delay))
            {
                return;
            }

            var runAtTime = _stopwatch.ElapsedMilliseconds + delay;
            _items.AddOrUpdate(uniqueId, new ActionSchedulerItem<TItem>(action, item, runAtTime), (key, inItem) =>
            {
                return new ActionSchedulerItem<TItem>(inItem.Action, item, runAtTime);
            });
        }
        #endregion Public Methods


        private static bool IsActionValid(Action<TItem> action)
        {
            return action != null;
        }

        private static bool IsDelayValid(long delay)
        {
            return delay >= 0;
        }

        private bool IsExecutionRequired(ActionSchedulerItem<TItem> item)
        {
            return item.RunAtTime <= _stopwatch.ElapsedMilliseconds;
        }

        private void RunnerLoop()
        {
            while (_isRunning)
            {
                var itemsToExecute = _items.Where(item => IsExecutionRequired(item.Value)).ToList();
                foreach (var item in itemsToExecute)
                {
                    Debug.WriteLine($"ActionScheduler: {item.Key}");
                    Debug.WriteLine($"Executed at {_stopwatch.ElapsedMilliseconds}");
                    Debug.WriteLine("");

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        item.Value.Action.Invoke(item.Value.Item);
                    });

                    ActionSchedulerItem<TItem> removedItem;
                    _items.TryRemove(item.Key, out removedItem);
                }
                Thread.Sleep(_resolution);
            }
        }
    }
}
