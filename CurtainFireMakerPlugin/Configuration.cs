using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace CurtainFireMakerPlugin
{
    internal class Configuration
    {
        private static string CondigPath => Plugin.Instance.CurtainFireMakerPath + "\\config.xml";

        public static void Load()
        {
            var doc = new XmlDocument();
            doc.Load(CondigPath);

            XmlNode rootNode = doc.SelectSingleNode(@"//configuration");
            Plugin.Instance.ScriptPath = GetPath(rootNode.SelectSingleNode("script").InnerText);
            Plugin.Instance.SettingScriptPath = GetPath(rootNode.SelectSingleNode("setting").InnerText);
            Plugin.Instance.ModullesDirPath = GetPath(rootNode.SelectSingleNode("scripts").InnerText);
            Plugin.Instance.ExportDirPath = GetPath(rootNode.SelectSingleNode("export").InnerText);
            Plugin.Instance.ModelName = rootNode.SelectSingleNode("model_name").InnerText;
            Plugin.Instance.ModelDescription = rootNode.SelectSingleNode("model_description").InnerText;
            Plugin.Instance.KeepLogOpen = bool.Parse(rootNode.SelectSingleNode("keep_log_open").InnerText);
        }

        public static void Save()
        {
            var doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.CreateElement("configuration");

            doc.AppendChild(declaration);
            doc.AppendChild(root);

            XmlElement element = doc.CreateElement("script");
            element.InnerText = Plugin.Instance.ScriptPath.Replace(Plugin.Instance.CurtainFireMakerPath + "\\", "");
            root.AppendChild(element);

            element = doc.CreateElement("setting");
            element.InnerText = Plugin.Instance.SettingScriptPath.Replace(Plugin.Instance.CurtainFireMakerPath + "\\", "");
            root.AppendChild(element);

            element = doc.CreateElement("scripts");
            element.InnerText = Plugin.Instance.ModullesDirPath.Replace(Plugin.Instance.CurtainFireMakerPath + "\\", "");
            root.AppendChild(element);

            element = doc.CreateElement("export");
            element.InnerText = Plugin.Instance.ExportDirPath.Replace(Plugin.Instance.CurtainFireMakerPath + "\\", "");
            root.AppendChild(element);

            element = doc.CreateElement("model_name");
            element.InnerText = Plugin.Instance.ModelName;
            root.AppendChild(element);

            element = doc.CreateElement("model_description");
            element.InnerText = Plugin.Instance.ModelDescription;
            root.AppendChild(element);

            element = doc.CreateElement("keep_log_open");
            element.InnerText = Plugin.Instance.KeepLogOpen.ToString();
            root.AppendChild(element);

            doc.Save(CondigPath);
        }

        private static string GetPath(string path)
        {
            return Path.IsPathRooted(path) ? path : Plugin.Instance.CurtainFireMakerPath + "\\" + path;
        }
    }
}
