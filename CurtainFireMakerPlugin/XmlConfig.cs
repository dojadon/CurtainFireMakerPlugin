using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

namespace CurtainFireMakerPlugin
{
    public abstract class XmlConfig
    {
        public static Uri PluginRootUri { get; } = new Uri(Plugin.PluginRootPath);

        public XmlDocument Document { get; private set; }
        public abstract XmlNode RootNode { get; }

        public virtual void Init()
        {
            Document = new XmlDocument();
        }

        public void Load(string path) => Document.Load(path);

        public void Save(string path) => Document.Save(path);

        protected string GetPath(string xpath) => GetAbsolutePath(GetString(xpath));

        protected void SetPath(string xpath, string value) => RootNode.SelectSingleNode(xpath).InnerText = GetAbsolutePath(value);

        protected string GetString(string xpath) => RootNode.SelectSingleNode(xpath).InnerText;

        protected void SetString(string xpath, string value) => RootNode.SelectSingleNode(xpath).InnerText = value;

        protected bool GetBool(string xpath) => bool.Parse(RootNode.SelectSingleNode(xpath).InnerText);

        protected int GetInt(string xpath) => int.Parse(RootNode.SelectSingleNode(xpath).InnerText);

        protected void SetValue(string xpath, object value) => RootNode.SelectSingleNode(xpath).InnerText = value.ToString();

        protected string[] GetStrings(string xpath)
        {
            return (from XmlNode node in RootNode.SelectNodes(xpath) select node.InnerText).ToArray();
        }

        protected void SetStrings(string xpath, string[] values)
        {
            var nodes = RootNode.SelectNodes(xpath);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes.Item(i).InnerText = values[i];
            }
        }

        protected string[] GetPaths(string xpath)
        {
            return (from XmlNode node in RootNode.SelectNodes(xpath) select GetAbsolutePath(node.InnerText)).ToArray();
        }

        protected void SetPaths(string xpath, string[] values)
        {
            var parentNode = RootNode.SelectSingleNode(xpath.Substring(0, xpath.LastIndexOf('/')));
            var childPath = xpath.Substring(xpath.LastIndexOf('/') + 1);

            var nodes = RootNode.SelectNodes(xpath);

            foreach (var node in nodes)
            {
                parentNode.RemoveChild((XmlNode)node);
            }

            foreach (var value in values)
            {
                var node = Document.CreateElement(childPath);
                node.AppendChild(Document.CreateTextNode(value));
                parentNode.AppendChild(node);
            }
        }

        public static string GetAbsolutePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                Uri uri = new Uri(PluginRootUri, path);
                return uri.LocalPath;
            }
        }

        public static string GetRelativePath(string path)
        {
            return path.Replace(Plugin.PluginRootPath, "");
        }
    }
}
