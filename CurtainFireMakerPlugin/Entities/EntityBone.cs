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

        public EntityBone(World world, string modelName, string boneName) : base(world)
        {
            Modelname = modelName;
            BoneName = boneName;

            try
            {
                Bone bone = GetBone();

                BoneCollection bones = GetBones();

                if (bone.ParentBoneID != -1 && bones[bone.ParentBoneID] != null)
                {
                    var parentBone = new EntityBone(world, Modelname, bones[bone.ParentBoneID].Name);
                    ParentEntity = parentBone;
                }

                OnSpawn();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override void UpdatePos()
        {
            Bone bone = GetBone();
            var pos = new Vector3();
            var rot = new Quaternion();

            foreach (var layer in bone.Layers)
            {
                MotionFrameData data = layer.Frames.GetFrame(World.FrameCount);
                pos += data.Position;
                rot *= data.Quaternion;
            }

            Pos = pos;
            Rot = rot;
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
