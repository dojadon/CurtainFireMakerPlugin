using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CurtainFireMakerPlugin.IO
{
    public class ActionTextWriter : TextWriter
    {
        private readonly Action<string> action;

        public ActionTextWriter(Action<string> action)
        {
            this.action = action;
        }

        public override void Write(object value)
        {
            this.action(value.ToString());
        }

        public override void Write(string value)
        {
            this.action(value);
        }

        public override void Write(string format, object arg0)
        {
            this.Write(string.Format(format, arg0));
        }

        public override Encoding Encoding => Encoding.Unicode;
    }
}
