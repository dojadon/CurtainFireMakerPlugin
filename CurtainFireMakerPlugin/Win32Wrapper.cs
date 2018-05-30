using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CurtainFireMakerPlugin
{
    public static class Win32Wrapper
    {
        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int[] lParam);

        public static IntPtr SetTabLength(IntPtr hWnd, int length) => SendMessage(hWnd, 0x00CB, 1, new int[] { length });
    }
}
