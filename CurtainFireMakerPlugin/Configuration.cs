using System;
using System.Xml;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin
{
    public class Configuration
    {
        private string ConfigPath { get; }

        private XmlDocument XmlDoc { get; }
        private XmlNode RootNode => XmlDoc.SelectSingleNode(@"//Configuration");

        private XmlNode NodeScripts => RootNode.SelectSingleNode("Scripts");

        private XmlNode NodeRunScript => NodeScripts.SelectSingleNode("Run");
        public string ScriptPath
        {
            get => GetAbsolutePath(NodeRunScript.InnerText);
            set => NodeRunScript.InnerText = GetRelativePath(value);
        }

        private XmlNode NodeInitScript => NodeScripts.SelectSingleNode("Init");
        public string InitScriptPath
        {
            get => GetAbsolutePath(NodeInitScript.InnerText);
            set => NodeInitScript.InnerText = GetRelativePath(value);
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

        private XmlNode NodeLog => RootNode.SelectSingleNode("Log");
        public string LogPath
        {
            get
            {
                try { return GetAbsolutePath(NodeLog.InnerText); }
                catch { return "..\\lastest.log"; }
            }
            set => NodeLog.InnerText = value;
        }

        private XmlNode NodeKeepLogOpen => RootNode.SelectSingleNode("KeepLogOpen");
        public bool KeepLogOpen { get => bool.Parse(NodeKeepLogOpen.InnerText); set => NodeKeepLogOpen.InnerText = value.ToString(); }

        private XmlNode NodeDropPmxFile => RootNode.SelectSingleNode("DropPmxFile");
        public bool DropPmxFile { get => bool.Parse(NodeDropPmxFile.InnerText); set => NodeDropPmxFile.InnerText = value.ToString(); }

        private XmlNode NodeDropVmdFile => RootNode.SelectSingleNode("DropVmdFile");
        public bool DropVmdFile { get => bool.Parse(NodeDropVmdFile.InnerText); set => NodeDropVmdFile.InnerText = value.ToString(); }

        public static string PluginRootPath => Application.StartupPath + "\\CurtainFireMaker\\";
        public static string SettingXmlFilePath => PluginRootPath + "config.xml";
        public static string ResourceDirPath => PluginRootPath + "Resource\\";

        public static Uri PluginRootUri { get; } = new Uri(PluginRootPath);

        public Configuration(string path)
        {
            ConfigPath = path;

            XmlDoc = new XmlDocument();
        }

        public void Load() => XmlDoc.Load(ConfigPath);

        public void Save() => XmlDoc.Save(ConfigPath);

        private static string GetAbsolutePath(string path)
        {
            if (path.StartsWith("C:"))
            {
                return path;
            }
            else
            {
                Uri uri = new Uri(PluginRootUri, path);
                return uri.LocalPath;
            }
        }

        private static string GetRelativePath(string path)
        {
            return path.Replace(PluginRootPath, "");
        }
    }
}
