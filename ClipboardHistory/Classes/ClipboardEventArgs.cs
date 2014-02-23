using System;
using System.Linq;

namespace ClipboardHistoryApp.Classes
{
	public sealed class ClipboardEventArgs : EventArgs
	{
		private readonly IntPtr _hwnd = IntPtr.Zero;

		public IntPtr Hwnd
		{
			get { return this._hwnd; }
		}

		public ClipboardEventArgs()
		{
			IntPtr topWindow = NativeMethods.GetForegroundWindow();
			IntPtr ownerWindow = NativeMethods.GetWindow(topWindow, NativeMethods.GetWindow_Cmd.GW_OWNER);
			if (ownerWindow != IntPtr.Zero)
			{
				topWindow = ownerWindow;
			}
			this._hwnd = topWindow;
		}
	}
}
