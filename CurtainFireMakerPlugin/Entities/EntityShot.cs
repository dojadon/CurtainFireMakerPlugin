using System;
using VecMath;
using CsPmx.Data;
using CsVmd.Data;
using CurtainFireMakerPlugin.Mathematics;

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
                if ((this.parentEntity = value) is EntityShot entity)
                {
                    RootBone.ParentId = entity.RootBone.BoneId;
                }
            }
        }

        public ShotModelData ModelData { get; }
        public PmxBoneData RootBone => ModelData.Bones[0];
        public PmxMorphData MaterialMorph => ModelData.MaterialMorph;

        private bool ShouldRecord
        {
            get => RecordWhenVelocityChanges ? IsUpdatedVelocity : IsUpdatedPos;
            set => IsUpdatedVelocity = IsUpdatedPos = value;
        }
        private bool IsUpdatedVelocity { get; set; }
        private bool IsUpdatedPos { get; set; }

        private static float Epsilon { get; set; } = 0.00001F;

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
            set => IsUpdatedPos |= !Vector3.EpsilonEquals(base.Pos, (base.Pos = value), Epsilon);
        }

        public override Quaternion Rot
        {
            get => base.Rot;
            set => IsUpdatedPos |= !Quaternion.EpsilonEquals(base.Rot, (base.Rot = value), Epsilon);
        }

        private bool recordWhenVelocityChanges = true;
        public bool RecordWhenVelocityChanges
        {
            get => recordWhenVelocityChanges;
            set
            {
                recordWhenVelocityChanges = value;
                IsUpdatedVelocity = IsUpdatedPos = IsUpdatedVelocity | IsUpdatedPos;
            }
        }

        public event EntityEventHandler<EntityShot, RecordEventArgs> RecordEvent;
        public event EntityEventHandler<EntityShot, InitModelDataEventArgs> InitModelDataEvent;

        protected virtual void OnReocrd() => RecordEvent?.Invoke(this, new RecordEventArgs(IsUpdatedVelocity, IsUpdatedPos));
        protected virtual void OnInitModelData() => InitModelDataEvent?.Invoke(this, new InitModelDataEventArgs(ModelData));

        public EntityShot(World world, string typeName, int color) : this(world, new ShotProperty(typeName, color)) { }

        public EntityShot(World world, ShotProperty property) : base(world)
        {
            try
            {
                Property = property;

                ModelData = World.AddShot(this);
                ModelData.EntityList.Add(this);

                Property.Type.Init(this);
                Property.Type.InitModelData(ModelData);

                RecordEvent += (sender, e) => AddVmdMotion();
            }
            catch (Exception e)
            {
                try { Console.WriteLine(Plugin.Instance.PythonRunner.FormatException(e)); } catch { }
                Console.WriteLine(e);
            }
        }

        internal override void Frame()
        {
            if (FrameCount == 1 || ShouldRecord)
            {
                OnReocrd();
            }
            ShouldRecord = false;

            base.Frame();
        }

        protected override void UpdateRot()
        {
            if (RecordWhenVelocityChanges)
            {
                Rot = Matrix4.LookAt(+Velocity, +Upward);
            }
            base.UpdateRot();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            if (ModelData.EntityList.Count == 1)
            {
                OnInitModelData();
            }

            AddVmdMorph(-World.FrameCount, 1.0F, MaterialMorph);
            AddVmdMorph(0, 1.0F, MaterialMorph);
            AddVmdMorph(1, 0.0F, MaterialMorph);

            AddVmdMotion();
        }

        public override void OnDeath()
        {
            base.OnDeath();

            AddVmdMorph(-1, 0.0F, MaterialMorph);
            AddVmdMorph(0, 1.0F, MaterialMorph);

            AddVmdMotion();
        }

        public override void SetMotionBezier(Vector2 pos1, Vector2 pos2, int length)
        {
            AddVmdMotion();

            base.SetMotionBezier(pos1, pos2, length);
        }

        internal override void RemoveMotionBezier()
        {
            AddVmdMotion();

            base.RemoveMotionBezier();
        }

        public void AddVmdMotion()
        {
            UpdateRot();

            var bezier = CubicBezierCurve.Line;

            if (MotionInterpolation != null && MotionInterpolation.StartFrame < World.FrameCount)
            {
                bezier = MotionInterpolation.Curve;
            }
            AddVmdMotion(RootBone, Pos, Rot, bezier);
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
                InterpolatePointZ = interpolation,
                InterpolatePointR = interpolation
            };

            World.VmdMotion.AddVmdMotion(motion);
        }

        public void AddVmdMorph(int frameOffset, float rate, PmxMorphData morph, bool replace = false)
        {
            if (Property.Type.HasMmdData)
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
                        Position = (DxMath.Vector3)func(vertex.Pos)
                    };
                    morph.MorphArray[i] = vertexMorph;
                }
                ModelData.AddMorph(morph);
            }
            return ModelData.MorphDict[morphName];
        }
    }

    public class RecordEventArgs : EventArgs
    {
        public bool IsUpdatedVelocity { get; }
        public bool IsUpdatedPos { get; }

        public RecordEventArgs(bool isUpdatedVelocity, bool isUpdatedPos)
        {
            IsUpdatedVelocity = isUpdatedVelocity;
            IsUpdatedPos = isUpdatedPos;
        }
    }

    public class InitModelDataEventArgs : EventArgs
    {
        public ShotModelData ModelData { get; }

        public InitModelDataEventArgs(ShotModelData data)
        {
            ModelData = data;
        }
    }
}
