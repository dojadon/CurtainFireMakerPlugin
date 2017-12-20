using System;
using System.Collections.Generic;
using System.Linq;
using CsMmdDataIO.Pmx;
using CsMmdDataIO.Vmd;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShot : EntityShotBase
    {
        public ShotProperty Property { get; }

        public ShotModelData ModelData { get; }
        public PmxBoneData RootBone => ModelData.Bones[0];

        public IMotionRecorder MotionRecorder { get; set; } = VmdMotionRecorder.Instance;
        public IRecording Recording { get; set; } = Entities.Recording.Velocity;

        public EntityShot(World world, ShotType type, int color, EntityShot parentEntity = null)
        : this(world, new ShotProperty(type, color), parentEntity) { }

        public EntityShot(World world, ShotType type, int color, short group, EntityShot parentEntity = null)
        : this(world, new ShotProperty(type, color, group), parentEntity) { }

        public EntityShot(World world, ShotProperty property, EntityShot parentEntity = null) : base(world, parentEntity)
        {
            Property = property;

            ModelData = World.AddShot(this);
            ModelData.OwnerEntities.Add(this);

            RootBone.ParentId = ParentEntity is EntityShot entity ? entity.RootBone.BoneId : RootBone.ParentId;

            Property.Type.InitEntity(this);
            Property.Type.InitModelData(ModelData);
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

        protected override void UpdateTasks()
        {
            base.UpdateTasks();

            if (Recording.ShouldRecord(this) || World.FrameCount == 0)
            {
                AddBoneKeyFrame();
            }
            IsUpdatedVelocity = IsUpdatedLocalMat = false;
        }

        public override void SetMotionInterpolationCurve(Vector2 pos1, Vector2 pos2, int length, bool isSyncingVelocity = true)
        {
            AddBoneKeyFrame(frameOffset: 0, priority: 0);
            base.SetMotionInterpolationCurve(pos1, pos2, length, isSyncingVelocity);
        }

        public override void RemoveMotionInterpolationCurve()
        {
            AddBoneKeyFrame(frameOffset: 0, priority: 1);
            base.RemoveMotionInterpolationCurve();
        }

        public void AddBoneKeyFrame(int frameOffset = 0, int priority = 0)
        {
            var curve = MotionInterpolation?.StartFrame < World.FrameCount ? MotionInterpolation.Curve : CubicBezierCurve.Line;
            AddBoneKeyFrame(RootBone, Recording.GetRecordedPos(this), Recording.GetRecordedRot(this), curve, frameOffset, priority);
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
}