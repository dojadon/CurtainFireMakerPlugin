using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using CurtainFireCore;

namespace CurtainFireMakerPlugin.Forms
{
    public class Preset : XmlConfig
    {
        private const string DefaultXml =
        "<?xml version = \"1.0\" encoding = \"UTF-8\"?>\n" +
        "<Configuration>\n" +
        "  <CFMPluginVersion>1.0</CFMPluginVersion>\n" +
        "  <StartFrame>0</StartFrame>\n" +
        "  <EndFrame>1200</EndFrame>\n" +
        "  <BackGround>False</BackGround>\n" +
        "  <Sequence></Sequence>\n" +
        "</Configuration>\n";

        public override XmlNode RootNode => Document.SelectSingleNode("//Configuration");

        public int StartFrame { get => GetInt("StartFrame", 0); set => SetValue("StartFrame", 0, value); }
        public int EndFrame { get => GetInt("EndFrame", 1200); set => SetValue("EndFrame", 1200, value); }

        public bool BackGround { get => GetBool("BackGround", false); set => SetValue("BackGround", false, value); }

        public string[] SequenceScripts { get => GetStrings("Sequence/Path"); set => SetStrings("Sequence/Path", value); }

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
            return IsFormated(path, "//Configuration/CFMPluginVersion");
        }
    }
}
