using CsPmx.Data;
using CsVmd.Data;
using CurtainFireMakerPlugin.Mathematics;
using System;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShot : Entity
    {
        public ShotProperty Property { get; }

        public override Entity ParentEntity
        {
            get => base.ParentEntity;
            set => RootBone.ParentId = (base.ParentEntity = value) is EntityShot entity ? entity.RootBone.BoneId : RootBone.ParentId;
        }

        public ShotModelData ModelData { get; }
        public PmxBoneData RootBone => ModelData.Bones[0];
        public PmxMorphData MaterialMorph => ModelData.MaterialMorph;

        public class RecordType
        {
            public static Predicate<EntityShot> None { get; } = e => false;
            public static Predicate<EntityShot> Velocity { get; } = e => e.IsUpdatedVelocity;
            public static Predicate<EntityShot> LocalMat { get; } = e => e.IsUpdatedLocalMat;
        }

        public Predicate<EntityShot> ShouldRecord { get; set; } = RecordType.LocalMat;

        public bool IsUpdatedVelocity { get; private set; } = true;
        public bool IsUpdatedLocalMat { get; private set; } = true;

        public static float Epsilon { get; set; } = 0.00001F;

        public override Vector3 Velocity
        {
            get => base.Velocity;
            set => IsUpdatedVelocity |= !Vector3.EpsilonEquals(base.Velocity, (base.Velocity = value), Epsilon);
        }

        public override Vector3 Upward
        {
            get => base.Upward;
            set => IsUpdatedVelocity |= !Vector3.EpsilonEquals(base.Upward, (base.Upward = value), Epsilon);
        }

        public override Vector3 Pos
        {
            get => base.Pos;
            set => IsUpdatedLocalMat |= !Vector3.EpsilonEquals(base.Pos, (base.Pos = value), Epsilon);
        }

        public override Quaternion Rot
        {
            get => base.Rot;
            set => IsUpdatedLocalMat |= !Quaternion.EpsilonEquals(base.Rot, (base.Rot = value), Epsilon);
        }

        public Func<EntityShot, Vector3> GetRecordedPos { get; set; } = e => e.Pos;
        public Func<EntityShot, Quaternion> GetRecordedRot { get; set; } = e => e.Velocity != Vector3.Zero ? (Quaternion)Matrix3.LookAt(+e.Velocity, +e.Upward) * e.Rot : e.Rot;

        public EntityShot(World world, string typeName, int color) : this(world, new ShotProperty(typeName, color)) { }

        public EntityShot(World world, ShotProperty property) : base(world)
        {
            try
            {
                Property = property;

                ModelData = World.AddShot(this);
                ModelData.OwnerEntities.Add(this);

                Property.Type.Init(this);
                Property.Type.InitModelData(ModelData);
            }
            catch (Exception e)
            {
                try { Console.WriteLine(Plugin.Instance.PythonRunner.FormatException(e)); } catch { }
                Console.WriteLine(e);
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            if (World.FrameCount < 0)
            {
                AddVmdMorph(-World.FrameCount, 0.0F, MaterialMorph);
            }
            else
            {
                AddVmdMorph(0, 1.0F, MaterialMorph);
                AddVmdMorph(1, 0.0F, MaterialMorph);
                AddVmdMorph(-World.FrameCount, 1.0F, MaterialMorph);
            }

            AddVmdMotion();
        }

        public override void OnDeath()
        {
            base.OnDeath();

            AddVmdMorph(-1, 0.0F, MaterialMorph);
            AddVmdMorph(0, 1.0F, MaterialMorph);

            AddVmdMotion();
        }

        internal override void Frame()
        {
            if (ShouldRecord(this) || World.FrameCount == 0)
            {
                AddVmdMotion();
            }
            IsUpdatedVelocity = IsUpdatedLocalMat = false;

            base.Frame();
        }

        public override void SetMotionInterpolationCurve(Vector2 pos1, Vector2 pos2, int length)
        {
            base.SetMotionInterpolationCurve(pos1, pos2, length);

            AddVmdMotion();
        }

        protected override void RemoveMotionInterpolationCurve()
        {
            AddVmdMotion();

            base.RemoveMotionInterpolationCurve();
        }

        public void AddVmdMotion()
        {
            var bezier = CubicBezierCurve.Line;

            if (MotionInterpolation != null && MotionInterpolation.StartFrame < World.FrameCount)
            {
                bezier = MotionInterpolation.Curve;
            }

            AddVmdMotion(RootBone, GetRecordedPos(this), GetRecordedRot(this), bezier);
        }

        public void AddVmdMotion(PmxBoneData bone, Vector3 pos, Quaternion rot, CubicBezierCurve bezier)
        {
            var interpolation = new byte[4];
            interpolation[0] = (byte)(127 * bezier.P1.x);
            interpolation[1] = (byte)(127 * bezier.P1.y);
            interpolation[2] = (byte)(127 * bezier.P2.x);
            interpolation[3] = (byte)(127 * bezier.P2.y);

            var motion = new VmdMotionFrameData()
            {
                BoneName = bone.BoneName,
                KeyFrameNo = World.FrameCount,
                Pos = pos,
                Rot = rot,
                InterpolatePointX = interpolation,
                InterpolatePointY = interpolation,
                InterpolatePointZ = interpolation
            };

            World.VmdMotion.AddVmdMotion(motion);
        }

        public void AddVmdMorph(int frameOffset, float rate, PmxMorphData morph, bool replace = false)
        {
            if (Property.Type.HasMesh)
            {
                var frameData = new VmdMorphFrameData()
                {
                    MorphName = morph.MorphName,
                    KeyFrameNo = World.FrameCount + frameOffset,
                    Rate = rate
                };
                World.VmdMotion.AddVmdMorph(frameData, morph);
            }
        }

        public PmxMorphData CreateVertexMorph(string morphName, Func<Vector3, Vector3> func)
        {
            if (!ModelData.MorphDict.ContainsKey(morphName))
            {
                var vertices = ModelData.Vertices;

                var morph = new PmxMorphData()
                {
                    MorphName = morphName,
                    Type = PmxMorphData.MORPHTYPE_VERTEX,
                    MorphArray = new PmxMorphVertexData[vertices.Length]
                };

                for (int i = 0; i < morph.MorphArray.Length; i++)
                {
                    var vertex = vertices[i];
                    var vertexMorph = new PmxMorphVertexData()
                    {
                        Index = vertex.VertexId,
                        Position = func(vertex.Pos)
                    };
                    morph.MorphArray[i] = vertexMorph;
                }
                ModelData.AddMorph(morph);
            }
            return ModelData.MorphDict[morphName];
        }
    }
}
