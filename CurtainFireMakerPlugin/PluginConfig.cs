using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

namespace CurtainFireMakerPlugin
{
    public class PluginConfig : XmlConfig
    {
        public const string DefaultXml =
        "<?xml version = \"1.0\" encoding = \"UTF-8\"?>\n" +
        "<Configuration>\n" +
        "  <PythonLibDirectory></PythonLibDirectory>" +
        "  <TotalTime>0</TotalTime>" +
        "  <RecentPresetDirectories>\n" +
        "  </RecentPresetDirectories>\n" +
        "  <RecentScriptDirectories>\n" +
        "  </RecentScriptDirectories>\n" +
        "</Configuration>\n";

        public override XmlNode RootNode => Document.SelectSingleNode(@"//Configuration");

        public string PythonLibDirectory { get => GetString("PythonLibDirectory", ""); set => SetString("PythonLibDirectory", "", value); }

        public string[] RecentPresetDirectories { get => GetStrings("RecentPresetDirectories/Path"); set => SetStrings("RecentPresetDirectories/Path", value); }

        public string[] RecentScriptDirectories { get => GetStrings("RecentScriptDirectories/Path"); set => SetStrings("RecentScriptDirectories/Path", value); }

        public int TotalTime { get => GetInt("TotalTime", 0); set => SetValue("TotalTime", 0, value); }

        public override void Init()
        {
            base.Init();
            Document.LoadXml(DefaultXml);
        }
    }
}
