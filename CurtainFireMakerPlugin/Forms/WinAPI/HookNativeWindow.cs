using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms.WinAPI
{
    public class HookNativeWindow : NativeWindow
    {
        public List<HotKey> HotKeys { get; } = new List<HotKey>();
        public Plugin Plugin { get; }

        public HookNativeWindow(Plugin plugin)
        {
            Plugin = plugin;
        }

        public void StartHook(Control c)
        {
            AssignHandle(c.Handle);
            c.HandleDestroyed += new EventHandler(OnHandleDestroyed);
            c.HandleCreated += new EventHandler(OnHandleCreated);

            HotKeys.ForEach(h => h.Register(c.Handle));
        }

        public void RegisterHotKey(HotKey key)
        {
            HotKeys.Add(key);
        }

        public void RegisterHotKeys(params HotKey[] key)
        {
            HotKeys.AddRange(key);
        }

        internal void OnHandleDestroyed(object sender, EventArgs e)
        {
            ReleaseHandle();

            HotKeys.ForEach(h => h.Unregister());
        }

        internal void OnHandleCreated(object sender, EventArgs e)
        {
            if (sender is Control c)
            {
                AssignHandle(c.Handle);
                HotKeys.ForEach(h => h.Register(c.Handle));
            }
        }

        const int WM_HOTKEY = 0x312;

        protected override void WndProc(ref Message m)
        {
            var msg = m;

            if (!HotKeys.Any(h => h.WndProc(msg, Plugin.Scene)))
            {
                base.WndProc(ref m);
            }
        }
    }
}
