using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ClipboardHistoryApp.Classes
{
    public sealed class ClipboardUpdateNotifier : IDisposable
    {
        #region Fields
        private event EventHandler ClipboardUpdate;
        private NotificationForm _form; 
        #endregion

        
        #region Constructors
        public ClipboardUpdateNotifier(EventHandler handler)
        {
            _form = new NotificationForm(this);
            ClipboardUpdate += handler;
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
        #endregion


        #region Event Handler
        private void OnClipboardUpdate(ClipboardEventArgs e)
        {
            Debug.WriteLine(e.Hwnd.ToString());
            var handler = ClipboardUpdate;
            if (handler != null)
            {
                handler(null, e);
            }
        } 
        #endregion


        #region IDisposable Interface / Resources Cleanup
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ClipboardUpdateNotifier()
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
                ClipboardUpdate = null;
            }
        } 
        #endregion


        #region NotificationForm Class
        private class NotificationForm : Form
        {
            private readonly ClipboardUpdateNotifier _parent;
            
            public NotificationForm(ClipboardUpdateNotifier parent)
            {
                _parent = parent;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
                {
                    _parent.OnClipboardUpdate(new ClipboardEventArgs());
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
