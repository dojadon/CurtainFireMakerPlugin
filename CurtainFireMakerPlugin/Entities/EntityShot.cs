using System;
using System.Collections.Generic;
using System.Linq;
using MMDataIO.Pmx;
using MMDataIO.Vmd;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShot : EntityCollisonable
    {
        public ShotProperty Property { get; }

        public ShotModelData ModelData { get; }
        public PmxBoneData RootBone => ModelData.Bones[0];

        public Vector3 Upward { get; set; } = Vector3.UnitY;
        public Vector3 LookAtVec { get; set; }

        public override Vector3 Velocity
        {
            get => base.Velocity;
            set
            {
                base.Velocity = value;
                if (value != Vector3.Zero) LookAtVec = +value;
            }
        }

        public Colliding Colliding { get; set; } = Colliding.None;

        public Func<EntityShot, Vector3> GetRecordedPos { get; set; } = e => e.Pos;
        public Func<EntityShot, Quaternion> GetRecordedRot { get; set; } = e => Matrix3.LookAt(e.LookAtVec, e.Upward);

        protected override bool IsCollisionable { get => Colliding != Colliding.None; set => Colliding = value ? Colliding : Colliding.None; }

        public EntityShot(World world, string typeName, int color, EntityShot parentEntity = null)
        : this(world, typeName, color, Matrix4.Identity, parentEntity) { }

        public EntityShot(World world, string typeName, int color, float scale, EntityShot parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityShot(World world, string typeName, int color, Vector3 scale, EntityShot parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityShot(World world, string typeName, int color, Matrix3 scale, EntityShot parentEntity = null)
        : this(world, typeName, color, (Matrix4)scale, parentEntity) { }

        public EntityShot(World world, string typeName, int color, Matrix4 scale, EntityShot parentEntity = null)
        : this(world, new ShotProperty(world.ShotTypeProvider.GetShotType(typeName), color, scale), parentEntity) { }

        public EntityShot(World world, ShotProperty property, EntityShot parentEntity = null) : base(world, parentEntity)
        {
            try
            {
                Property = property;

                ModelData = World.AddShot(this);

                RootBone.ParentId = ParentEntity is EntityShot entity ? entity.RootBone.BoneId : RootBone.ParentId;

                Property.Type.InitEntity(this);
            }
            catch (Exception e)
            {
                try { Console.WriteLine(World.Executor.FormatException(e)); } catch { }
                Console.WriteLine(e);
            }
        }

        protected override void Record()
        {
            base.Record();
            AddRootBoneKeyFrame();
        }

        protected override bool ShouldRecord() => World.FrameCount == 0 || base.ShouldRecord();

        public override void OnSpawn()
        {
            base.OnSpawn();

            if (World.FrameCount > 0)
            {
                AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, -1, -1);
            }
            AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, -World.FrameCount, -1);
            AddRootBoneKeyFrame();
        }

        public override void OnDeath()
        {
            base.OnDeath();

            AddRootBoneKeyFrame(frameOffset: 0, priority: 0);
            AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, 1, -1);
        }

        public override void OnCollided(Triangle tri, float time)
        {
            Colliding.OnCollide(this, tri, time);
        }

        public override void SetMotionInterpolationCurve(Vector2 pos1, Vector2 pos2, int length, bool isSyncingVelocity = true)
        {
            AddRootBoneKeyFrame(frameOffset: 0, priority: 0);
            base.SetMotionInterpolationCurve(pos1, pos2, length, isSyncingVelocity);
        }

        public override void RemoveMotionInterpolationCurve()
        {
            AddRootBoneKeyFrame(frameOffset: 0, priority: 1);
            base.RemoveMotionInterpolationCurve();
        }

        public void AddRootBoneKeyFrame(int frameOffset = 0, int priority = 0)
        {
            var curve = MotionInterpolation?.StartFrame < World.FrameCount ? MotionInterpolation.Curve : CubicBezierCurve.Line;
            AddBoneKeyFrame(RootBone, GetRecordedPos(this), GetRecordedRot(this), curve, frameOffset, priority);
        }

        public void AddBoneKeyFrame(PmxBoneData bone, Vector3 pos, Quaternion rot, CubicBezierCurve curve, int frameOffset = 0, int priority = 0)
        {
            var frame = new VmdMotionFrameData(bone.BoneName, World.FrameCount + frameOffset, pos, rot);
            frame.InterpolationPointX1 = frame.InterpolationPointY1 = frame.InterpolationPointZ1 = curve.P1;
            frame.InterpolationPointX2 = frame.InterpolationPointY2 = frame.InterpolationPointZ2 = curve.P2;
            World.KeyFrames.AddBoneKeyFrame(frame, priority);
        }

        public void AddMorphKeyFrame(PmxMorphData morph, float weight, int frameOffset = 0, int priority = 0)
        {
            var frame = new VmdMorphFrameData(morph.MorphName, World.FrameCount + frameOffset, weight);
            World.KeyFrames.AddMorphKeyFrame(frame, priority);
        }

        public PmxMorphData CreateVertexMorph(Func<Vector3, Vector3> func)
        {
            return ModelData.CreateVertexMorph("V" + EntityId, func);
        }
    }

    public class Colliding
    {
        public Action<EntityShot, Triangle, float> OnCollide { get; private set; }

        public static readonly Colliding None = new Colliding() { OnCollide = (e, tri, time) => { } };
        public static readonly Colliding Vanish = new Colliding() { OnCollide = (e, tri, time) => e.OnDeath() };
        public static readonly Colliding Stick = new Colliding()
        {
            OnCollide = (e, tri, time) =>
            {
                e.Pos += e.Velocity * time;
                e.Velocity = Vector3.Zero;
            }
        };
        public static readonly Colliding Reflect = new Colliding()
        {
            OnCollide = (e, tri, time) =>
            {
                e.Pos += e.Velocity * time + tri.Normal * 2.0F;
                e.Velocity = tri.Normal * (e.Velocity * tri.Normal * -2) + e.Velocity;
            }
        };
    }
}