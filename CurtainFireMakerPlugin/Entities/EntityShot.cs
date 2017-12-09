using System;
using System.Collections.Generic;
using System.Linq;
using CsMmdDataIO.Pmx;
using CsMmdDataIO.Vmd;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShot : Entity
    {
        public ShotProperty Property { get; }

        public ShotModelData ModelData { get; }
        public PmxBoneData RootBone => ModelData.Bones[0];
        public PmxMorphData MaterialMorph => ModelData.MaterialMorph;

        public IRecording Recording { get; set; } = Entities.Recording.Velocity;

        public bool IsUpdatedVelocity { get; private set; } = true;
        public bool IsUpdatedLocalMat { get; private set; } = true;

        private static float Epsilon { get; set; } = 0.00001F;

        public Vector3 LookAtVec { get; set; }

        public override Vector3 Velocity
        {
            get => base.Velocity;
            set
            {
                IsUpdatedVelocity |= !Vector3.EpsilonEquals(base.Velocity, (base.Velocity = value), Epsilon);
                if (Velocity != Vector3.Zero) LookAtVec = Velocity;
            }
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

        private MotionInterpolation MotionInterpolation { get; set; }

        public EntityShot(World world, ShotType type, int color, EntityShot parentEntity = null)
        : this(world, new ShotProperty(type, color), parentEntity) { }

        public EntityShot(World world, ShotType type, int color, short group, EntityShot parentEntity = null)
        : this(world, new ShotProperty(type, color, group), parentEntity) { }

        public EntityShot(World world, ShotProperty property, EntityShot parentEntity = null) : base(world, parentEntity)
        {
            try
            {
                Property = property;

                ModelData = World.AddShot(this);
                ModelData.OwnerEntities.Add(this);

                RootBone.ParentId = ParentEntity is EntityShot entity ? entity.RootBone.BoneId : RootBone.ParentId;

                Property.Type.InitEntity(this);
                Property.Type.InitModelData(ModelData);
            }
            catch (Exception e)
            {
                try { Console.WriteLine(Plugin.Instance.PythonExecutor.FormatException(e)); } catch { }
                Console.WriteLine(e);
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            if (World.FrameCount <= 0)
            {
                AddMorphKeyFrame(MaterialMorph, -World.FrameCount, 0.0F);
            }
            else
            {
                AddMorphKeyFrame(MaterialMorph, -1, 1.0F);
                AddMorphKeyFrame(MaterialMorph, 0, 0.0F);
                AddMorphKeyFrame(MaterialMorph, -World.FrameCount, 1.0F);
            }

            AddBoneKeyFrame();
        }

        public override void OnDeath()
        {
            base.OnDeath();

            AddMorphKeyFrame(MaterialMorph, -1, 0.0F);
            AddMorphKeyFrame(MaterialMorph, 0, 1.0F);

            AddBoneKeyFrame();
        }

        public override void Frame()
        {
            if (Recording.ShouldRecord(this) || World.FrameCount == 0)
            {
                AddBoneKeyFrame();
            }
            IsUpdatedVelocity = IsUpdatedLocalMat = false;

            base.Frame();
        }

        protected override void UpdateLocalMat()
        {
            float interpolation = 1.0F;

            if (MotionInterpolation != null)
            {
                if (MotionInterpolation.Within(World.FrameCount))
                {
                    interpolation = MotionInterpolation.GetChangeAmount(World.FrameCount);

                    if (!MotionInterpolation.Within(World.FrameCount + 1) && MotionInterpolation.IsSync)
                    {
                        Velocity *= interpolation;
                        interpolation = 1.0F;
                    }
                }
                else
                {
                    RemoveMotionInterpolationCurve();
                }
            }
            Pos += Velocity * interpolation;
        }

        public void SetMotionInterpolationCurve(Vector2 pos1, Vector2 pos2, int length, bool isSyncingVelocity = true)
        {
            AddBoneKeyFrame();
            MotionInterpolation = new MotionInterpolation(World.FrameCount, length, pos1, pos2, isSyncingVelocity);
        }

        protected void RemoveMotionInterpolationCurve()
        {
            AddBoneKeyFrame();
            MotionInterpolation = null;
        }

        public void AddBoneKeyFrame()
        {
            var posCurve = CubicBezierCurve.Line;

            if (MotionInterpolation != null && MotionInterpolation.StartFrame < World.FrameCount)
            {
                posCurve = MotionInterpolation.Curve;
            }

            AddBoneKeyFrame(RootBone, Recording.GetRecordedPos(this), Recording.GetRecordedRot(this), posCurve);
        }

        public void AddBoneKeyFrame(PmxBoneData bone, Vector3 pos, Quaternion rot, CubicBezierCurve posCurve)
        {
            var frame = new VmdMotionFrameData(bone.BoneName, World.FrameCount, pos, rot)
            {
                InterpolationPointX1 = posCurve.P1,
                InterpolationPointX2 = posCurve.P2,
                InterpolationPointY1 = posCurve.P1,
                InterpolationPointY2 = posCurve.P2,
                InterpolationPointZ1 = posCurve.P1,
                InterpolationPointZ2 = posCurve.P2,
            };
            World.KeyFrames.AddBoneKeyFrame(bone, frame);
        }

        public void AddMorphKeyFrame(PmxMorphData morph, int frameOffset, float weight)
        {
            if (Property.Type.HasMesh)
            {
                var frame = new VmdMorphFrameData(morph.MorphName, World.FrameCount + frameOffset, weight);
                World.KeyFrames.AddMorphKeyFrame(morph, frame);
            }
        }

        public PmxMorphData CreateVertexMorph(Func<Vector3, Vector3> func)
        {
            string morphName = $"V_{MaterialMorph.MorphName}";

            if (!ModelData.MorphDict.ContainsKey(morphName))
            {
                var vertices = ModelData.Vertices;

                var morph = new PmxMorphData()
                {
                    MorphName = morphName,
                    SlotType = MorphSlotType.RIP,
                    MorphType = MorphType.VERTEX,
                    MorphArray = new IPmxMorphTypeData[vertices.Length]
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

    public struct CubicBezierCurve
    {
        public static readonly CubicBezierCurve Line = new CubicBezierCurve(new Vector2(0, 0), new Vector2(0.5F, 0.5F), new Vector2(0.5F, 0.5F), new Vector2(1, 1));

        public Vector2 P0 { get; }
        public Vector2 P1 { get; }
        public Vector2 P2 { get; }
        public Vector2 P3 { get; }

        public CubicBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        public float[] SolveTimeFromX(float x, float eps = 1.0E-4F)
        {
            float a0 = P0.x - x;
            float a1 = 3 * (-P0.x + P1.x);
            float a2 = 3 * (P0.x - 2 * P1.x + P2.x);
            float a3 = -P0.x + P3.x + 3 * (P1.x - P2.x);

            double[] solution = EquationUtil.SolveCubic(a3, a2, a1, a0);

            return (from d in solution where (0 - eps <= d && d <= 1 + eps) select (float)d).Distinct().ToArray();
        }

        public float X(float t) => GetPosition(t, P0.x, P1.x, P2.x, P3.x);

        public float Y(float t) => GetPosition(t, P0.y, P1.y, P2.y, P3.y);

        public float GetPosition(float t, float p0, float p1, float p2, float p3)
        {
            float inv = 1 - t;
            return inv * inv * inv * p0 + 3 * inv * inv * t * p1 + 3 * inv * t * t * p2 + t * t * t * p3;
        }
    }

    internal class MotionInterpolation
    {
        public CubicBezierCurve Curve { get; }
        public int StartFrame { get; }
        public int EndFrame { get; }
        public int Length { get; }
        public bool IsSync { get; }

        public MotionInterpolation(int startFrame, int length, Vector2 p1, Vector2 p2, bool isSync)
        {
            Curve = new CubicBezierCurve(new Vector2(0, 0), p1, p2, new Vector2(1, 1));

            StartFrame = startFrame;
            Length = length;
            EndFrame = StartFrame + Length;
            IsSync = isSync;
        }

        public bool Within(int frame)
        {
            return StartFrame <= frame && frame < EndFrame;
        }

        public float GetChangeAmount(int frame)
        {
            if (Within(frame))
            {
                float unit = 1.0F / Length;

                float x1 = (frame - StartFrame) * unit;
                float x2 = x1 + unit;

                return (FuncY(x2) - FuncY(x1)) * Length;
            }
            else
            {
                return 1.0F;
            }
        }

        public float FuncY(float x)
        {
            float[] t = Curve.SolveTimeFromX(x);

            if (t.Length == 0)
            {
                throw new ArithmeticException($"ベジエ曲線の解が見つかりません : x[ {x} ]");
            }

            float time = t[0];

            if (t.Length > 1)
            {
                for (int i = 1; i < t.Length; i++)
                {
                    if (Math.Abs(x - time) > Math.Abs(x - t[i])) time = t[i];
                }
            }
            return Curve.Y(time);
        }
    }
}