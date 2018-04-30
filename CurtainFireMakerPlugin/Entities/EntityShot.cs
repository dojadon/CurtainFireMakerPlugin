using System;
using System.Collections.Generic;
using System.Linq;
using MMDataIO.Pmx;
using MMDataIO.Vmd;
using VecMath;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShot : EntityCollisonable
    {
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
        public Func<EntityShot, Vector3> GetRecordedPos { get; set; } = e => e.Pos;
        public Func<EntityShot, Quaternion> GetRecordedRot { get; set; } = e => Matrix3.LookAt(e.LookAtVec, e.Upward);

        public Colliding Colliding { get; set; } = Colliding.None;

        protected override bool IsCollisionable { get => Colliding != Colliding.None; set => Colliding = value ? Colliding : Colliding.None; }

        private ScheduledTaskManager TaskScheduler { get; } = new ScheduledTaskManager();

        public EntityShot(World world, string typeName, int color, EntityShotBase parentEntity = null)
        : this(world, typeName, color, Matrix4.Identity, parentEntity) { }

        public EntityShot(World world, string typeName, int color, float scale, EntityShotBase parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityShot(World world, string typeName, int color, Vector3 scale, EntityShotBase parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityShot(World world, string typeName, int color, Matrix3 scale, EntityShotBase parentEntity = null)
        : this(world, typeName, color, (Matrix4)scale, parentEntity) { }

        public EntityShot(World world, string typeName, int color, Matrix4 scale, EntityShotBase parentEntity = null)
        : this(world, new ShotProperty(world.ShotTypeProvider.GetShotType(typeName), color, scale), parentEntity) { }

        public EntityShot(World world, ShotProperty property, EntityShotBase parentEntity = null) : base(world, property, parentEntity)
        {
        }

        protected override void Record()
        {
            base.Record();
            AddRootBoneKeyFrame();
        }

        protected override bool ShouldRecord() => World.FrameCount == 0 || base.ShouldRecord();

        public override void Spawn()
        {
            base.Spawn();

            if (World.FrameCount > 0)
            {
                AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, -1, -1);
            }
            AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, -World.FrameCount, -1);
            AddRootBoneKeyFrame();
        }

        public override void Remove(bool isFinalize = false)
        {
            base.Remove(isFinalize);

            AddRootBoneKeyFrame(frameOffset: 0, priority: 0);
            AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, 1, -1);
        }

        public override void Frame()
        {
            TaskScheduler.Frame();
            base.Frame();
        }

        public override void OnCollided(Vector3 normal, float time)
        {
            Colliding.OnCollide(this, normal, time);
        }

        public override void SetMotionInterpolationCurve(Vector2 pos1, Vector2 pos2, int length, bool isKeepTerminalSlope = true)
        {
            AddRootBoneKeyFrame(frameOffset: 0, priority: 0);
            base.SetMotionInterpolationCurve(pos1, pos2, length, isKeepTerminalSlope);
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

        private void AddTask(ScheduledTask task)
        {
            TaskScheduler.AddTask(task);
        }

        private void AddTask(PythonFunction task, Func<int, int> interval, int executeTimes, int waitTime, bool withArg = false)
        {
            if (withArg)
            {
                AddTask(new ScheduledTask(t => PythonCalls.Call(task, t), interval, executeTimes, waitTime));
            }
            else
            {
                AddTask(new ScheduledTask(t => PythonCalls.Call(task), interval, executeTimes, waitTime));
            }
        }

        public void AddTask(PythonFunction task, PythonFunction interval, int executeTimes, int waitTime, bool withArg = false)
        {
            AddTask(task, i => (int)PythonCalls.Call(interval, i), executeTimes, waitTime, withArg);
        }

        public void AddTask(PythonFunction task, int interval, int executeTimes, int waitTime, bool withArg = false)
        {
            AddTask(task, i => interval, executeTimes, waitTime, withArg);
        }
    }

    public class Colliding
    {
        public Action<EntityShot, Vector3, float> OnCollide { get; private set; }

        public static readonly Colliding None = new Colliding() { OnCollide = (e, tri, time) => { } };
        public static readonly Colliding Vanish = new Colliding() { OnCollide = (e, tri, time) => e.Remove() };
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
            OnCollide = (e, normal, time) =>
            {
                e.Pos += e.Velocity * time + normal * 2.0F;
                e.Velocity = normal * (e.Velocity * normal * -2) + e.Velocity;
            }
        };
    }
}