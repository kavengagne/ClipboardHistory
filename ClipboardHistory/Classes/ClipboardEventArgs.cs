using System;

namespace ClipboardHistoryApp.Classes
{
	public sealed class ClipboardEventArgs : EventArgs
	{
		private readonly IntPtr _hwnd;

		public IntPtr Hwnd
		{
			get { return _hwnd; }
		}

		public ClipboardEventArgs()
		{
			IntPtr topWindow = NativeMethods.GetForegroundWindow();
			IntPtr ownerWindow = NativeMethods.GetWindow(topWindow, NativeMethods.GetWindow_Cmd.GW_OWNER);
			if (ownerWindow != IntPtr.Zero)
			{
				topWindow = ownerWindow;
			}
			_hwnd = topWindow;
		}
	}
}
