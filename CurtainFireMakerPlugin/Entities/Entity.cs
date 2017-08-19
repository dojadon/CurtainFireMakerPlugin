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
        public virtual Matrix4 WorldMat { get; set; }
        public Vector3 WorldPos => WorldMat.TransformVec;
        public Quaternion WorldRot => WorldMat;

        private Vector3 spawnPos = new Vector3();
        public Vector3 SpawnPos => spawnPos;

        public virtual Vector3 Pos { get; set; }
        public virtual Quaternion Rot { get; set; } = Quaternion.Identity;

        public virtual Vector3 Velocity { get; set; }
        public virtual Vector3 Upward { get; set; } = new Vector3(0, 1, 0);

        private Entity parentEntity;
        public virtual Entity ParentEntity
        {
            get => parentEntity;
            set
            {
                parentEntity = value;
                UpdateWorldMat();
            }
        }

        public int FrameCount { get; set; }
        public int LivingLimit { get; set; }
        public int SpawnFrameNo { get; set; }
        public int DeathFrameNo { get; set; }

        public virtual Func<Entity, bool> DiedDecision { get; set; } = e =>
        {
            return e.LivingLimit != 0 && e.FrameCount > e.LivingLimit || (e.Pos - e.SpawnPos).Length() > 400.0;
        };

        public bool IsDeath { get; set; }
        public bool IsSpawned { get; set; }

        protected MotionInterpolation MotionInterpolation { get; set; }
        private TaskManager TaskManager { get; } = new TaskManager();

        public World World { get; }

        public int EntityId { get; }
        private static int nextEntityId;

        public delegate void EntityEventHandler<T, R>(T sender, R e) where T : Entity where R : EventArgs;

        public event EntityEventHandler<Entity, EventArgs> SpawnEvent;
        public event EntityEventHandler<Entity, EventArgs> DeathEvent;

        public event EntityEventHandler<Entity, EventArgs> SetMotionInterpolationCurveEvent;
        public event EntityEventHandler<Entity, EventArgs> RemoveMotionInterpolationCurveEvent;

        public Entity(World world)
        {
            World = world;
            EntityId = nextEntityId++;
        }

        internal virtual void Frame()
        {
            TaskManager.Frame();

            UpdatePos();
            UpdateRot();
            UpdateWorldMat();

            FrameCount++;
            if (DiedDecision(this))
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
                    RemoveMotionInterpolationCurve();
                }
            }
            Pos += interpolatedVelocity;
        }

        protected virtual void UpdateRot()
        {
        }

        protected virtual void UpdateWorldMat()
        {
            WorldMat = Matrix4.Translation(Rot, Pos);

            if (ParentEntity != null)
            {
                WorldMat = WorldMat * ParentEntity.WorldMat;
            }
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

            SpawnEvent?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnDeath()
        {
            DeathFrameNo = World.RemoveEntity(this);
            IsDeath = true;

            DeathEvent?.Invoke(this, EventArgs.Empty);
        }

        public virtual void SetMotionInterpolationCurve(Vector2 pos1, Vector2 pos2, int length)
        {
            MotionInterpolation = new MotionInterpolation(World.FrameCount, length, pos1, pos2);

            SetMotionInterpolationCurveEvent?.Invoke(this, EventArgs.Empty);
        }

        internal virtual void RemoveMotionInterpolationCurve()
        {
            RemoveMotionInterpolationCurveEvent?.Invoke(this, EventArgs.Empty);

            MotionInterpolation = null;
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
