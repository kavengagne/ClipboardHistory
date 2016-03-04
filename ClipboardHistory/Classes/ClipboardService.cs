using System;
using System.Text;
using System.Windows.Forms;
using ClipboardHistoryApp.AppResources;
using ClipboardHistoryApp.Helpers;
using ClipboardHistoryApp.Models;
using ClipboardHistoryApp.Scheduler;


namespace ClipboardHistoryApp.Classes
{
    public sealed class ClipboardService : IDisposable
    {
        private const double SCHEDULER_RESOLUTION = 10;
        private const double CLIPBOARD_UPDATE_DELAY = 50;

        #region Fields
        private Action<ClipboardDataItem> _notificationCallback;
        private NotificationForm _form;
        private IntPtr _visualStudioHandle;
        private readonly ActionScheduler<ClipboardDataItem> _clipboardUpdateActionScheduler;
        #endregion

        
        #region Constructors
        public ClipboardService(Action<ClipboardDataItem> notificationCallback)
        {
            _form = new NotificationForm(this);
            NativeMethods.SetParent(_form.Handle, NativeMethods.HWND_MESSAGE);
            _notificationCallback = notificationCallback;
            _clipboardUpdateActionScheduler =
                new ActionScheduler<ClipboardDataItem>(TimeSpan.FromMilliseconds(SCHEDULER_RESOLUTION));
        }
        #endregion


        #region Public Methods
        public void DisableNotifications()
        {
            NativeMethods.RemoveClipboardFormatListener(_form.Handle);
        }

        public void EnableNotifications()
        {
            NativeMethods.AddClipboardFormatListener(_form.Handle);
        }

        public void SetWindowHandle(IntPtr mainWindowHandle)
        {
            _visualStudioHandle = mainWindowHandle;
        }

        public ClipboardDataItem GetClipboardDataItem()
        {
            string text = String.Empty;
            bool isError = false;
            if (Clipboard.ContainsText())
            {
                isError = !ExceptionWrapper.TrySafe<Exception>(
                    () => text = Clipboard.GetText(),
                    ex => text = String.Format(Resources.ClipboardException, ex.Message));
            }
            return new ClipboardDataItem(text, isError);
        }

        public void SetClipboardText(string text, Action<ClipboardDataItem> errorCallback)
        {
            DisableNotifications();
            try
            {
                Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                var item = new ClipboardDataItem(string.Format(Resources.ClipboardException, ex.Message), true);
                errorCallback?.Invoke(item);
            }
            EnableNotifications();
        }
        #endregion


        #region Event Handler
        private void OnClipboardUpdate(Message message)
        {
            var handler = _notificationCallback;
            if (handler != null)
            {
                var topWindowHandle = WindowHelper.GetTopWindowHandle();
                if (Configuration.VisualStudioClipboardOnly && topWindowHandle != _visualStudioHandle)
                {
                    return;
                }

                var data = GetClipboardDataItem();
                var uniqueId = GetUniqueKey(message, data);
                _clipboardUpdateActionScheduler.AddOrUpdate(uniqueId, handler, data, TimeSpan.FromMilliseconds(CLIPBOARD_UPDATE_DELAY));
            }
        }
        #endregion


        #region Private Methods
        private static string GetUniqueKey(Message message, ClipboardDataItem data)
        {
            var key = $"{message.HWnd}{message.Msg}{data.Snippet}{data.NumberOfLines}";
            return Convert.ToBase64String(Encoding.Default.GetBytes(key));
        }
        #endregion Private Methods


        #region IDisposable Interface / Resources Cleanup
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ClipboardService()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_form != null)
                {
                    DisableNotifications();
                    _form.Close();
                    _form.Dispose();
                    _form = null;
                }
                _notificationCallback = null;
            }
        } 
        #endregion


        #region NotificationForm Class
        private class NotificationForm : Form
        {
            private readonly ClipboardService _parent;
            
            public NotificationForm(ClipboardService parent)
            {
                _parent = parent;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
                {
                    _parent.OnClipboardUpdate(m);
                }
                base.WndProc(ref m);
            }

            public new void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            
            ~NotificationForm()
            {
                Dispose(false);
            }
            
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Parent = null;
                }
                base.Dispose(disposing);
            }
        }
        #endregion
    }
}
