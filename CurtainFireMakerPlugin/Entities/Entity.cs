using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Mathematics;
using CurtainFireMakerPlugin.Tasks;
using CurtainFireMakerPlugin.Entities.Motion;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin.Entities
{
    public class Entity
    {
        private Matrix worldMat = new Matrix();
        public Matrix WorldMat => worldMat;
        public Vector3 WorldPos { get { return (Vector3)WorldMat; } }
        public Quaternion WorldRot { get { return WorldMat; } }

        private Vector3 spawnPos = new Vector3();
        public Vector3 SpawnPos => this.spawnPos;

        public Vector3 Pos { get; set; } = new Vector3();
        public Vector3 PrevPos { get; set; } = new Vector3();
        public Quaternion Rot { get; set; } = new Quaternion(0, 0, 0, 1);
        public Quaternion PrevRot { get; set; } = new Quaternion(0, 0, 0, 1);

        public Vector3 Velocity { get; set; } = new Vector3();
        public Vector3 PrevVelocity { get; set; } = new Vector3();

        public Vector3 Upward { get; set; } = new Vector3(0, 1, 0);
        public Vector3 PrevUpward { get; set; } = new Vector3(0, 1, 0);

        protected Entity parentEntity;

        public int FrameCount { get; set; }
        public int LivingLimit { get; set; }
        public int SpawnFrameNo { get; set; }
        public int DeathFrameNo { get; set; }

        public Func<Entity, bool> CheckWorldOut { get; set; } = entity => (entity.Pos - entity.SpawnPos).Length() > 400.0;

        public bool IsDeath { get; set; }

        protected MotionInterpolation motionInterpolation;
        private TaskManager taskManager = new TaskManager();
        public World world;

        internal virtual void Frame()
        {
            this.PrevPos = this.Pos;
            this.PrevRot = this.PrevRot;
            this.PrevUpward = this.Upward;
            this.PrevVelocity = this.Velocity;

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
                if (this.motionInterpolation.Within(this.world.FrameCount))
                {
                    //double changeAmount = this.motionInterpolation.GetChangeAmount(this.world.FrameCount);
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
            this.worldMat = (Matrix)this.Rot + this.Pos;

            if (this.parentEntity != null)
            {
                this.worldMat = this.parentEntity.WorldMat * this.worldMat;
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
            this.SpawnFrameNo = this.world.AddEntity(this);
            this.spawnPos = this.Pos;
        }

        public virtual void OnDeath()
        {
            this.DeathFrameNo = this.world.RemoveEntity(this);
            this.IsDeath = true;
        }

        public virtual void SetMotionBezier(Vector2 pos1, Vector2 pos2, int length)
        {
            Vector3 endPos = this.Velocity * length + this.Pos;
            int frame = this.world.FrameCount;
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
    }
}
