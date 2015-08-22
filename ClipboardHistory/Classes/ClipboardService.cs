using System;
using System.Windows.Forms;
using ClipboardHistoryApp.AppResources;
using ClipboardHistoryApp.Helpers;
using ClipboardHistoryApp.Models;

namespace ClipboardHistoryApp.Classes
{
    public sealed class ClipboardService : IDisposable
    {
        #region Fields
        private Action<ClipboardDataItem> _notificationCallback;
        private NotificationForm _form;
        private IntPtr _visualStudioHandle;
        #endregion

        
        #region Constructors
        public ClipboardService(Action<ClipboardDataItem> notificationCallback)
        {
            _form = new NotificationForm(this);
            _notificationCallback = notificationCallback;            
        } 
        #endregion


        #region Public Methods
        public void DisableNotifications()
        {
            NativeMethods.RemoveClipboardFormatListener(_form.Handle);
        }

        public void EnableNotifications()
        {
            NativeMethods.SetParent(_form.Handle, NativeMethods.HWND_MESSAGE);
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
        private void OnClipboardUpdate()
        {
            var handler = _notificationCallback;
            if (handler != null)
            {
                var topWindowHandle = WindowHelper.GetTopWindowHandle();
                if (Configuration.VisualStudioClipboardOnly && topWindowHandle != _visualStudioHandle)
                {
                    return;
                }
                handler.Invoke(GetClipboardDataItem());
            }
        }
        #endregion


        #region Private Methods

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
                    _parent.OnClipboardUpdate();
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
