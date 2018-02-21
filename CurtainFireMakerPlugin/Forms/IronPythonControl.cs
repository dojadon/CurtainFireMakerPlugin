using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class IronPythonControl : UserControl
    {
        public string ScriptText { get => textBoxScript.Text; set => textBoxScript.Text = value.Replace("\r\n", "\n").Replace("\n", "\r").Replace("\r", "\r\n"); }

        public IronPythonControl()
        {
            InitializeComponent();
        }
    }
}
