using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Mathematics;
using CsPmx.Data;
using CsVmd.Data;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShot : Entity
    {
        public ShotProperty Property { get; }

        private Entity parentEntity;
        public override Entity ParentEntity
        {
            get => parentEntity;
            set
            {
                this.parentEntity = value;

                if (value is EntityShot entity && entity.Property.Type.RecordMotion())
                {
                    this.RootBone.ParentId = entity.RootBone.BoneId;
                }
            }
        }

        internal ShotModelData ModelData { get; }
        public PmxBoneData RootBone { get; }
        public PmxBoneData[] Bones { get; }
        public PmxMorphData MaterialMorph { get; }

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

        public EntityShot(World world, string typeName, int color) : this(world, new ShotProperty(typeName, color))
        {
        }

        public EntityShot(World world, ShotProperty property) : base(world)
        {
            this.Property = property;

            this.Property.Type.Init(this);

            ModelData = this.world.AddShot(this);
            Bones = ModelData.Bones;
            RootBone = Bones[0];
            MaterialMorph = ModelData.MaterialMorph;
        }

        internal override void Frame()
        {
            base.Frame();

            if (this.FrameCount == 1 || this.ShouldRecord(this))
            {
                this.AddVmdMotion();
            }
        }

        protected override void UpdateRot()
        {
            if (this.RotateAccodingToVelocity)
            {
                this.Rot = Matrix.LookAt(+this.Velocity, +this.Upward);
            }
            base.UpdateRot();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            this.AddVmdMorph(-this.world.FrameCount, 1.0F, this.MaterialMorph);
            this.AddVmdMorph(0, 1.0F, this.MaterialMorph);
            this.AddVmdMorph(1, 0.0F, this.MaterialMorph);

            this.AddVmdMotion();
        }

        public override void OnDeath()
        {
            base.OnDeath();

            this.AddVmdMorph(-1, 0.0F, this.MaterialMorph);
            this.AddVmdMorph(0, 1.0F, this.MaterialMorph);

            this.AddVmdMotion();
        }

        public override void SetMotionBezier(Vector2 pos1, Vector2 pos2, int length)
        {
            this.AddVmdMotion();

            base.SetMotionBezier(pos1, pos2, length);
        }

        internal override void RemoveMotionBezier()
        {
            this.AddVmdMotion(true);

            base.RemoveMotionBezier();
        }

        public void AddVertexMorph(Func<Vector3, Vector3> func)
        {
            var morph = new PmxMorphData();
        }

        public void AddVmdMotion(bool replace = false)
        {
            this.UpdateRot();

            var motion = new VmdMotionFrameData()
            {
                BoneName = this.RootBone.BoneName,
                KeyFrameNo = this.world.FrameCount,
                Pos = (DxMath.Vector3)this.Pos,
                Rot = (DxMath.Quaternion)this.Rot
            };

            if (this.motionInterpolation != null && this.motionInterpolation.startFrame < this.world.FrameCount)
            {
                replace = true;

                var interpolation = new byte[4];
                interpolation[0] = (byte)(127 * this.motionInterpolation.curve.p1.x);
                interpolation[1] = (byte)(127 * this.motionInterpolation.curve.p1.y);
                interpolation[2] = (byte)(127 * this.motionInterpolation.curve.p2.x);
                interpolation[3] = (byte)(127 * this.motionInterpolation.curve.p2.y);

                motion.InterpolatePointX = motion.InterpolatePointY = motion.InterpolatePointZ = motion.InterpolatePointR = interpolation;
            }

            this.world.VmdMotion.AddVmdMotion(motion, replace);
        }

        public void AddVmdMorph(int frameOffset, float rate, PmxMorphData morph)
        {
            if (this.Property.Type.HasMmdData())
            {
                var frameData = new VmdMorphFrameData()
                {
                    MorphName = morph.MorphName,
                    KeyFrameNo = this.world.FrameCount + frameOffset,
                    Rate = rate
                };
                this.world.VmdMotion.AddVmdMorph(frameData, morph);
            }
        }

        public PmxMorphData CreateVertexMorph(Func<Vector3, Vector3> func)
        {
            var vertices = ModelData.Vertices;

            var morph = new PmxMorphData()
            {
                MorphName = "v" + this.MaterialMorph.MorphName,
                Type = PmxMorphData.MORPHTYPE_VERTEX,
                MorphArray = new PmxMorphVertexData[vertices.Length]
            };

            for (int i = 0; i < morph.MorphArray.Length; i++)
            {
                var vertex = vertices[i];
                var pos = new Vector3()
                {
                    x = vertex.Pos.X * Property.Scale.x,
                    y = vertex.Pos.Y * Property.Scale.y,
                    z = vertex.Pos.Z * Property.Scale.z
                };

                var vertexMorph = new PmxMorphVertexData()
                {
                    Index = this.world.PmxModel.VertexList.IndexOf(vertex),
                    Position = (DxMath.Vector3)func(pos)
                };
                morph.MorphArray[i] = vertexMorph;
            }
            this.world.PmxModel.MorphList.Add(morph);
            return morph;
        }

        public void CreateVertexMorph(PythonFunction func)
        {
            this.CreateVertexMorph(v => (Vector3)PythonCalls.Call(func, v));
        }
    }
}
