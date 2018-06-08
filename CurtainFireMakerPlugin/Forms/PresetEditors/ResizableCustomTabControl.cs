using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms.PresetEditors
{
    class ResizableCustomTabControl : CustomTabControl
    {
        private Point Point = new Point();
        private Cursor CurrentCursor { get; set; }
        private bool IsDraggable { get; set; } = false;
        private bool IsResizable { get; set; } = false;

        public ResizableCustomTabControl() : base()
        {
            MouseDown += CustomTabControl_MouseDown;
            MouseMove += CustomTabControl_MouseMove;
            MouseUp += CustomTabControl_MouseUp;
        }

        private void CustomTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = GetCursor(e.X, e.Y);
            CurrentCursor = Cursor.Current;

            IsDraggable = true;
            IsResizable = Cursors.Default != Cursor.Current;

            Point.X = e.X;
            Point.Y = e.Y;
        }

        private void CustomTabControl_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = GetCursor(e.X, e.Y);

            if (IsResizable)
            {
                if (CurrentCursor == Cursors.SizeNS || CurrentCursor == Cursors.SizeNWSE)
                {
                    Height += e.Y - Point.Y;
                }

                if (CurrentCursor == Cursors.SizeWE || CurrentCursor == Cursors.SizeNWSE)
                {
                    Width += e.X - Point.X;
                }

                Point.X = e.X;
                Point.Y = e.Y;
            }
            else if (IsDraggable)
            {
                Left += e.X - Point.X;
                Top += e.Y - Point.Y;
            }
        }

        private Cursor GetCursor(int x, int y)
        {
            if (IsResizable) return CurrentCursor;

            bool isRight = x >= Size.Width - 2 && x <= Size.Width - 1;
            bool isBottom = y >= Size.Height - 2 && y <= Size.Height - 1;

            if (isRight && isBottom) return Cursors.SizeNWSE;

            if (isRight) return Cursors.SizeWE;

            if (isBottom) return Cursors.SizeNS;

            return Cursors.Default;
        }

        private void CustomTabControl_MouseUp(object sender, MouseEventArgs e)
        {
            IsDraggable = false;
            IsResizable = false;
        }
    }
}
