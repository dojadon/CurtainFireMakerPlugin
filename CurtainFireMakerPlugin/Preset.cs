using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace CurtainFireMakerPlugin
{
    public class Preset
    {
        public const string TemplatePresetName = "Template";

        public const string DefaultConfig =
        "﻿<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
        "<Configuration>\n" +
        "  <Name>Preset</Name>\n" +
        "  <Export>\n" +
        "    <Pmx>Export</Pmx>\n" +
        "    <Vmd>Export</Vmd>\n" +
        "  </Export>\n" +
        "  <Scripts></Scripts>\n" +
        "</Configuration>";

        public const string DefaultPreScript =
        "# -*- coding: utf-8 -*-\r\n" +
        "WORLD.MaxFrame = 1200\r\n" +
        "WORLD.FrameCount = 0";

        public const string ConfigFileName = "config.xml";

        private Guid Guid { get; }

        private XmlNode PresetNameNode => XmlDoc.SelectSingleNode("//Configuration/Name");
        public string PresetName { get => PresetNameNode.InnerText; set => PresetNameNode.InnerText = value; }

        public string PresetDirPath => Configuration.PresetsDirPath + Guid + "\\";
        public string PreScriptDirPath => PresetDirPath + "PreScript\\";
        public string ConfigFilePath => PresetDirPath + ConfigFileName;
        public string RootScriptFilePath => PresetDirPath + "common.py";

        private XmlNode ExportDirPmxNode => XmlDoc.SelectSingleNode("//Configuration/Export/Pmx");
        public string ExportDirPmx
        {
            get => Configuration.GetAbsolutePath(ExportDirPmxNode.InnerText);
            set => ExportDirPmxNode.InnerText = Configuration.GetRelativePath(value);
        }

        private XmlNode ExportDirVmdNode => XmlDoc.SelectSingleNode("//Configuration/Export/Vmd");
        public string ExportDirVmd
        {
            get => Configuration.GetAbsolutePath(ExportDirVmdNode.InnerText);
            set => ExportDirVmdNode.InnerText = Configuration.GetRelativePath(value);
        }

        public string RootScript { get; set; }

        public List<ScriptFile> Scripts { get; } = new List<ScriptFile>();

        private XmlDocument XmlDoc { get; } = new XmlDocument();

        public Preset(Guid guid)
        {
            Guid = guid;

            CreateRequiredFiles();

            Load();
        }

        private void Load()
        {
            XmlDoc.Load(ConfigFilePath);

            Dictionary<string, string> scriptPaths =
            (from XmlNode node in XmlDoc.SelectNodes("//Configuration/Scripts/Path") select node.InnerText).Distinct().ToDictionary(s => Path.GetFileNameWithoutExtension(s));

            foreach (var path in Directory.GetFiles(PreScriptDirPath).Where(p => p.EndsWith(".py")))
            {
                string name = Path.GetFileNameWithoutExtension(path);

                if (scriptPaths.ContainsKey(name))
                {
                    Scripts.Add(new ScriptFile(scriptPaths[name]) { PreScript = File.ReadAllText(path) });
                }
            }
            RootScript = File.ReadAllText(RootScriptFilePath);
        }

        private void CreateRequiredFiles()
        {
            if (!Directory.Exists(PresetDirPath))
            {
                Directory.CreateDirectory(PresetDirPath);
            }

            if (!File.Exists(ConfigFilePath))
            {
                File.WriteAllText(ConfigFilePath, DefaultConfig);
            }

            if (!File.Exists(RootScriptFilePath))
            {
                File.Copy(Configuration.CommonScriptPath, RootScriptFilePath);
            }

            if (!Directory.Exists(PreScriptDirPath))
            {
                Directory.CreateDirectory(PreScriptDirPath);
            }
        }

        public void Save()
        {
            CreateRequiredFiles();

            XmlNode scriptsNode = XmlDoc.SelectSingleNode("//Configuration/Scripts");

            if (scriptsNode != null)
            {
                foreach (XmlNode node in XmlDoc.SelectNodes("//Configuration/Scripts/Path"))
                {
                    scriptsNode.RemoveChild(node);
                }
            }
            else
            {
                scriptsNode = XmlDoc.CreateElement("Scripts");
                XmlDoc.SelectSingleNode("//Configuration").AppendChild(scriptsNode);
            }

            foreach (var script in Scripts)
            {
                File.WriteAllText(PreScriptDirPath + script.FileName + ".py", script.PreScript);

                XmlNode node = XmlDoc.CreateElement("Path");
                node.AppendChild(XmlDoc.CreateTextNode(script.Path));
                scriptsNode.AppendChild(node);
            }

            XmlDoc.Save(ConfigFilePath);

            File.WriteAllText(RootScriptFilePath, RootScript);
        }
    }

    public class ScriptFile
    {
        public string Path { get; }
        public string PreScript { get; set; }

        public string FileName => System.IO.Path.GetFileNameWithoutExtension(Path);

        public ScriptFile(string path)
        {
            Path = path;
        }
    }
}
