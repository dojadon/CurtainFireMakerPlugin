using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;
using CsMmdDataIO.Vmd;
using CsMmdDataIO.Pmx;

namespace CurtainFireMakerPlugin.Entities
{
    public interface IMotionRecorder
    {
        void AddBoneKeyFrame(World world, PmxBoneData bone, Vector3 pos, Quaternion rot, CubicBezierCurve posCurve, int frameTime, int priority);

        void AddMorphKeyFrame(World world, PmxMorphData morph, float weight, int frameTime, int priority);
    }

    public class VmdMotionRecorder : IMotionRecorder
    {
        public static VmdMotionRecorder Instance { get; } = new VmdMotionRecorder();

        public void AddBoneKeyFrame(World world, PmxBoneData bone, Vector3 pos, Quaternion rot, CubicBezierCurve posCurve, int frameTime, int priority)
        {
            var frame = new VmdMotionFrameData(bone.BoneName, frameTime, pos, rot);
            frame.InterpolationPointX1 = frame.InterpolationPointY1 = frame.InterpolationPointZ1 = posCurve.P1;
            frame.InterpolationPointX2 = frame.InterpolationPointY2 = frame.InterpolationPointZ2 = posCurve.P2;
            world.KeyFrames.AddBoneKeyFrame(bone, frame, priority);
        }

        public void AddMorphKeyFrame(World world, PmxMorphData morph, float weight, int frameTime, int priority)
        {
            var frame = new VmdMorphFrameData(morph.MorphName, frameTime, weight);
            world.KeyFrames.AddMorphKeyFrame(morph, frame, priority);
        }
    }
}
