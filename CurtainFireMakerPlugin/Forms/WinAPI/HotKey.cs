using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms.WinAPI
{
    public abstract class HotKey
    {
        [DllImport("user32", SetLastError = true)]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, int fsModifier, int vk);

        [DllImport("user32", SetLastError = true)]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

        public IntPtr LParam { get; }
        public IntPtr Handle { get; private set; }
        public int Id { get; }

        public int KeyCode { get; }
        public int Modifiers { get; }

        public HotKey(int id, Keys key)
        {
            Id = id;

            KeyCode = (int)(key & Keys.KeyCode);
            Modifiers = (int)(key & Keys.Modifiers) >> 16;

            LParam = new IntPtr(Modifiers | KeyCode << 16);
        }

        public void Register(IntPtr hWnd)
        {
            Handle = hWnd;

            if (RegisterHotKey(Handle, Id, Modifiers, KeyCode) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public void Unregister()
        {
            if (Handle == IntPtr.Zero)
                return;
            if (UnregisterHotKey(Handle, Id) == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            Handle = IntPtr.Zero;
        }

        const int WM_HOTKEY = 0x312;

        public bool WndProc(Message m, MikuMikuPlugin.Scene scene)
        {
            if (m.Msg == WM_HOTKEY && m.LParam == LParam)
            {
                return OnMessage(m, scene);
            }
            return false;
        }

        public abstract bool OnMessage(Message m, MikuMikuPlugin.Scene scene);
    }
}
