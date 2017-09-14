using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuPlugin;
using DxMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityBone : Entity
    {
        private static Dictionary<Tuple<string, string>, EntityBone> BoneDict { get; } = new Dictionary<Tuple<string, string>, EntityBone>();

        private Vector3 InitializePos { get; }

        public Model Model { get; }
        public Bone Bone { get; }

        public override Func<Entity, bool> DiedDecision { get => e => false; set { } }

        public EntityBone(World world, string modelName, string boneName) : base(world)
        {
            Model = World.Scene.Models.ToList().Find(m => m.DisplayName == modelName);
            if (Model == null)
            {
                throw new ArgumentException($"Not found model : {modelName}");
            }

            Bone = Model.Bones[boneName];
            if (Bone == null)
            {
                throw new ArgumentException($"Not found bone : {modelName}, {boneName}");
            }

            InitializePos = Bone.InitialPosition;

            var parentBone = Model.Bones[Bone.ParentBoneID];
            if (Bone.ParentBoneID != -1 && parentBone != null)
            {
                var tuple = new Tuple<string, string>(modelName, parentBone.Name);

                if (BoneDict.ContainsKey(tuple))
                {
                    ParentEntity = BoneDict[tuple];
                }
                else
                {
                    ParentEntity = new EntityBone(world, modelName, parentBone.Name);
                }
                InitializePos -= parentBone.InitialPosition;
            }

            OnSpawn();
            Frame();

            BoneDict.Add(new Tuple<string, string>(modelName, boneName), this);
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
