using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Mathematics;
using CsPmx.Data;
using CsVmd.Data;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShot : Entity
    {
        public ShotProperty Property { get; }
        public Entity ParentEntity { get => this.parentEntity; set { this.parentEntity = value; } }

        public PmxBoneData rootBone;
        public PmxBoneData[] bones;
        public PmxMorphData materialMorph;

        private delegate bool RecordMotion(EntityShot entity);
        private static RecordMotion WhenVelocityChanges = e => !e.Velocity.Equals(e.PrevVelocity) || !e.Upward.Equals(e.PrevUpward);
        private static RecordMotion WhenPosChanges = e => !e.Pos.Equals(e.PrevPos) || !e.Rot.Equals(e.PrevRot);

        private RecordMotion ShouldRecord = WhenVelocityChanges;

        public bool RotateAccodingToVelocity { get; set; } = true;
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
                if (this.Velocity != Vector3.Zero && this.Upward != Vector3.Zero)
                {
                    var mat = Matrix.LookAt(+this.Velocity, +this.Upward);
                    this.Rot = mat;

                    if (this.FrameCount == 10 && this.Property.Type.Name.Equals("AMULET"))
                    {
                        Console.WriteLine("----------------------------------------------------------------------------");
                        Console.WriteLine(+this.Velocity + ", " + +this.Upward);
                        Console.WriteLine("*********************************************************");
                        Console.WriteLine(mat);
                        Console.WriteLine("*********************************************************");
                        Console.WriteLine((Matrix)this.Rot);
                        Console.WriteLine("----------------------------------------------------------------------------");
                    }
                }
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

        public override void SetMotionBezier(Vector2 pos1, Vector2 pos2, int length)
        {
            this.AddVmdMotion(0);

            base.SetMotionBezier(pos1, pos2, length);
        }

        public override void RemoveMotionBezier()
        {
            this.AddVmdMotion(0, true);

            base.RemoveMotionBezier();
        }

        public void AddVmdMotion(int frameOffset, bool replace = false)
        {
            this.UpdateRot();

            var motion = new VmdMotionFrameData();
            motion.boneName = this.rootBone.boneName;
            motion.keyFrameNo = this.world.FrameCount + frameOffset;
            motion.pos = (DxMath.Vector3)this.Pos;
            motion.rot = (DxMath.Quaternion)this.Rot;

            if (this.motionInterpolation != null && this.motionInterpolation.startFrame < this.world.FrameCount)
            {
                replace = true;

                var interpolation = new byte[4];
                interpolation[0] = (byte)(127 * this.motionInterpolation.curve.p1.x);
                interpolation[1] = (byte)(127 * this.motionInterpolation.curve.p1.y);
                interpolation[2] = (byte)(127 * this.motionInterpolation.curve.p2.x);
                interpolation[3] = (byte)(127 * this.motionInterpolation.curve.p2.y);

                motion.interpolatePointX = motion.interpolatePointY = motion.interpolatePointZ = motion.interpolatePointR = interpolation;
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
