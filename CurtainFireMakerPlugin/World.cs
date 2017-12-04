using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Entities;
using CurtainFireMakerPlugin.Entities.Models;
using CurtainFireMakerPlugin.Tasks;
using CurtainFireMakerPlugin.ShotTypes;
using CurtainFireMakerPlugin.IO;
using IronPython.Runtime;
using IronPython.Runtime.Operations;
using CsMmdDataIO.Vmd.Data;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        public int MaxFrame { get; set; } = 1000;

        public Plugin Plugin { get; }
        public Scene Scene { get; }
        public Configuration Config { get; }
        internal PythonExecutor Executor { get; }

        private List<Entity> AddEntityList { get; } = new List<Entity>();
        private List<Entity> RemoveEntityList { get; } = new List<Entity>();
        private List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        internal ShotGroupManager ShotManager { get; }
        internal CurtainFireModel PmxModel { get; }
        internal CurtainFireMotion KeyFrames { get; }

        private TaskManager TaskManager { get; } = new TaskManager();

        internal String ExportFileName { get; set; }

        public event EventHandler ExportEvent;

        internal string PmxExportPath => Config.PmxExportDirPath + "\\" + ExportFileName + ".pmx";
        internal string VmdExportPath => Config.VmdExportDirPath + "\\" + ExportFileName + ".vmd";

        public string ModelName { get; set; }
        public string ModelDescription { get; set; } = "This model is created by Curtain Fire Maker Plugin";

        internal World(Plugin plugin, string fileName)
        {
            Plugin = plugin;

            Scene = Plugin.Scene;
            Config = Plugin.Config;
            Executor = Plugin.PythonExecutor;

            ShotManager = new ShotGroupManager(this);
            PmxModel = new CurtainFireModel(this);
            KeyFrames = new CurtainFireMotion(this);

            ExportFileName = fileName;
            ModelName = ExportFileName;
        }

        private void OnExport(EventArgs e)
        {
            ExportEvent?.Invoke(this, e);
        }

        internal ShotModelData AddShot(EntityShot entity)
        {
            return ShotManager.AddEntity(entity);
        }

        internal int AddEntity(Entity entity)
        {
            AddEntityList.Add(entity);

            return FrameCount;
        }

        internal int RemoveEntity(Entity entity)
        {
            RemoveEntityList.Add(entity);

            return FrameCount;
        }

        internal void InitPre()
        {
            foreach (var type in ShotType.ShotTypeList)
            {
                type.InitWorld(this);
            }
        }

        internal void InitPost()
        {
            if (FrameCount > 0)
            {
                KeyFrames.AddPropertyKeyFrame(new VmdPropertyFrameData(0, false));
                KeyFrames.AddPropertyKeyFrame(new VmdPropertyFrameData(FrameCount, true));
            }
        }

        internal void Frame()
        {
            TaskManager.Frame();

            EntityList.AddRange(AddEntityList);
            RemoveEntityList.ForEach(e => EntityList.Remove(e));

            AddEntityList.Clear();
            RemoveEntityList.Clear();

            EntityList.ForEach(e => e.Frame());

            FrameCount++;
        }

        internal void Export()
        {
            EntityList.ForEach(e => e.OnDeath());

            PmxModel.FinalizeModel();
            KeyFrames.Finish();

            KeyFrames.Export();
            PmxModel.Export();

            OnExport(EventArgs.Empty);
        }

        internal void DropFileToMMM()
        {
            if (Config.DropPmxFile)
            {
                FileDropUtil.Drop(Plugin.Instance.ApplicationForm.Handle, new StringCollection() { PmxExportPath });
            }

            if (Config.DropVmdFile && (Config.DropPmxFile || Scene.Models.Count > 0))
            {
                FileDropUtil.Drop(Plugin.Instance.ApplicationForm.Handle, new StringCollection() { VmdExportPath });
            }
        }

        internal void AddTask(Task task)
        {
            TaskManager.AddTask(task);
        }

        internal void AddTask(Action<Task> task, int interval, int executeTimes, int waitTime)
        {
            AddTask(new Task(task, interval, executeTimes, waitTime));
        }

        public void AddTask(PythonFunction func, int interval, int executeTimes, int waitTime, bool withArg = false)
        {
            if (withArg)
            {
                AddTask(task => PythonCalls.Call(func, task), interval, executeTimes, waitTime);
            }
            else
            {
                AddTask(task => PythonCalls.Call(func), interval, executeTimes, waitTime);
            }
        }
    }
}
