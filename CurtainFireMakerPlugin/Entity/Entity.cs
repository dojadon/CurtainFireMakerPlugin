using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;
using CurtainFireMakerPlugin.Task;

namespace CurtainFireMakerPlugin.Entity
{
    class Entity
    {
        public Matrix WorldMat { get; set; } = new Matrix();
        public Vector3 WorldPos { get { return new Vector3(WorldMat.M14, WorldMat.M24, WorldMat.M34); } }
        public Quaternion WorldRot { get { return Quaternion.RotationMatrix(WorldMat); } }

        public Vector3 Pos { get; set; } = new Vector3();
        public Vector3 PrevPos { get; set; } = new Vector3();
        public Quaternion Rot { get; } = new Quaternion();
        public Quaternion PrevRot { get; set; } = new Quaternion();

        public Vector3 Veclocity { get; set; } = new Vector3();
        public Vector3 PrevVeclocity { get; set; } = new Vector3();

        public Quaternion RotFirst { get; set; } = new Quaternion();
        public Quaternion RotSecond { get; set; } = new Quaternion();

        public Vector3 Upward { get; set; } = new Vector3();
        public Vector3 PrevUpward { get; set; } = new Vector3();

        protected Entity parentEntity;

        public int FrameCount { get; set; }
        public int LivingLimit { get; set; }
        public int SpawnFrameNo { get; }
        public int DeathFrameNo { get; }

        private TaskManager taskManager = new TaskManager();
    }
}
