using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using MMDataIO.Pmx;
using MMTransform;
using VecMath;
using MikuMikuPlugin;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityMMModel : Entity
    {
        public Model Model { get; }
        public PmxModelData Data { get; } = new PmxModelData();

        private List<PmxBone> PmxBones { get; } = new List<PmxBone>();
        private List<EntityBone> EntityBones { get; } = new List<EntityBone>();

        public EntityBone this[string boneName]
        {
            get
            {
                try
                {
                    return EntityBones.First(b => b.PmxBone.BoneName == boneName);
                }
                catch
                {
                    throw new ArgumentException($"Not found bone : {boneName}");
                }
            }
        }

        public EntityMMModel(World world, Scene scene, string filePath) : base(world)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                Data.Read(new BinaryReader(stream));
            }

            try
            {
                Model = scene.Models.First(m => m.Name == Data.Header.ModelName);
            }
            catch
            {
                throw new ArgumentException($"Not found model : {Data.Header.ModelName}");
            }

            for (int i = 0; i < Data.BoneArray.Length; i++)
            {
                PmxBones.Add(new PmxBone(Data.BoneArray[i]) { BoneIndex = i });
                EntityBones.Add(new EntityBone(World, PmxBones[i], Model.Bones[Data.BoneArray[i].BoneName]));
            }

            for (int i = 0; i < Data.BoneArray.Length; i++)
            {
                PmxBones[i].Init(Data.BoneArray[i], PmxBones);
                EntityBones[i].Init(EntityBones);
            }
        }

        public override void Frame()
        {
        }
    }

    public class EntityBone : Entity
    {
        public PmxBone PmxBone { get; }
        public Bone MMMBone { get; }

        public List<EntityBone> ParentBones { get; } = new List<EntityBone>();

        public EntityBone(World world, PmxBone bone, Bone mmmBone) : base(world)
        {
            PmxBone = bone;
            MMMBone = mmmBone;
        }

        public void Init(List<EntityBone> bones)
        {
            if (PmxBone.ParentBone != null)
            {
                ParentBones.Add(bones[PmxBone.ParentBone.BoneIndex]);
            }

            if (PmxBone.LinkParent != null)
            {
                ParentBones.Add(bones[PmxBone.LinkParent.BoneIndex]);
            }
        }

        public override void Frame()
        {
            var pos = DxMath.Vector3.Zero;
            var rot = DxMath.Quaternion.Identity;

            foreach (var layer in MMMBone.Layers)
            {
                var data = layer.Frames.GetFrame(World.FrameCount);
                pos += data.Position;
                rot *= data.Quaternion;
            }

            PmxBone.Pos = Pos = pos;
            PmxBone.Rot = Rot = rot;

            foreach (var bone in ParentBones.Where(b => b.FrameCount != World.FrameCount))
            {
                bone.Frame();
            }

            PmxBone.UpdateLocalMatrix();
            PmxBone.UpdateWorldMatrix();

            LocalMat = PmxBone.LocalMat;
            WorldMat = PmxBone.WorldMat;

            FrameCount = World.FrameCount;
        }

        public void SetExtraParent(EntityBone bone)
        {
            PmxBone.Flag |= BoneFlags.EXTRA;
            PmxBone.ParentBone = bone.PmxBone;
        }
    }
}
