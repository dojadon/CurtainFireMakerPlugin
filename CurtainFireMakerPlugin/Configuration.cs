using System;
using System.Xml;
using System.IO;
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
            get => MightMakeAbsolute(NodeScript.InnerText);
            set => NodeScript.InnerText = MightMakeRelative(value);
        }

        private XmlNode NodeSettingScript => NodeScripts.SelectSingleNode("Setting");
        public string SettingScriptPath
        {
            get => MightMakeAbsolute(NodeSettingScript.InnerText);
            set => NodeSettingScript.InnerText = MightMakeRelative(value);
        }

        private XmlNode NodeLibs => RootNode.SelectSingleNode("Libs");
        public string[] ModullesDirPaths
        {
            get
            {
                var array = new string[NodeLibs.ChildNodes.Count];
                for (int i = 0; i < NodeLibs.ChildNodes.Count; i++)
                {
                    array[i] = MightMakeAbsolute(NodeLibs.ChildNodes.Item(i).InnerText);
                }
                return array;
            }
            set
            {
                for (int i = 0; i < NodeLibs.ChildNodes.Count; i++)
                {
                    NodeLibs.ChildNodes.Item(i).InnerText = MightMakeRelative(value[i]);
                }
            }
        }
        private XmlNode NodeExport => RootNode.SelectSingleNode("Export");
        public string ExportDirPath
        {
            get => MightMakeAbsolute(NodeExport.InnerText);
            set => NodeExport.InnerText = MightMakeRelative(value);
        }

        private XmlNode NodeStartFrame => RootNode.SelectSingleNode("StartFrame");
        public int StartFrame { get => int.Parse(NodeStartFrame.InnerText); set => NodeStartFrame.InnerText = value.ToString(); }

        private XmlNode NodeModel => RootNode.SelectSingleNode("Model");

        private XmlNode NodeModelName => NodeModel.SelectSingleNode("Name");
        public string ModelName { get => NodeModelName.InnerText; set => NodeModelName.InnerText = value; }

        private XmlNode NodeModelDescription => NodeModel.SelectSingleNode("Description");
        public string ModelDescription { get => NodeModelDescription.InnerText; set => NodeModelDescription.InnerText = value; }

        private XmlNode NodeKeepLogOpen => RootNode.SelectSingleNode("KeepLogOpen");
        public bool KeepLogOpen { get => bool.Parse(NodeKeepLogOpen.InnerText); set => NodeKeepLogOpen.InnerText = value.ToString(); }

        public string ResourceDirPath => Plugin.Instance.PluginRootPath + "\\Resource";

        public Configuration(string path)
        {
            CondigPath = path;

            XmlDoc = new XmlDocument();
        }

        public void Load() => XmlDoc.Load(CondigPath);

        public void Save() => XmlDoc.Save(CondigPath);

        private static string MightMakeAbsolute(string path) => Path.IsPathRooted(path) ? path : Plugin.Instance.PluginRootPath + "\\" + path;

        private static string MightMakeRelative(string path) => path.Replace(Plugin.Instance.PluginRootPath + "\\", "");
    }
}
