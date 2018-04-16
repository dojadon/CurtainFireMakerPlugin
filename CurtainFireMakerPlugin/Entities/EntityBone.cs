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
    public class EntityBone : Entity
    {
        public PmxBone PmxBone { get; }
        public Bone MMMBone { get; }

        public List<EntityBone> ParentBones { get; } = new List<EntityBone>();

        public Matrix3 LocalCoordinate { get; private set; }

        public override Matrix4 WorldMat => CurrentWorldMat;

        private Matrix4 CurrentWorldMat { get; set; }

        public EntityBone(World world, PmxBone bone, Bone mmmBone) : base(world)
        {
            PmxBone = bone;
            MMMBone = mmmBone;
        }

        public override void Spawn()
        {
            base.Spawn();

            Frame();
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

            LocalCoordinate = Matrix3.LookAt(PmxBone.ArrowVec, Vector3.UnitY);
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

            foreach (var bone in ParentBones)
            {
                bone.Frame();
            }

            PmxBone.UpdateLocalMatrix();
            PmxBone.UpdateWorldMatrix();

            LocalMat = PmxBone.LocalMat;
            CurrentWorldMat = new Matrix4(LocalCoordinate * PmxBone.WorldMat.Rotation, PmxBone.WorldMat.Translation);
        }

        public void SetExtraParent(EntityBone bone)
        {
            PmxBone.Flag |= BoneFlags.EXTRA;
            PmxBone.ParentBone = bone.PmxBone;
        }
    }
}
