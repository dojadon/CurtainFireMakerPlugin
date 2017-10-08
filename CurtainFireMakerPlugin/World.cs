using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Entities;
using CurtainFireMakerPlugin.Entities.Models;
using CurtainFireMakerPlugin.Tasks;
using CurtainFireMakerPlugin.IO;
using IronPython.Runtime;
using IronPython.Runtime.Operations;
using CsMmdDataIO.Vmd.Data;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        public int MaxFrame { get; set; } = 1000;
        public Scene Scene => Plugin.Instance.Scene;
        public Configuration Config => Plugin.Instance.Config;
        internal PythonExecutor Executor => Plugin.Instance.PythonExecutor;

        private List<Entity> addEntityList = new List<Entity>();
        private List<Entity> removeEntityList = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        internal ShotManager ShotManager { get; }
        internal CurtainFireModel PmxModel { get; }
        internal CurtainFireMotion KeyFrames { get; }

        private TaskManager TaskManager { get; } = new TaskManager();

        public String ExportFileName { get; set; }

        public event EventHandler ExportEvent;

        public string PmxExportPath => Config.ExportDirPath + "\\" + ExportFileName + ".pmx";
        public string VmdExportPath => Config.ExportDirPath + "\\" + ExportFileName + ".vmd";

        internal World(string fileName)
        {
            ShotManager = new ShotManager(this);
            PmxModel = new CurtainFireModel(this);
            KeyFrames = new CurtainFireMotion(this);

            ExportFileName = fileName;
        }

        protected virtual void OnExport(EventArgs e)
        {
            ExportEvent?.Invoke(this, e);
        }

        internal ShotModelData AddShot(EntityShot entity)
        {
            return ShotManager.AddEntity(entity);
        }

        internal int AddEntity(Entity entity)
        {
            addEntityList.Add(entity);

            return FrameCount;
        }

        internal int RemoveEntity(Entity entity)
        {
            removeEntityList.Add(entity);

            return FrameCount;
        }

        internal void Init()
        {
            if (FrameCount > 0)
            {
                KeyFrames.AddPropertyKeyFrame(new VmdPropertyFrameData()
                {
                    FrameTime = 0,
                    IsVisible = false,
                });

                KeyFrames.AddPropertyKeyFrame(new VmdPropertyFrameData()
                {
                    FrameTime = FrameCount,
                    IsVisible = true,
                });
            }
        }

        internal void Frame()
        {
            TaskManager.Frame();

            EntityList.AddRange(addEntityList);
            removeEntityList.ForEach(e => EntityList.Remove(e));

            addEntityList.Clear();
            removeEntityList.Clear();

            EntityList.ForEach(e => e.Frame());

            FrameCount++;
        }

        internal void Export()
        {
            EntityList.ForEach(e => e.OnDeath());
            PmxModel.Finish();
            KeyFrames.Finish();

            KeyFrames.Export();
            PmxModel.Export();
        }

        internal void Finish()
        {
            if (Config.DropPmxFile)
            {
                FileDropUtil.Drop(Plugin.Instance.ApplicationForm.Handle, new StringCollection() { PmxExportPath });
            }

            if (Config.DropVmdFile)
            {
                FileDropUtil.Drop(Plugin.Instance.ApplicationForm.Handle, new StringCollection() { VmdExportPath });
            }
        }

        public void AddTask(Task task)
        {
            TaskManager.AddTask(task);
        }

        public void AddTask(Action<Task> task, int interval, int executeTimes, int waitTime)
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
