using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShot : Entity
    {
        public ShotProperty Property { get; }
        public Entity ParentEntity { get => this.parentEntity; set { this.parentEntity = value; } }

        private delegate bool ShouldRecordMotion(EntityShot entity);
        private static ShouldRecordMotion WhenVelocityChanges = e => e.Velocity.Equals(e.PrevVelocity) || e.Upward.Equals(e.PrevUpward);
        private static ShouldRecordMotion WhenPosChanges = e => e.Pos.Equals(e.PrevPos) || e.Rot.Equals(e.PrevRot);

        private ShouldRecordMotion ShouldRecord = WhenPosChanges;

        public bool RotateAccodingToVelocity { get; set; }
        public bool RecordWhenVelocityChanges
        {
            set
            {
                this.ShouldRecord = value ? WhenVelocityChanges : WhenPosChanges;
                this.RotateAccodingToVelocity = false;
            }
        }

        public EntityShot(ShotProperty property)
        {
            this.Property = property;
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
                Matrix rot = Matrix.LookAtLH(Vector3.Zero, this.Velocity, this.Upward);
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

        public void AddVmdMotion(int frameOffset)
        {
            this.UpdateRot();
        }

        public void AddVmdMorph(int frameOffset, float rate)
        {

        }
    }
}
