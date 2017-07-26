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
        /// <summary>  
        ///  ワールド座標系の座標変換行列
        /// </summary>  
        public Matrix WorldMat => worldMat;
        /// <summary>  
        ///  ワールド座標系の座標
        /// </summary>  
        public Vector3 WorldPos => WorldMat.TransformVec;
        /// <summary>  
        ///  ワールド座標系の姿勢
        /// </summary>  
        public Quaternion WorldRot => WorldMat;

        private Vector3 spawnPos = new Vector3();
        /// <summary>  
        ///  OnSpawnが呼ばれた時の座標
        /// </summary>  
        public Vector3 SpawnPos => this.spawnPos;

        /// <summary>  
        ///  ローカル座標系の座標
        /// </summary>  
        public virtual Vector3 Pos { get; set; }
        /// <summary>  
        ///  ローカル座標系の姿勢
        /// </summary>  
        public virtual Quaternion Rot { get; set; } = new Quaternion(0, 0, 0, 1);

        /// <summary>  
        ///  1フレーム単位の移動量
        /// </summary>  
        public virtual Vector3 Velocity { get; set; }
        /// <summary>  
        ///  上方向を表すベクトル
        /// </summary>  
        public virtual Vector3 Upward { get; set; } = new Vector3(0, 1, 0);

        /// <summary>  
        ///  親エンティティ
        /// </summary>  
        public virtual Entity ParentEntity { get; set; }

        /// <summary>  
        ///  OnSpawnが呼ばれてから経過したフレーム数
        /// </summary>  
        public int FrameCount { get; set; }
        /// <summary>  
        ///  寿命
        /// </summary>  
        public int LivingLimit { get; set; }
        /// <summary>  
        ///  OnSpawnが呼ばれた時のフレーム番号
        /// </summary>  
        public int SpawnFrameNo { get; set; }
        /// <summary>  
        ///  OnDeathが呼ばれた時のフレーム番号
        /// </summary>  
        public int DeathFrameNo { get; set; }

        /// <summary>  
        ///  死亡判定
        /// </summary>  
        public Func<Entity, bool> CheckWorldOut { get; set; } = entity => (entity.Pos - entity.SpawnPos).Length > 400.0;

        /// <summary>  
        ///  OnDeathが呼ばれたか
        /// </summary>  
        public bool IsDeath { get; set; }
        /// <summary>  
        ///  OnSpawnが呼ばれたか
        /// </summary>  
        public bool IsSpawned { get; set; }

        protected MotionInterpolation motionInterpolation;
        private TaskManager taskManager = new TaskManager();

        /// <summary>  
        ///  所属しているワールド
        /// </summary>  
        public World World { get; }

        /// <summary>  
        ///  単一のID
        /// </summary>  
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

        /// <summary>  
        ///  Pythonの特殊メソッド。OnSpawnと同義
        /// </summary>  
        public void __call__()
        {
            this.OnSpawn();
        }

        /// <summary>  
        ///  EntityをWorldに追加する
        /// </summary>  
        public virtual void OnSpawn()
        {
            this.SpawnFrameNo = this.World.AddEntity(this);
            this.spawnPos = this.Pos;
            this.IsSpawned = true;
        }

        /// <summary>  
        ///  EntityをWorldから削除する
        /// </summary>  
        public virtual void OnDeath()
        {
            this.DeathFrameNo = this.World.RemoveEntity(this);
            this.IsDeath = true;
        }

        /// <summary>  
        ///  <paramref name="pos1"/> 制御点1
        ///  <paramref name="pos2"/> 制御点2
        ///  <paramref name="length"/> モーション補間曲線を適用するフレーム数
        /// </summary>  
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

        /// <summary>  
        /// <paramref name="func"/>実行する関数
        /// <paramref name="func"/>実行する間隔
        /// <paramref name="executeTimes"/>実行する回数
        /// <paramref name="waitTime"/>実行するまでの待機フレーム数
        /// <paramref name="withArg"/>Taskを引数に与えて実行するか
        /// </summary>  
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
