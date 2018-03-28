using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class ProjectEditorControl : UserControl
    {
        public List<Project> Projects { get; } = new List<Project>();

        public int SelectedProjectIndex => ComboBoxProjects.SelectedIndex;
        public bool IsProjectSelected => 0 <= SelectedProjectIndex && SelectedProjectIndex < Projects.Count;
        public Project SelectedProject => Projects[SelectedProjectIndex];

        public List<ScriptFile> PreScripts => SelectedProject.Scripts;

        public string RootScript => TextBoxRootScript.Text;

        private Plugin Plugin { get; }

        public ProjectEditorControl(Plugin plugin)
        {
            Plugin = plugin;

            InitializeComponent();

            PreScriptEditor.ProjectEditor = this;

            foreach (var path in Directory.GetDirectories(Configuration.ProjectsDirPath))
            {
                if (Guid.TryParse(Path.GetFileName(path), out Guid guid))
                {
                    Projects.Add(new Project(guid));
                }
            }

            UpdateDataSource();
            PreScriptEditor.UpdateDataSource();
            UpdateEnable();

            if (Projects.Count > 0)
            {
                ComboBoxProjects.SelectedIndex = 0;
            }
        }

        public void UpdatePreScript(string path, string script)
        {
            PreScriptEditor.UpdatePreScript(path, script);
        }

        public string GetPreScript(string path)
        {
            return PreScriptEditor.GetPreScript(path);
        }

        public void GenerateCurtainFire()
        {
            if (PreScriptEditor.IsScriptSelected)
            {
                Plugin.Config.ScriptPath = PreScriptEditor.SelectedPreScript.Path;
            }

       //     if (IsProjectSelected)
            {
                Plugin.Config.PmxExportDirPath = SelectedProject.ExportDirPmx;
                Plugin.Config.VmdExportDirPath = SelectedProject.ExportDirVmd;
            }

            Plugin.Run(null);
        }

        private bool IsUpdating { get; set; }

        private void UpdateDataSource()
        {
            IsUpdating = true;

            Projects.Sort((p1, p2) => string.Compare(p1.ProjectName, p2.ProjectName));

            ComboBoxProjects.DataSource = null;
            ComboBoxProjects.DataSource = Projects;
            ComboBoxProjects.DisplayMember = "ProjectName";

            IsUpdating = false;
        }

        private void UpdateEnable()
        {
            TextBoxProjectName.Enabled = IsProjectSelected;
            TextBoxExportDirPmx.Enabled = IsProjectSelected;
            TextBoxExportDirVmd.Enabled = IsProjectSelected;
            ButtonReferencePmx.Enabled = IsProjectSelected;
            ButtonReferenceVmd.Enabled = IsProjectSelected;
        }

        private void SelectedProjectIndexChanged(object sender, EventArgs e)
        {
            if (IsUpdating) return;

            if (IsProjectSelected)
            {
                TextBoxProjectName.Text = SelectedProject.ProjectName;
                TextBoxRootScript.Text = SelectedProject.RootScript.Replace("\r\n", "\n").Replace("\n", "\r").Replace("\r", "\r\n");
                TextBoxExportDirPmx.Text = SelectedProject.ExportDirPmx;
                TextBoxExportDirVmd.Text = SelectedProject.ExportDirVmd;
            }
            else
            {
                TextBoxProjectName.Text = TextBoxExportDirPmx.Text = TextBoxExportDirVmd.Text = TextBoxRootScript.Text = "";
            }
            UpdateEnable();
            PreScriptEditor.UpdateDataSource();
        }

        private void ReferenceExportDirPmx(object sender, EventArgs e)
        {
            FolderBrowserDialog.SelectedPath = TextBoxExportDirPmx.Text;

            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TextBoxExportDirPmx.Text = FolderBrowserDialog.SelectedPath;
            }
        }

        private void ReferenceExportDirVmd(object sender, EventArgs e)
        {
            FolderBrowserDialog.SelectedPath = TextBoxExportDirVmd.Text;

            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TextBoxExportDirVmd.Text = FolderBrowserDialog.SelectedPath;
            }
        }

        private void TextChangedRootScript(object sender, EventArgs e)
        {
            if (IsProjectSelected)
            {
                SelectedProject.RootScript = TextBoxRootScript.Text;
            }
        }

        private void TextChangedProjectName(object sender, EventArgs e)
        {
            if (IsProjectSelected)
            {
                SelectedProject.ProjectName = TextBoxProjectName.Text;
            }
        }

        private void TextChangedExportDirPmx(object sender, EventArgs e)
        {
            if (IsProjectSelected)
            {
                SelectedProject.ExportDirPmx = TextBoxExportDirPmx.Text;
            }
        }

        private void TextChangedExportDirVmd(object sender, EventArgs e)
        {
            if (IsProjectSelected)
            {
                SelectedProject.ExportDirVmd = TextBoxExportDirVmd.Text;
            }
        }

        private void AddProject(object sender, EventArgs e)
        {
            Projects.Add(new Project(Guid.NewGuid()) { ProjectName = "NewProject" + Enumerable.Range(0, Int32.MaxValue).First(i => Projects.All(p => p.ProjectName != ("NewProject" + i))) });

            UpdateDataSource();

            ComboBoxProjects.SelectedIndex = Projects.Count - 1;
        }

        private void LeaveTextBoxProjectName(object sender, EventArgs e)
        {
            UpdateDataSource();
        }

        public void Save() => Projects.ForEach(p => p.Save());
    }
}