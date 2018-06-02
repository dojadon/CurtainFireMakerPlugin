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
        "  <RecentSelectedPresetPath>新規</RecentSelectedPresetPath>\n" +
        "  <RecentPresetDirectories>\n" +
        "  </RecentPresetDirectories>\n" +
        "  <RecentScriptDirectories>\n" +
        "  </RecentScriptDirectories>\n" +
        "</Configuration>\n";

        public override XmlNode RootNode => Document.SelectSingleNode(@"//Configuration");

        public string RecentSelectedPresetPath { get => GetPath("RecentSelectedPresetPath"); set => SetPath("RecentSelectedPresetPath", value); }

        public string[] RecentPresetDirectories { get => GetPaths("RecentPresetDirectories/Path"); set => SetPaths("RecentPresetDirectories/Path", value); }

        public string[] RecentScriptDirectories { get => GetPaths("RecentScriptDirectories/Path"); set => SetPaths("RecentScriptDirectories/Path", value); }

        public override void Init()
        {
            base.Init();
            Document.LoadXml(DefaultXml);
        }
    }
}
