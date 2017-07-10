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

        public EntityBone(string modelName, string boneName)
        {
            this.world = World.Instance;

            Modelname = modelName;
            BoneName = boneName;

            try
            {
                Bone bone = this.GetBone();

                BoneCollection bones = this.GetBones();

                if (bone.ParentBoneID != -1 && bones[bone.ParentBoneID] != null)
                {
                    var parentBone = new EntityBone(Modelname, bones[bone.ParentBoneID].Name);
                    this.parentEntity = parentBone;
                }

                this.OnSpawn();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override void UpdatePos()
        {
            Bone bone = this.GetBone();
            var pos = new Vector3();
            var rot = new Quaternion();

            foreach (var layer in bone.Layers)
            {
                MotionFrameData data = layer.Frames.GetFrame(this.world.FrameCount);
                pos += data.Position;
                rot *= data.Quaternion;
            }

            this.Pos = pos;
            this.Rot = rot;
        }

        private BoneCollection GetBones()
        {
            Model model = Plugin.Instance.Scene.Models.ToList().Find(m => m.Name.Equals(this.Modelname));

            if (model != null)
            {
                return model.Bones;
            }
            else
            {
                throw new ApplicationException("Not found model name : " + this.Modelname);
            }
        }

        private Bone GetBone()
        {
            Bone bone = this.GetBones()[this.BoneName];

            if (bone != null)
            {
                return bone;
            }
            else
            {
                throw new ApplicationException("Not found bone name : " + this.Modelname + ":" + this.BoneName);
            }
        }
    }
}
