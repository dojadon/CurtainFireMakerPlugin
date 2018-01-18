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

        public string ScriptPath { get => GetPath("Script"); set => SetPath("Script", value); }
        public string CommonScriptPath { get => GetPath("CommonScript"); set => SetPath("CommonScript", value); }

        public string[] ModullesDirPaths { get => GetPaths("Libs/Dir"); set => SetPaths("Libs/Dir", value); }

        public string PmxExportDirPath { get => GetPath("Export/Pmx"); set => SetPath("Export/Pmx", value); }
        public string VmdExportDirPath { get => GetPath("Export/Vmd"); set => SetPath("Export/Vmd", value); }

        public bool ShouldDropPmxFile { get => GetBool("ShouldDropFile/Pmx"); set => SetBool("ShouldDropFile/Pmx", value); }
        public bool ShouldDropVmdFile { get => GetBool("ShouldDropFile/Vmd"); set => SetBool("ShouldDropFile/Vmd", value); }

        public static string PluginRootPath => Application.StartupPath + "\\CurtainFireMaker\\";
        public static string SettingXmlFilePath => PluginRootPath + "config.xml";
        public static string SettingPythonFilePath => PluginRootPath + "config.py";
        public static string ResourceDirPath => PluginRootPath + "Resource\\";
        public static string LogPath => PluginRootPath + "lastest.log";

        public static Uri PluginRootUri { get; } = new Uri(PluginRootPath);

        public Configuration(string path)
        {
            ConfigPath = path;

            XmlDoc = new XmlDocument();
        }

        public void Load() => XmlDoc.Load(ConfigPath);

        public void Save() => XmlDoc.Save(ConfigPath);

        public string GetPath(string xpath) => GetAbsolutePath(GetString(xpath));

        public void SetPath(string xpath, string value) => RootNode.SelectSingleNode(xpath).InnerText = GetAbsolutePath(value);

        public string GetString(string xpath) => RootNode.SelectSingleNode(xpath).InnerText;

        public void SetString(string xpath, string value) => RootNode.SelectSingleNode(xpath).InnerText = value;

        public bool GetBool(string xpath) => bool.Parse(RootNode.SelectSingleNode(xpath).InnerText);

        public void SetBool(string xpath, bool value) => RootNode.SelectSingleNode(xpath).InnerText = value.ToString();

        public string[] GetStrings(string xpath)
        {
            return (from XmlNode node in RootNode.SelectNodes(xpath) select node.InnerText).ToArray();
        }

        public void SetStrings(string xpath, string[] values)
        {
            var nodes = RootNode.SelectNodes(xpath);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes.Item(i).InnerText = values[i];
            }
        }

        public string[] GetPaths(string xpath)
        {
            return (from XmlNode node in RootNode.SelectNodes(xpath) select GetAbsolutePath(node.InnerText)).ToArray();
        }

        public void SetPaths(string xpath, string[] values)
        {
            var nodes = RootNode.SelectNodes(xpath);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes.Item(i).InnerText = GetRelativePath(values[i]);
            }
        }

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
