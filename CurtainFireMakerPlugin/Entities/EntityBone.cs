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

        public EntityBone(World world, string modelName, string boneName) : base(world)
        {
            Modelname = modelName;
            BoneName = boneName;

            Bone bone = GetBone();

            BoneCollection bones = GetBones();

            InitializePos = bone.InitialPosition;

            if (bone.ParentBoneID != -1 && bones[bone.ParentBoneID] != null)
            {
                var parentBone = new EntityBone(world, Modelname, bones[bone.ParentBoneID].Name);
                ParentEntity = parentBone;
                InitializePos -= bones[bone.ParentBoneID].InitialPosition;
            }

            OnSpawn();
            Frame();
        }

        protected override void UpdatePos()
        {
            Bone bone = GetBone();
            var pos = InitializePos;
            var rot = Quaternion.Identity;

            foreach (var layer in bone.Layers)
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

        private BoneCollection GetBones()
        {
            Model model = World.Scene.Models.ToList().Find(m => m.DisplayName.Equals(Modelname));

            if (model != null)
            {
                return model.Bones;
            }
            else
            {
                throw new ApplicationException($"Not found model name : {Modelname}");
            }
        }

        private Bone GetBone()
        {
            Bone bone = GetBones()[BoneName];

            if (bone != null)
            {
                return bone;
            }
            else
            {

                throw new ApplicationException($"Not found bone name : {Modelname} : {BoneName}");
            }
        }
    }
}
