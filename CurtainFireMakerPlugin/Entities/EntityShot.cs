using System;
using System.Collections.Generic;
using System.Linq;
using CsMmdDataIO.Pmx;
using CsMmdDataIO.Vmd;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShot : Entity
    {
        public ShotProperty Property { get; }

        public ShotModelData ModelData { get; }
        public PmxBoneData RootBone => ModelData.Bones[0];

        public IMotionRecorder MotionRecorder { get; set; } = VmdMotionRecorder.Instance;
        public IRecording Recording { get; set; } = Entities.Recording.Velocity;

        public bool IsUpdatedVelocity { get; private set; } = true;
        public bool IsUpdatedLocalMat { get; private set; } = true;

        private static float Epsilon { get; set; } = 0.00001F;

        public Vector3 LookAtVec { get; set; }

        public override Vector3 Velocity
        {
            get => base.Velocity;
            set
            {
                IsUpdatedVelocity |= !Vector3.EpsilonEquals(base.Velocity, (base.Velocity = value), Epsilon);
                if (Velocity != Vector3.Zero) LookAtVec = Velocity;
            }
        }

        public override Vector3 Upward
        {
            get => base.Upward;
            set => IsUpdatedVelocity |= !Vector3.EpsilonEquals(base.Upward, (base.Upward = value), Epsilon);
        }

        public override Vector3 Pos
        {
            get => base.Pos;
            set => IsUpdatedLocalMat |= !Vector3.EpsilonEquals(base.Pos, (base.Pos = value), Epsilon);
        }

        public override Quaternion Rot
        {
            get => base.Rot;
            set => IsUpdatedLocalMat |= !Quaternion.EpsilonEquals(base.Rot, (base.Rot = value), Epsilon);
        }

        private MotionInterpolation MotionInterpolation { get; set; }

        public EntityShot(World world, ShotType type, int color, EntityShot parentEntity = null)
        : this(world, new ShotProperty(type, color), parentEntity) { }

        public EntityShot(World world, ShotType type, int color, short group, EntityShot parentEntity = null)
        : this(world, new ShotProperty(type, color, group), parentEntity) { }

        public EntityShot(World world, ShotProperty property, EntityShot parentEntity = null) : base(world, parentEntity)
        {
            try
            {
                Property = property;

                ModelData = World.AddShot(this);
                ModelData.OwnerEntities.Add(this);

                RootBone.ParentId = ParentEntity is EntityShot entity ? entity.RootBone.BoneId : RootBone.ParentId;

                Property.Type.InitEntity(this);
                Property.Type.InitModelData(ModelData);
            }
            catch (Exception e)
            {
                try { Console.WriteLine(Plugin.Instance.PythonExecutor.FormatException(e)); } catch { }
                Console.WriteLine(e);
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            if (World.FrameCount > 0)
            {
                AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, -1, -1);
                AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, -World.FrameCount, -1);
            }
            AddBoneKeyFrame();
        }

        public override void OnDeath()
        {
            base.OnDeath();

            AddBoneKeyFrame(frameOffset: -1);
            AddBoneKeyFrame(RootBone, (Pos = new Vector3(0, -5000000, 0)), Quaternion.Identity, CubicBezierCurve.Line, 0, -1);
        }

        public override void Frame()
        {
            if (Recording.ShouldRecord(this) || World.FrameCount == 0)
            {
                AddBoneKeyFrame();
            }
            IsUpdatedVelocity = IsUpdatedLocalMat = false;

            base.Frame();
        }

        protected override void UpdateLocalMat()
        {
            Vector3 interpolatedVelocity = Velocity;

            if (MotionInterpolation != null && World.FrameCount <= MotionInterpolation.EndFrame)
            {
                if (World.FrameCount < MotionInterpolation.EndFrame)
                {
                    interpolatedVelocity *= MotionInterpolation.GetChangeAmount(World.FrameCount);

                    if (MotionInterpolation.EndFrame < World.FrameCount + 1 && MotionInterpolation.IsSyncVelocity)
                    {
                        Velocity = interpolatedVelocity;
                    }
                }
                else
                {
                    AddBoneKeyFrame(frameOffset: 0, priority: 1);
                    MotionInterpolation = null;
                }
            }
            Pos += interpolatedVelocity;
        }

        public void SetMotionInterpolationCurve(Vector2 pos1, Vector2 pos2, int length, bool isSyncingVelocity = true)
        {
            AddBoneKeyFrame();
            MotionInterpolation = new MotionInterpolation(World.FrameCount, length, pos1, pos2, isSyncingVelocity);
        }

        public void AddBoneKeyFrame(int frameOffset = 0, int priority = 0)
        {
            var posCurve = CubicBezierCurve.Line;

            if (MotionInterpolation != null && MotionInterpolation.StartFrame < World.FrameCount)
            {
                posCurve = MotionInterpolation.Curve;
            }
            AddBoneKeyFrame(RootBone, Recording.GetRecordedPos(this), Recording.GetRecordedRot(this), posCurve, frameOffset, priority);
        }

        public void AddBoneKeyFrame(PmxBoneData bone, Vector3 pos, Quaternion rot, CubicBezierCurve posCurve, int frameOffset = 0, int priority = 0)
        {
            MotionRecorder.AddBoneKeyFrame(World, bone, pos, rot, posCurve, World.FrameCount + frameOffset, priority);
        }

        public void AddMorphKeyFrame(PmxMorphData morph, float weight, int frameOffset = 0, int priority = 0)
        {
            MotionRecorder.AddMorphKeyFrame(World, morph, weight, World.FrameCount + frameOffset, priority);
        }

        public PmxMorphData CreateVertexMorph(Func<Vector3, Vector3> func)
        {
            return ModelData.CreateVertexMorph("V" + EntityId, func);
        }
    }

    internal class MotionInterpolation
    {
        public CubicBezierCurve Curve { get; }
        public int StartFrame { get; }
        public int EndFrame { get; }
        public int Length { get; }
        public bool IsSyncVelocity { get; }

        public MotionInterpolation(int startFrame, int length, Vector2 p1, Vector2 p2, bool isSync)
        {
            Curve = new CubicBezierCurve(new Vector2(0, 0), p1, p2, new Vector2(1, 1));

            StartFrame = startFrame;
            Length = length;
            EndFrame = StartFrame + Length;
            IsSyncVelocity = isSync;
        }

        public bool Within(int frame)
        {
            return StartFrame <= frame && frame < EndFrame;
        }

        public float GetChangeAmount(int frame)
        {
            if (Within(frame))
            {
                float unit = 1.0F / Length;

                float x1 = (frame - StartFrame) * unit;
                float x2 = x1 + unit;

                return (FuncY(x2) - FuncY(x1)) * Length;
            }
            else
            {
                return 1.0F;
            }
        }

        public float FuncY(float x)
        {
            float[] t = Curve.SolveTimeFromX(x);

            if (t.Length == 0)
            {
                throw new ArithmeticException($"ベジエ曲線の解が見つかりません : x[ {x} ]");
            }

            float time = t[0];

            if (t.Length > 1)
            {
                for (int i = 1; i < t.Length; i++)
                {
                    if (Math.Abs(x - time) > Math.Abs(x - t[i])) time = t[i];
                }
            }
            return Curve.Y(time);
        }
    }
}