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
                motion.boneName = entity.bones[1].boneName;
                motion.keyFrameNo = entity.world.FrameCount;

                motion.rot = (DxMath.Quaternion)Quaternion.RotationAxisAngle(Vector3.UnitZ, angle);
                entity.world.motion.AddVmdMotion(motion);

                motion = this.GetMotionData();
                motion.boneName = entity.bones[2].boneName;
                motion.keyFrameNo = entity.world.FrameCount;

                motion.rot = (DxMath.Quaternion)Quaternion.RotationAxisAngle(Vector3.UnitZ, -angle);
                entity.world.motion.AddVmdMotion(motion);

            }, 120, 0, 0);

            entity.AddTask(task =>
            {
                var motion = this.GetMotionData();
                motion.boneName = entity.bones[1].boneName;
                motion.keyFrameNo = entity.world.FrameCount;

                motion.rot = (DxMath.Quaternion)Quaternion.RotationAxisAngle(Vector3.UnitZ, -angle);
                entity.world.motion.AddVmdMotion(motion);

                motion = this.GetMotionData();
                motion.boneName = entity.bones[2].boneName;
                motion.keyFrameNo = entity.world.FrameCount;

                motion.rot = (DxMath.Quaternion)Quaternion.RotationAxisAngle(Vector3.UnitZ, angle);
                entity.world.motion.AddVmdMotion(motion);

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

            motion.interpolatePointX = motion.interpolatePointY = motion.interpolatePointZ = motion.interpolatePointR = interpolation;
            return motion;
        }
    }
}
