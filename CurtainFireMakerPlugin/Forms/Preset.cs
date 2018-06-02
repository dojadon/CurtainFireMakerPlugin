using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;

namespace CurtainFireMakerPlugin.Forms
{
    public class Preset : XmlConfig
    {
        private const string DefaultXml =
        "<?xml version = \"1.0\" encoding = \"UTF-8\"?>\n" +
        "<Configuration>\n" +
        "  <StartFrame>0</StartFrame>\n" +
        "  <EndFrame>1200</EndFrame>\n" +
        "  <Sequence></Sequence>\n" +
        "</Configuration>\n";

        public override XmlNode RootNode => Document.SelectSingleNode("//Configuration");

        public int StartFrame { get => GetInt("StartFrame"); set => SetValue("StartFrame", value); }
        public int EndFrame { get => GetInt("EndFrame"); set => SetValue("EndFrame", value); }

        public string[] SequenceScripts { get => GetPaths("Sequence/Path"); set => SetPaths("Sequence/Path", value); }

        public Preset()
        {
        }

        public override void Init()
        {
            base.Init();
            Document.LoadXml(DefaultXml);
        }

        public static bool IsFormated(string path)
        {
            return IsFormated(path, "//Configuration", "//Configuration/StartFrame", "//Configuration/EndFrame", "//Configuration/Sequence");
        }
    }
}
