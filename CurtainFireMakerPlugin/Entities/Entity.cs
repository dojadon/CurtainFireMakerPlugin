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
        private Matrix worldMat = Matrix.Identity;
        public Matrix WorldMat => worldMat;
        public Vector3 WorldPos => WorldMat.TransformVec;
        public Quaternion WorldRot => WorldMat;

        private Vector3 spawnPos = new Vector3();
        public Vector3 SpawnPos => this.spawnPos;

        public virtual Vector3 Pos { get; set; }
        public virtual Quaternion Rot { get; set; } = new Quaternion(0, 0, 0, 1);

        public virtual Vector3 Velocity { get; set; }
        public virtual Vector3 Upward { get; set; } = new Vector3(0, 1, 0);

        public virtual Entity ParentEntity { get; set; }

        public int FrameCount { get; set; }
        public int LivingLimit { get; set; }
        public int SpawnFrameNo { get; set; }
        public int DeathFrameNo { get; set; }

        public Func<Entity, bool> CheckWorldOut { get; set; } = entity => (entity.Pos - entity.SpawnPos).Length > 400.0;

        public bool IsDeath { get; set; }
        public bool IsSpawned { get; set; }

        protected MotionInterpolation MotionInterpolation { get; set; }
        private TaskManager taskManager = new TaskManager();

        public World World { get; }

        public int EntityId { get; }
        private static int nextEntityId;

        public delegate void EntityEventHandler<T, R>(T sender, R e) where T : Entity where R : EventArgs;

        public Entity(World world)
        {
            World = world;
            EntityId = nextEntityId++;
        }

        internal virtual void Frame()
        {
            taskManager.Frame();

            UpdatePos();
            UpdateRot();
            UpdateWorldMat();

            FrameCount++;
            if (DeathCheck())
            {
                OnDeath();
            }
        }

        protected virtual void UpdatePos()
        {
            Vector3 interpolatedVelocity = Velocity;

            if (MotionInterpolation != null)
            {
                if (MotionInterpolation.Within(World.FrameCount))
                {
                    float changeAmount = MotionInterpolation.GetChangeAmount(World.FrameCount);
                    interpolatedVelocity *= changeAmount;
                }
                else
                {
                    RemoveMotionBezier();
                }
            }
            Pos += interpolatedVelocity;
        }

        protected virtual void UpdateRot()
        {
        }

        protected void UpdateWorldMat()
        {
            worldMat = Rot;
            worldMat.TransformVec = Pos;

            if (ParentEntity != null)
            {
                worldMat = WorldMat * ParentEntity.WorldMat;
            }
        }

        protected virtual bool DeathCheck()
        {
            return LivingLimit != 0 && FrameCount > LivingLimit || CheckWorldOut(this);
        }

        public void __call__()
        {
            OnSpawn();
        }

        public virtual void OnSpawn()
        {
            SpawnFrameNo = World.AddEntity(this);
            spawnPos = Pos;
            IsSpawned = true;
        }

        public virtual void OnDeath()
        {
            DeathFrameNo = World.RemoveEntity(this);
            IsDeath = true;
        }

        public virtual void SetMotionBezier(Vector2 pos1, Vector2 pos2, int length)
        {
            MotionInterpolation = new MotionInterpolation(World.FrameCount, length, pos1, pos2);
        }

        internal virtual void RemoveMotionBezier()
        {
            MotionInterpolation = null;
        }

        public void AddTask(Task task)
        {
            taskManager.AddTask(task);
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
