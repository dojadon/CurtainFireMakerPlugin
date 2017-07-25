using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;
using CurtainFireMakerPlugin.Tasks;
using CurtainFireMakerPlugin.Entities.Motion;
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

        protected MotionInterpolation motionInterpolation;
        private TaskManager taskManager = new TaskManager();
        public World World { get; }

        public int EntityId { get; }
        private static int nextEntityId;

        public Entity(World world)
        {
            this.World = world;
            this.EntityId = nextEntityId++;
        }

        internal virtual void Frame()
        {
            this.taskManager.Frame();

            this.UpdatePos();
            this.UpdateRot();
            this.UpdateWorldMat();

            this.FrameCount++;
            if (this.DeathCheck())
            {
                this.OnDeath();
            }
        }

        protected virtual void UpdatePos()
        {
            Vector3 interpolatedVelocity = this.Velocity;

            if (this.motionInterpolation != null)
            {
                if (this.motionInterpolation.Within(this.World.FrameCount))
                {
                    //float changeAmount = this.motionInterpolation.GetChangeAmount(this.world.FrameCount);
                    //interpolatedVelocity *= (float)changeAmount;
                    //Console.WriteLine(changeAmount);
                }
                else
                {
                    this.RemoveMotionBezier();
                }
            }
            this.Pos += interpolatedVelocity;
        }

        protected virtual void UpdateRot()
        {
        }

        protected void UpdateWorldMat()
        {
            this.worldMat = this.Rot;
            this.worldMat.TransformVec = this.Pos;

            if (this.ParentEntity != null)
            {
                this.worldMat = this.WorldMat * this.ParentEntity.WorldMat;
            }
        }

        protected virtual bool DeathCheck()
        {
            return this.LivingLimit != 0 && this.FrameCount > this.LivingLimit || this.CheckWorldOut(this);
        }

        public void __call__()
        {
            this.OnSpawn();
        }

        public virtual void OnSpawn()
        {
            this.SpawnFrameNo = this.World.AddEntity(this);
            this.spawnPos = this.Pos;
            this.IsSpawned = true;
        }

        public virtual void OnDeath()
        {
            this.DeathFrameNo = this.World.RemoveEntity(this);
            this.IsDeath = true;
        }

        public virtual void SetMotionBezier(Vector2 pos1, Vector2 pos2, int length)
        {
            Vector3 endPos = this.Velocity * length + this.Pos;
            int frame = this.World.FrameCount;
            this.motionInterpolation = new MotionInterpolation(frame, frame + length, pos1, pos2, this.Pos, endPos);
        }

        internal virtual void RemoveMotionBezier()
        {
            this.motionInterpolation = null;
        }

        public void AddTask(Task task)
        {
            this.taskManager.AddTask(task);
        }

        public void AddTask(Action<Task> task, int interval, int executeTimes, int waitTime)
        {
            this.AddTask(new Task(task, interval, executeTimes, waitTime));
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

        public override bool Equals(object obj) => obj is Entity e && EntityId == e.EntityId;

        public override int GetHashCode() => EntityId;
    }
}
