using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public abstract class EntityShootable : Entity
    {
        public bool IsUpdatedVelocity { get; protected set; } = true;
        public bool IsUpdatedLocalMat { get; protected set; } = true;

        private static float Epsilon { get; set; } = 0.00001F;

        public Vector3 LookAtVec { get; set; }

        private Vector3 velocity;
        public Vector3 Velocity
        {
            get => velocity;
            set
            {
                if (!Vector3.EpsilonEquals(velocity, (velocity = value), Epsilon)) OnVelocityUpdated();
            }
        }

        public Vector3 Upward { get; set; } = Vector3.UnitY;

        public override Vector3 Pos
        {
            get => base.Pos;
            set { if (!Vector3.EpsilonEquals(base.Pos, (base.Pos = value), Epsilon)) OnLocalMatChanged(); }
        }

        public override Quaternion Rot
        {
            get => base.Rot;
            set { if (!Quaternion.EpsilonEquals(base.Rot, (base.Rot = value), Epsilon)) OnLocalMatChanged(); }
        }

        protected MotionInterpolation MotionInterpolation { get; set; }

        public EntityShootable(World world, Entity parentEntity = null) : base(world, parentEntity) { }

        public override void Frame()
        {
            base.Frame();

            Pos += GetInterpolatedVelocity();
            IsUpdatedVelocity = IsUpdatedLocalMat = false;
        }

        protected virtual void OnVelocityUpdated()
        {
            IsUpdatedVelocity = true;
            if (Velocity != Vector3.Zero)
            {
                LookAtVec = +Velocity;
            }
        }

        protected virtual void OnLocalMatChanged()
        {
            IsUpdatedLocalMat = true;
        }

        protected virtual Vector3 GetInterpolatedVelocity()
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
                    RemoveMotionInterpolationCurve();
                }
            }
            return interpolatedVelocity;
        }

        public virtual void SetMotionInterpolationCurve(Vector2 pos1, Vector2 pos2, int length, bool isSyncingVelocity = true)
        {
            MotionInterpolation = new MotionInterpolation(World.FrameCount, length, pos1, pos2, isSyncingVelocity);
        }

        public virtual void RemoveMotionInterpolationCurve()
        {
            MotionInterpolation = null;
        }
    }

    public class MotionInterpolation
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
