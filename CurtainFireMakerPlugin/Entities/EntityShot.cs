using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Mathematics;
using CsPmx.Data;
using CsVmd.Data;
using MikuMikuPlugin;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShot : Entity
    {
        public ShotProperty Property { get; }
        public Entity ParentEntity { get => this.parentEntity; set { this.parentEntity = value; } }

        public PmxBoneData rootBone;
        public PmxBoneData[] bones;
        public PmxMorphData materialMorph;

        private delegate bool ShouldRecordMotion(EntityShot entity);
        private static ShouldRecordMotion WhenVelocityChanges = e => !e.Velocity.Equals(e.PrevVelocity) || !e.Upward.Equals(e.PrevUpward);
        private static ShouldRecordMotion WhenPosChanges = e => !e.Pos.Equals(e.PrevPos) || !e.Rot.Equals(e.PrevRot);

        private ShouldRecordMotion ShouldRecord = WhenVelocityChanges;

        public bool RotateAccodingToVelocity { get; set; }
        public bool RecordWhenVelocityChanges
        {
            set
            {
                this.ShouldRecord = value ? WhenVelocityChanges : WhenPosChanges;
                this.RotateAccodingToVelocity = value;
            }
        }

        public EntityShot(string typeName, int color) : this(new ShotProperty(typeName, color))
        {
        }

        public EntityShot(ShotProperty property)
        {
            this.world = World.Instance;

            this.Property = property;

            this.world.AddShot(this);
        }

        public override void Frame()
        {
            base.Frame();

            this.Property.Type.Frame(this);

            if (this.FrameCount == 1 || this.ShouldRecord(this))
            {
                this.AddVmdMotion(0);
            }
        }

        protected override void UpdateRot()
        {
            if (this.RotateAccodingToVelocity)
            {
                this.Rot =(Quaternion)Matrix.LookAtLH(Vector3.Zero, this.Velocity, this.Upward);
            }
            else
            {
                base.UpdateRot();
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            this.AddVmdMorph(-this.world.FrameCount, 1.0F);
            this.AddVmdMorph(0, 1.0F);
            this.AddVmdMorph(1, 0.0F);

            this.AddVmdMotion(0);
        }

        public override void OnDeath()
        {
            base.OnDeath();

            this.AddVmdMorph(-1, 0.0F);
            this.AddVmdMorph(0, 1.0F);

            this.AddVmdMotion(0);
        }

        public void AddVmdMotion(int frameOffset, bool replace = false)
        {
            this.UpdateRot();

            var motion = new VmdMotionFrameData();
            motion.boneName = this.rootBone.boneName;
            motion.keyFrameNo = this.world.FrameCount + frameOffset;
            motion.pos = (DxMath.Vector3)this.Pos;
            motion.rot =(DxMath.Quaternion) this.Rot;

            if (this.motionInterpolation != null && this.motionInterpolation.startFrame < this.world.FrameCount)
            {
                replace = true;
            }

            this.world.motion.AddVmdMotion(motion, replace);
        }

        public void AddVmdMorph(int frameOffset, float rate)
        {
            var morph = new VmdMorphFrameData();
            morph.morphName = this.materialMorph.morphName;
            morph.keyFrameNo = this.world.FrameCount + frameOffset;
            morph.rate = rate;

            this.world.motion.AddVmdMorph(morph);
        }
    }
}
