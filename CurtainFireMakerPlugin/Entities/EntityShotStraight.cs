using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShotStraight : EntityShotBase
    {
        public override bool IsReusable => LifeSpan != 0 && World.FrameCount > SpawnFrameNo + LifeSpan;

        public override bool IsNeededUpdate => false;

        public Vector3 InitialPos { get; private set; }

        public override Vector3 Pos { get => IsSpawned ? InitialPos + Velocity * (World.FrameCount - SpawnFrameNo) : base.Pos; set => base.Pos = value; }

        public Vector3 Velocity { get; set; }
        public Vector3 Upward { get; set; } = Vector3.UnitY;

        public Func<EntityShotStraight, Quaternion> GetRecordedRot { get; set; } = e => Matrix3.LookAt(e.Velocity, e.Upward);

        public EntityShotStraight(World world, string typeName, int color)
           : this(world, typeName, color, Matrix4.Identity) { }

        public EntityShotStraight(World world, string typeName, int color, float scale)
        : this(world, typeName, color, new Matrix3(scale)) { }

        public EntityShotStraight(World world, string typeName, int color, Vector3 scale)
        : this(world, typeName, color, new Matrix3(scale)) { }

        public EntityShotStraight(World world, string typeName, int color, Matrix3 scale)
        : this(world, typeName, color, (Matrix4)scale) { }

        public EntityShotStraight(World world, string typeName, int color, Matrix4 scale)
        : this(world, new ShotProperty(world.ShotTypeProvider.GetShotType(typeName), color, scale)) { }

        public EntityShotStraight(World world, ShotProperty property) : base(world, property)
        {
        }

        public override bool Spawn()
        {
            if (base.Spawn())
            {
                InitialPos = Pos;
                Rot = GetRecordedRot(this);

                if (World.FrameCount > 0)
                {
                    AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, -1, -1);
                }
                AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, -World.FrameCount, -1);
                AddBoneKeyFrame(RootBone, InitialPos, Rot, CubicBezierCurve.Line, 0, 0);

                AddBoneKeyFrame(RootBone, InitialPos + Velocity * LifeSpan, Rot, CubicBezierCurve.Line, LifeSpan, 0);
                AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, LifeSpan + 1, -1);
                return true;
            }
            return false;
        }
    }
}
