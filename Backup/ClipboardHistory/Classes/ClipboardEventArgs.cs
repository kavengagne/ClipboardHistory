using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClipboardHistory.Classes
{
	public sealed class ClipboardEventArgs : EventArgs
	{
		private IntPtr _hwnd = IntPtr.Zero;

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
