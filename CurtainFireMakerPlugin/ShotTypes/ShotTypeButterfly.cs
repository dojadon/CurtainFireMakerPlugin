using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;
using CurtainFireMakerPlugin.Mathematics;
using CsVmd.Data;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public class ShotTypeButterfly : ShotTypePmx
    {
        public ShotTypeButterfly(string name, string path, double scale) : base(name, path, scale)
        {
        }

        public override void Init(EntityShot entity)
        {
            double angle = Math.PI / 4;
            entity.AddTask(task =>
            {
                var motion = this.GetMotionData();
                motion.BoneName = entity.Bones[1].BoneName;
                motion.KeyFrameNo = entity.world.FrameCount;

                motion.Rot = (DxMath.Quaternion)Quaternion.RotationAxisAngle(Vector3.UnitZ, angle);
                entity.world.VmdMotion.AddVmdMotion(motion);

                motion = this.GetMotionData();
                motion.BoneName = entity.Bones[2].BoneName;
                motion.KeyFrameNo = entity.world.FrameCount;

                motion.Rot = (DxMath.Quaternion)Quaternion.RotationAxisAngle(Vector3.UnitZ, -angle);
                entity.world.VmdMotion.AddVmdMotion(motion);

            }, 120, 0, 0);

            entity.AddTask(task =>
            {
                var motion = this.GetMotionData();
                motion.BoneName = entity.Bones[1].BoneName;
                motion.KeyFrameNo = entity.world.FrameCount;

                motion.Rot = (DxMath.Quaternion)Quaternion.RotationAxisAngle(Vector3.UnitZ, -angle);
                entity.world.VmdMotion.AddVmdMotion(motion);

                motion = this.GetMotionData();
                motion.BoneName = entity.Bones[2].BoneName;
                motion.KeyFrameNo = entity.world.FrameCount;

                motion.Rot = (DxMath.Quaternion)Quaternion.RotationAxisAngle(Vector3.UnitZ, angle);
                entity.world.VmdMotion.AddVmdMotion(motion);

            }, 120, 0, 60);
        }

        private VmdMotionFrameData GetMotionData()
        {
            var motion = new VmdMotionFrameData();

            var interpolation = new byte[4];
            interpolation[0] = (byte)(127 * 0.9F);
            interpolation[1] = (byte)(127 * 0.2F);
            interpolation[2] = (byte)(127 * 0.2F);
            interpolation[3] = (byte)(127 * 0.9F);

            motion.InterpolatePointX = motion.InterpolatePointY = motion.InterpolatePointZ = motion.InterpolatePointR = interpolation;
            return motion;
        }
    }
}
