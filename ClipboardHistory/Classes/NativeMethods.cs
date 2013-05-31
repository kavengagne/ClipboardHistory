using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClipboardHistory.Classes
{
    internal static class NativeMethods
    {
        public delegate IntPtr HookProc(int code, IntPtr wParam, KBLLHOOKSTRUCT lParam);

        [StructLayout(LayoutKind.Sequential)]
        public class KBLLHOOKSTRUCT
        {
            public Keys vkCode;
            public uint scanCode;
            public KBLLHOOKSTRUCTFlags flags;
            public uint time;
            private UIntPtr dwExtraInfo;
        }
        [Flags]
        public enum KBLLHOOKSTRUCTFlags : uint
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }


        #region Constants
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_PASTE = 0x0302;
        public const int WM_CLIPBOARDUPDATE = 0x031D;
        public static IntPtr HWND_MESSAGE = new IntPtr(-3);
        #endregion


        #region Windows API Imports
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(int hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);
        
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr hookId);
        
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CallNextHookEx(IntPtr hookId, int nCode, IntPtr wParam, [In]KBLLHOOKSTRUCT lParam);
        
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern short GetAsyncKeyState(Keys key);
        
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetOpenClipboardWindow();
        #endregion
    }
}
