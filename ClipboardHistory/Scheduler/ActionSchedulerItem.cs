using System;


namespace ClipboardHistoryApp.Scheduler
{
    public class ActionSchedulerItem<TItem>
    {
        public Action<TItem> Action { get; set; }
        public TItem Item { get; set; }
        public long RunAtTime { get; set; }

        public ActionSchedulerItem(Action<TItem> action, TItem item, long runAtTime)
        {
            Action = action;
            Item = item;
            RunAtTime = runAtTime;
        }
    }
}