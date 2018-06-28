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
    public class EntityShot : EntityShootable
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
            AddRootBoneKeyFrame();
        }

        protected override bool ShouldRecord() => World.FrameCount == 0 || base.ShouldRecord();

        public override bool Spawn()
        {
            if (base.Spawn())
            {
                if (World.FrameCount > 0)
                {
                    AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, -1, -1);
                }
                AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, -World.FrameCount, -1);
                AddRootBoneKeyFrame();

                return true;
            }
            return false;
        }

        public override bool Remove(bool isFinalize = false)
        {
            if (base.Remove(isFinalize))
            {
                AddRootBoneKeyFrame(frameOffset: 0, priority: 0);
                AddBoneKeyFrame(RootBone, new Vector3(0, -5000000, 0), Quaternion.Identity, CubicBezierCurve.Line, 1, -1);
                return true;
            }
            return false;
        }

        public override void Frame()
        {
            TaskScheduler.Frame();
            base.Frame();
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
}