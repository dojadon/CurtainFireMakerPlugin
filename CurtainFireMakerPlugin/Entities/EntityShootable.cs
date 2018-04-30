using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public abstract class EntityShootable : EntityShotBase
    {
        public virtual Vector3 Velocity { get; set; }

        public Vector3 PrevPos { get; private set; }
        public Quaternion PrevRot { get; private set; }
        public Vector3 PrevVelocity { get; private set; }

        public static float Epsilon = 1E-4F;

        protected MotionInterpolation MotionInterpolation { get; set; }

        public EntityShootable(World world, string typeName, int color, EntityShotBase parentEntity = null)
        : this(world, typeName, color, Matrix4.Identity, parentEntity) { }

        public EntityShootable(World world, string typeName, int color, float scale, EntityShotBase parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityShootable(World world, string typeName, int color, Vector3 scale, EntityShotBase parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityShootable(World world, string typeName, int color, Matrix3 scale, EntityShotBase parentEntity = null)
        : this(world, typeName, color, (Matrix4)scale, parentEntity) { }

        public EntityShootable(World world, string typeName, int color, Matrix4 scale, EntityShotBase parentEntity = null)
        : this(world, new ShotProperty(world.ShotTypeProvider.GetShotType(typeName), color, scale), parentEntity) { }

        public EntityShootable(World world, ShotProperty property, EntityShotBase parentEntity = null) : base(world, property, parentEntity) { }

        public override void Frame()
        {
            base.Frame();

            if (MotionInterpolation != null && World.FrameCount == MotionInterpolation.EndFrame)
            {
                RemoveMotionInterpolationCurve();
            }

            if (ShouldRecord())
            {
                Record();
            }

            Pos += Velocity;

            PrevPos = Pos;
            PrevRot = Rot;
            PrevVelocity = Velocity;
        }

        protected virtual bool ShouldRecord() => !(Vector3.EpsilonEquals(Velocity, PrevVelocity, Epsilon) && Vector3.EpsilonEquals(Pos, PrevPos, Epsilon) && Quaternion.EpsilonEquals(Rot, PrevRot, Epsilon));

        protected abstract void Record();

        public virtual void SetMotionInterpolationCurve(Vector2 pos1, Vector2 pos2, int length, bool isKeepTerminalSlope = true)
        {
            MotionInterpolation = new MotionInterpolation(World.FrameCount, length, pos1, pos2, isKeepTerminalSlope);
        }

        public virtual void RemoveMotionInterpolationCurve()
        {
            if (MotionInterpolation.IsKeepTerminalSlope)
            {
                Velocity *= (1.0F - MotionInterpolation.Curve.P2.y) / (1.0F - MotionInterpolation.Curve.P2.x);
            }
            MotionInterpolation = null;
        }
    }

    public class MotionInterpolation
    {
        public CubicBezierCurve Curve { get; }
        public int StartFrame { get; }
        public int EndFrame { get; }
        public bool IsKeepTerminalSlope { get; }

        public MotionInterpolation(int startFrame, int length, Vector2 p1, Vector2 p2, bool isKeepTerminalSlope)
        {
            Curve = new CubicBezierCurve(new Vector2(0, 0), p1, p2, new Vector2(1, 1));

            StartFrame = startFrame;
            EndFrame = StartFrame + length;
            IsKeepTerminalSlope = isKeepTerminalSlope;
        }
    }
}
