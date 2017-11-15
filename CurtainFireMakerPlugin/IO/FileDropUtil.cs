using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.IO
{
    internal class FileDropUtil
    {
        public static void Drop(IntPtr hWnd, StringCollection filePaths)
        {
            Clipboard.Clear();
            Clipboard.SetFileDropList(filePaths);
            var data = Clipboard.GetDataObject();

            IDropTarget target = Control.FromHandle(hWnd);
            DragDropEffects dwEffect = DragDropEffects.Copy | DragDropEffects.Link;

            target.OnDragDrop(new DragEventArgs(data, 0, 0, 0, dwEffect, dwEffect));

            Clipboard.Clear();
        }
    }
}
