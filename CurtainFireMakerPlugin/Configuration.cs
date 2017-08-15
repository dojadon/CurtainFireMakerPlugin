using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace CurtainFireMakerPlugin
{
    internal class Configuration
    {
        private string CondigPath { get; }

        public string ScriptPath { get; set; }
        public string SettingScriptPath { get; set; }
        public string[] ModullesDirPaths { get; set; }
        public string ExportDirPath { get; set; }

        public string ModelName { get; set; }
        public string ModelDescription { get; set; }
        public bool KeepLogOpen { get; set; }

        public string ResourceDirPath => Plugin.Instance.PluginRootPath + "\\Resource";

        public Configuration(string path)
        {
            CondigPath = path;

            var doc = new XmlDocument();
            doc.Load(CondigPath);

            XmlNode rootNode = doc.SelectSingleNode(@"//Configuration");

            XmlNode scriptNode = rootNode.SelectSingleNode("Scripts");

            ScriptPath = GetPath(scriptNode.SelectSingleNode("Run").InnerText);
            SettingScriptPath = GetPath(scriptNode.SelectSingleNode("Setting").InnerText);

            XmlNode libNode = rootNode.SelectSingleNode("Libs");
            var list = new List<string>();
            foreach (var node in libNode.ChildNodes)
            {
                list.Add(((XmlNode)node).InnerText);
            }
            ModullesDirPaths = list.ToArray();

            ExportDirPath = GetPath(rootNode.SelectSingleNode("Export").InnerText);

            XmlNode modelNode = rootNode.SelectSingleNode("Model");

            ModelName = modelNode.SelectSingleNode("Name").InnerText;
            ModelDescription = modelNode.SelectSingleNode("Description").InnerText;
            KeepLogOpen = bool.Parse(rootNode.SelectSingleNode("KeepLogOpen").InnerText);
        }

        public void Save()
        {
            var doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.CreateElement("configuration");

            doc.AppendChild(declaration);
            doc.AppendChild(root);

            XmlElement element;
            XmlElement scriptEle = doc.CreateElement("Scripts");
            root.AppendChild(scriptEle);

            element = doc.CreateElement("Run");
            element.InnerText = ScriptPath.Replace(Plugin.Instance.PluginRootPath + "\\", "");
            scriptEle.AppendChild(element);

            element = doc.CreateElement("Setting");
            element.InnerText = SettingScriptPath.Replace(Plugin.Instance.PluginRootPath + "\\", "");
            scriptEle.AppendChild(element);

            XmlElement libEle = doc.CreateElement("Libs");
            root.AppendChild(libEle);

            foreach (var path in ModullesDirPaths)
            {
                element = doc.CreateElement("Dir");
                element.InnerText = path.Replace(Plugin.Instance.PluginRootPath + "\\", "");
                libEle.AppendChild(element);
            }

            element = doc.CreateElement("Export");
            element.InnerText = ExportDirPath.Replace(Plugin.Instance.PluginRootPath + "\\", "");
            root.AppendChild(element);

            XmlElement modelEle = doc.CreateElement("Model");
            root.AppendChild(scriptEle);

            element = doc.CreateElement("Name");
            element.InnerText = ModelName;
            modelEle.AppendChild(element);

            element = doc.CreateElement("Description");
            element.InnerText = ModelDescription;
            modelEle.AppendChild(element);

            element = doc.CreateElement("KeepLogOpen");
            element.InnerText = KeepLogOpen.ToString();
            root.AppendChild(element);

            doc.Save(CondigPath);
        }

        private static string GetPath(string path)
        {
            return Path.IsPathRooted(path) ? path : Plugin.Instance.PluginRootPath + "\\" + path;
        }
    }
}
