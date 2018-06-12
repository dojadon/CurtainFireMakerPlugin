using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

namespace CurtainFireMakerPlugin
{
    public abstract class XmlConfig
    {
        public XmlDocument Document { get; private set; }
        public abstract XmlNode RootNode { get; }

        public virtual void Init()
        {
            Document = new XmlDocument();
        }

        public void Load(string path) => Document.Load(path);

        public void Save(string path) => Document.Save(path);

        protected XmlNode GetNode(string xpath, object defaultVal)
        {
            var node = RootNode.SelectSingleNode(xpath);

            if (node == null)
            {
                node = Document.CreateElement(xpath);
                node.AppendChild(Document.CreateTextNode(defaultVal.ToString()));
                RootNode.AppendChild(node);
            }
            return node;
        }

        protected XmlNodeList GetNodes(string xpath)
        {
            return RootNode.SelectNodes(xpath);
        }

        protected string GetString(string xpath, object defaultVal) => GetNode(xpath, defaultVal).InnerText;

        protected void SetString(string xpath, object defaultVal, string value) => GetNode(xpath, defaultVal).InnerText = value;

        protected bool GetBool(string xpath, object defaultVal) => bool.Parse(GetNode(xpath, defaultVal).InnerText);

        protected int GetInt(string xpath, object defaultVal) => int.Parse(GetNode(xpath, defaultVal).InnerText);

        protected void SetValue(string xpath, object defaultVal, object value) => GetNode(xpath, defaultVal).InnerText = value.ToString();

        protected string[] GetStrings(string xpath)
        {
            return (from XmlNode node in GetNodes(xpath) select node.InnerText).ToArray();
        }

        protected void SetStrings(string xpath, string[] values)
        {
            var parentNode = RootNode.SelectSingleNode(xpath.Substring(0, xpath.LastIndexOf('/')));
            var childPath = xpath.Substring(xpath.LastIndexOf('/') + 1);

            var nodes = GetNodes(xpath);

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

        public static bool IsFormated(string path, params string[] xpaths)
        {
            var doc = new XmlDocument();
            doc.Load(path);

            return xpaths.All(p => doc.SelectNodes(p).Count > 0);
        }
    }
}
