﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;
using CurtainFireMakerPlugin.Tasks;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        public int MaxFrame { get; set; } = 1000;

        private List<Entity> addEntityList = new List<Entity>();
        private List<Entity> removeEntityList = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        internal ShotManager ShotManager { get; }
        internal CurtainFireModel PmxModel { get; }
        internal CurtainFireMotion VmdMotion { get; }

        private TaskManager TaskManager { get; } = new TaskManager();

        public String ExportFileName { get; set; }

        public event EventHandler Export;

        public World(string fileName)
        {
            ShotManager = new ShotManager(this);
            PmxModel = new CurtainFireModel(this);
            VmdMotion = new CurtainFireMotion(this);

            ExportFileName = fileName;

            Export += (obj, e) =>
            {
                PmxModel.Export(this);
                VmdMotion.Export(this);
            };
        }

        protected virtual void OnExport(EventArgs e)
        {
            Export?.Invoke(this, e);
        }

        internal ShotModelData AddShot(EntityShot entity)
        {
            return this.ShotManager.AddEntity(entity);
        }

        internal int AddEntity(Entity entity)
        {
            this.addEntityList.Add(entity);

            return this.FrameCount;
        }

        internal int RemoveEntity(Entity entity)
        {
            this.removeEntityList.Add(entity);

            return this.FrameCount;
        }

        internal void Frame()
        {
            this.TaskManager.Frame();

            this.EntityList.AddRange(this.addEntityList);
            this.removeEntityList.ForEach(e => this.EntityList.Remove(e));

            this.addEntityList.Clear();
            this.removeEntityList.Clear();

            this.EntityList.ForEach(e => e.Frame());
            this.EntityList.RemoveAll(e => e.IsDeath);

            this.FrameCount++;
        }

        internal void Finish()
        {
            EntityList.ForEach(e => e.OnDeath());
            PmxModel.CompressMorph();

            OnExport(EventArgs.Empty);
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
                this.AddTask(task => PythonCalls.Call(func, task), interval, executeTimes, waitTime);
            }
            else
            {
                this.AddTask(task => PythonCalls.Call(func), interval, executeTimes, waitTime);
            }
        }
    }
}
