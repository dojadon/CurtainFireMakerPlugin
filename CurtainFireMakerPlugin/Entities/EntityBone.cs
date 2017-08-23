using System;
using System.Collections;
using System.Linq;
using System.Text;
using MikuMikuPlugin;
using DxMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityBone : Entity
    {
        private string Modelname { get; }
        private string BoneName { get; }
        private Vector3 InitializePos { get; }

        public Model Model { get; }
        public Bone Bone { get; }

        public EntityBone(World world, string modelName, string boneName) : base(world)
        {
            Model = World.Scene.Models.ToList().Find(m => m.DisplayName.Equals(Modelname));
            if (Model == null)
            {
                throw new ArgumentException($"Not found model : ({modelName}, {boneName})");
            }

            Bone = Model.Bones[boneName];
            if (Bone == null)
            {
                throw new ArgumentException($"Not found bone : ({modelName}, {boneName})");
            }

            InitializePos = Bone.InitialPosition;

            var parentBone = Model.Bones[Bone.ParentBoneID];
            if (Bone.ParentBoneID != -1 && parentBone != null)
            {
                ParentEntity = new EntityBone(world, Modelname, parentBone.Name);
                InitializePos -= parentBone.InitialPosition;
            }

            OnSpawn();
            Frame();
        }

        protected override void UpdatePos()
        {
            var pos = InitializePos;
            var rot = Quaternion.Identity;

            foreach (var layer in Bone.Layers)
            {
                MotionFrameData data = layer.Frames.GetFrame(World.FrameCount);
                pos += data.Position;
                rot *= data.Quaternion;
            }

            Pos = pos;
            Rot = rot;
        }

        public override void OnDeath()
        {
        }
    }
}
