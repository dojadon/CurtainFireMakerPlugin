using System;
using System.Xml;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CurtainFireMakerPlugin
{
    internal class Configuration
    {
        private string CondigPath { get; }

        private XmlDocument XmlDoc { get; }
        private XmlNode RootNode => XmlDoc.SelectSingleNode(@"//Configuration");

        private XmlNode NodeScripts => RootNode.SelectSingleNode("Scripts");

        private XmlNode NodeScript => NodeScripts.SelectSingleNode("Run");
        public string ScriptPath
        {
            get => GetAbsolutePath(NodeScript.InnerText);
            set => NodeScript.InnerText = GetRelativePath(value);
        }

        private XmlNode NodeShotTypeSettingScript => NodeScripts.SelectSingleNode("ShotTypeSetting");
        public string ShotTypeSettingScriptPath
        {
            get => GetAbsolutePath(NodeShotTypeSettingScript.InnerText);
            set => NodeShotTypeSettingScript.InnerText = GetRelativePath(value);
        }

        private XmlNode NodeLibs => RootNode.SelectSingleNode("Libs");
        public string[] ModullesDirPaths
        {
            get => (from XmlNode node in NodeLibs.ChildNodes select GetAbsolutePath(node.InnerText)).ToArray();
            set
            {
                for (int i = 0; i < NodeLibs.ChildNodes.Count; i++)
                {
                    NodeLibs.ChildNodes.Item(i).InnerText = GetRelativePath(value[i]);
                }
            }
        }
        private XmlNode NodeExport => RootNode.SelectSingleNode("Export");
        private XmlNode NodePmxExport => NodeExport.SelectSingleNode("Pmx");
        public string PmxExportDirPath
        {
            get => GetAbsolutePath(NodePmxExport.InnerText);
            set => NodePmxExport.InnerText = GetRelativePath(value);
        }
        private XmlNode NodeVndExport => NodeExport.SelectSingleNode("Vmd");
        public string VmdExportDirPath
        {
            get => GetAbsolutePath(NodeVndExport.InnerText);
            set => NodeVndExport.InnerText = GetRelativePath(value);
        }

        private XmlNode NodeKeepLogOpen => RootNode.SelectSingleNode("KeepLogOpen");
        public bool KeepLogOpen { get => bool.Parse(NodeKeepLogOpen.InnerText); set => NodeKeepLogOpen.InnerText = value.ToString(); }

        private XmlNode NodeDropPmxFile => RootNode.SelectSingleNode("DropPmxFile");
        public bool DropPmxFile { get => bool.Parse(NodeDropPmxFile.InnerText); set => NodeDropPmxFile.InnerText = value.ToString(); }

        private XmlNode NodeDropVmdFile => RootNode.SelectSingleNode("DropVmdFile");
        public bool DropVmdFile { get => bool.Parse(NodeDropVmdFile.InnerText); set => NodeDropVmdFile.InnerText = value.ToString(); }

        public string ResourceDirPath => Plugin.Instance.PluginRootPath + "\\Resource";

        public Configuration(string path)
        {
            CondigPath = path;

            XmlDoc = new XmlDocument();
        }

        public void Load() => XmlDoc.Load(CondigPath);

        public void Save() => XmlDoc.Save(CondigPath);

        private static string GetAbsolutePath(string path) => Path.IsPathRooted(path) ? path : Plugin.Instance.PluginRootPath + "\\" + path;

        private static string GetRelativePath(string path) => path.Replace(Plugin.Instance.PluginRootPath + "\\", "");
    }
}
