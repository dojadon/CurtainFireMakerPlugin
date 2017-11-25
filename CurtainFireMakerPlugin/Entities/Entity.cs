using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;
using CurtainFireMakerPlugin.Tasks;
using CurtainFireMakerPlugin.Mathematics;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin.Entities
{
    public class Entity
    {
        public virtual Matrix4 WorldMat => ParentEntity != null ? LocalMat * ParentEntity.WorldMat : LocalMat;
        public Vector3 WorldPos => WorldMat.Translation;
        public Matrix3 WorldRot => WorldMat;

        public virtual Matrix4 LocalMat
        {
            get => new Matrix4(Rot, Pos);
            set
            {
                Pos = value.Translation;
                Rot = value;
            }
        }
        public virtual Vector3 Pos { get; set; }
        public virtual Quaternion Rot { get; set; } = Quaternion.Identity;

        public virtual Vector3 Velocity { get; set; }
        public virtual Vector3 Upward { get; set; } = Vector3.UnitY;

        public virtual Entity ParentEntity { get; protected set; }

        public int FrameCount { get; private set; }
        public int LivingLimit { get; set; }
        public int SpawnFrameNo { get; private set; }
        public int DeathFrameNo { get; private set; }

        public virtual Func<Entity, bool> DiedDecision { get; set; } = e => e.LivingLimit != 0 && e.FrameCount > e.LivingLimit;

        public bool IsDeath { get; private set; }
        public bool IsSpawned { get; private set; }

        private TaskManager TaskManager { get; } = new TaskManager();

        public World World { get; }

        public int EntityId { get; }
        private static int nextEntityId;

        public Entity(World world, Entity parentEntity = null)
        {
            World = world;
            EntityId = nextEntityId++;

            ParentEntity = parentEntity;
        }

        internal virtual void Frame()
        {
            UpdateLocalMat();

            TaskManager.Frame();

            FrameCount++;
            if (DiedDecision(this))
            {
                OnDeath();
            }
        }

        protected virtual void UpdateLocalMat()
        {
            Pos += Velocity;
        }

        public void __call__()
        {
            OnSpawn();
        }

        public virtual void OnSpawn()
        {
            SpawnFrameNo = World.AddEntity(this);
            IsSpawned = true;
        }

        public virtual void OnDeath()
        {
            DeathFrameNo = World.RemoveEntity(this);
            IsDeath = true;
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

        public override bool Equals(object obj) => obj is Entity e && EntityId == e.EntityId;

        public override int GetHashCode() => EntityId;
    }
}
