using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace CurtainFireMakerPlugin
{
    internal class Configuration
    {
        public static void Load()
        {
            var doc = new XmlDocument();
            doc.Load(Plugin.Instance.CurtainFireMakerPath + "\\config.xml");

            XmlNode rootNode = doc.SelectSingleNode(@"//configuration");
            Plugin.Instance.ScriptPath = GetPath(rootNode.SelectSingleNode("script").InnerText);
            Plugin.Instance.ExportPmxPath = GetPath(rootNode.SelectSingleNode("export_pmx").InnerText);
            Plugin.Instance.ExportVmdPath = GetPath(rootNode.SelectSingleNode("export_vmd").InnerText);
            Plugin.Instance.ModelName = rootNode.SelectSingleNode("model_name").InnerText;
            Plugin.Instance.ModelDescription = rootNode.SelectSingleNode("model_description").InnerText;
        }

        private static string GetPath(string path)
        {
            return Path.IsPathRooted(path) ? path : Plugin.Instance.CurtainFireMakerPath + "\\" + path;
        }
    }
}
