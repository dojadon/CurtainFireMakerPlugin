using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class ProgressForm : Form
    {
        public int Maximum { get => progressBar.Maximum; set => progressBar.Maximum = value; }
        public int Value { get => progressBar.Value; set => progressBar.Value = value; }

        public bool IsCanceled => DialogResult == DialogResult.Cancel;

        public string LogText => richTextBoxLog.Text;

        public ProgressForm()
        {
            InitializeComponent();
        }

        public TextWriter CreateLogWriter() => new ControlWriter(richTextBoxLog);

        private void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }

    public class ControlWriter : TextWriter
    {
        private RichTextBox textbox;

        private StringBuilder content = new StringBuilder();

        public ControlWriter(RichTextBox textbox)
        {
            this.textbox = textbox;
        }

        public override void Write(char value)
        {
            content.Append(value);
        }

        public override void Write(string value)
        {
            content.Append(value);
        }

        public override void Flush()
        {
            if (content.Length != 0)
            {
                textbox.AppendText(content.ToString());
                content = new StringBuilder();
            }
            base.Flush();
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
