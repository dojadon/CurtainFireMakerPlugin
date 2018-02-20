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

        private PmxBones PmxBones { get; }
        private List<EntityBone> EntityBones { get; }

        public EntityBone this[string boneName] => EntityBones.First(b=>b.PmxBone.BoneName == boneName);

        public EntityMMModel(World world, Scene scene, string filePath) : base(world)
        {
            PmxBones = new PmxBones();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                Data.Read(new BinaryReader(stream));
            }
            PmxBones.SetBoneData(Data.BoneArray);

            Model = scene.Models.FirstOrDefault(m => m.Name == Data.Header.ModelName && m.EnglishName == Data.Header.ModelNameE);

            if (Model == null)
            {
                Console.WriteLine($"Not found model : {Data.Header.ModelName}");
                return;
            }

            foreach(var pmxBone in PmxBones)
            {
                var bone = new EntityBone(World, pmxBone, Model.Bones[pmxBone.BoneName]);
                bone.OnSpawn();
                EntityBones.Add(bone);
            }
            OnSpawn();
        }

        public override void Frame()
        {
            PmxBones.UpdateMatries();

            EntityBones.ForEach(b => b.ApplyMatrix());
        }
    }

    public class EntityBone : Entity
    {
        public PmxBone PmxBone { get; }
        public Bone MMMBone { get; }

        public EntityBone(World world, PmxBone bone, Bone mmmBone) : base(world)
        {
            PmxBone = bone;
            MMMBone = mmmBone;
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
        }

        public void ApplyMatrix()
        {
            LocalMat = PmxBone.LocalMat;
            WorldMat = PmxBone.WorldMat;
        }
    }
}
