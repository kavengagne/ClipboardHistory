using System;
using ClipboardHistoryApp.Classes;

namespace ClipboardHistoryApp.Helpers
{
    public static class WindowHelper
    {
        public static IntPtr GetTopWindowHandle()
        {
            IntPtr topWindow = NativeMethods.GetForegroundWindow();
            IntPtr ownerWindow = NativeMethods.GetWindow(topWindow, NativeMethods.GetWindow_Cmd.GW_OWNER);
            if (ownerWindow != IntPtr.Zero)
            {
                topWindow = ownerWindow;
            }
            return topWindow;
        }
    }
}