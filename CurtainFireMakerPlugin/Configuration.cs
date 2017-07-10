using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CurtainFireMakerPlugin
{
    internal class Configuration
    {


        public static void Init(string path)
        {
            var doc = new XmlDocument();
            doc.Load(path);


        }
    }
}
