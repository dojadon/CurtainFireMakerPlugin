using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;
using CurtainFireMakerPlugin.Tasks;
using CurtainFireMakerPlugin.Entities.Motion;

namespace CurtainFireMakerPlugin.Entities
{
    public class Entity
    {
        private Matrix worldMat = new Matrix();
        public Matrix WorldMat => worldMat;
        public Vector3 WorldPos { get { return new Vector3(WorldMat.M14, WorldMat.M24, WorldMat.M34); } }
        public Quaternion WorldRot { get { return Quaternion.RotationMatrix(WorldMat); } }

        private Vector3 spawnPos = new Vector3();
        public Vector3 SpawnPos => this.spawnPos;

        public Vector3 Pos { get; set; } = new Vector3();
        public Vector3 PrevPos { get; set; } = new Vector3();
        public Quaternion Rot { get; set; } = new Quaternion();
        public Quaternion PrevRot { get; set; } = new Quaternion();

        public Vector3 Velocity { get; set; } = new Vector3();
        public Vector3 PrevVelocity { get; set; } = new Vector3();

        public Quaternion RotFirst { get; set; } = new Quaternion();
        public Quaternion RotSecond { get; set; } = new Quaternion();

        public Vector3 Upward { get; set; } = new Vector3();
        public Vector3 PrevUpward { get; set; } = new Vector3();

        protected Entity parentEntity;

        public int FrameCount { get; set; }
        public int LivingLimit { get; set; }
        public int SpawnFrameNo { get; set; }
        public int DeathFrameNo { get; set; }

        public Func<Entity, bool> CheckWorldOut { get; set; } = entity => (entity.Pos - entity.SpawnPos).Length() > 400.0;

        public bool IsDeath { get; set; }

        protected MotionInterpolation motionInterpolation;
        private TaskManager taskManager = new TaskManager();
        private World world;

        public virtual void Frame()
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

        protected void UpdatePos()
        {
            Vector3 interpolatedVelocity = this.Velocity;

            if (this.motionInterpolation != null)
            {
                if (this.motionInterpolation.Within(this.world.FrameCount))
                {
                    double changeAmount = this.motionInterpolation.GetChangeAmount(this.world.FrameCount);
                    interpolatedVelocity *= (float)changeAmount;
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
            this.Rot = this.RotFirst * this.RotSecond;
        }

        protected void UpdateWorldMat()
        {
            this.worldMat = Matrix.RotationQuaternion(this.Rot);
            Matrix.Translation(this.Pos.X, this.Pos.Y, this.Pos.Z, out this.worldMat);

            if (this.parentEntity != null)
            {
                this.worldMat = this.parentEntity.WorldMat * this.worldMat;
            }
        }

        protected virtual bool DeathCheck()
        {
            return this.LivingLimit != 0 && this.FrameCount > this.LivingLimit || this.CheckWorldOut.Invoke(this);
        }

        public virtual void OnSpawn()
        {
            this.SpawnFrameNo = this.world.AddEntity(this);
        }

        public virtual void OnDeath()
        {
            this.DeathFrameNo = this.world.RemoveEntity(this);
            this.IsDeath = true;
        }

        public void SetMotionBezier(Vector2 pos1, Vector2 pos2, int length)
        {
            Vector3 endPos = this.Velocity * length + this.Pos;
            int frame = this.world.FrameCount;
            this.motionInterpolation = new MotionInterpolation(frame, frame + length, pos1, pos2, this.Pos, endPos);
        }

        protected void RemoveMotionBezier()
        {
            this.motionInterpolation = null;
        }

        public void AddTask(Task task)
        {
            this.taskManager.AddTask(task);
        }

        public void AddTask(Action task, int interval, int executeTimes, int waitTime)
        {
            this.AddTask(new Task(task, interval, executeTimes, waitTime));
        }
    }
}
