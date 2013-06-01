using System;
using System.Windows.Forms;

namespace ClipboardHistory.Classes
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
            this._form = new NotificationForm(this);
            this.ClipboardUpdate += handler;
        } 
        #endregion


        #region Public Methods
		//sss
        public void DisableNotifications()
        {
            NativeMethods.RemoveClipboardFormatListener(this._form.Handle);
        }

        public void EnableNotifications()
        {
            NativeMethods.SetParent(this._form.Handle, NativeMethods.HWND_MESSAGE);
            NativeMethods.AddClipboardFormatListener(this._form.Handle);
        } 
        #endregion


        #region Event Handler
        private void OnClipboardUpdate(EventArgs e)
        {
            var handler = this.ClipboardUpdate;
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
        }
        ~ClipboardUpdateNotifier()
        {
            Dispose(false);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._form != null)
                {
                    this.DisableNotifications();
                    this._form.Close();
                    this._form.Dispose();
                    this._form = null;
                }
                this.ClipboardUpdate = null;
            }
        } 
        #endregion


        #region NotificationForm Class
        private class NotificationForm : Form
        {
            private ClipboardUpdateNotifier _parent;
            public NotificationForm(ClipboardUpdateNotifier parent)
            {
                this._parent = parent;
            }
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
                {
                    this._parent.OnClipboardUpdate(null);
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
                    this.Parent = null;
                }
                base.Dispose(disposing);
            }
        } 
        #endregion
    }
}
