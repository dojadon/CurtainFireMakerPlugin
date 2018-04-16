using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecMath;
using MMDataIO.Pmx;
using MMDataIO.Vmd;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityShotBase : Entity
    {
        public ShotProperty Property { get; }

        public ShotModelData ModelData { get; }
        public PmxBoneData RootBone => ModelData.Bones[0];

        public virtual bool IsReusable => IsRemoved;

        public EntityShotBase(World world, string typeName, int color, EntityShot parentEntity = null)
            : this(world, typeName, color, Matrix4.Identity, parentEntity) { }

        public EntityShotBase(World world, string typeName, int color, float scale, EntityShot parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityShotBase(World world, string typeName, int color, Vector3 scale, EntityShot parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityShotBase(World world, string typeName, int color, Matrix3 scale, EntityShot parentEntity = null)
        : this(world, typeName, color, (Matrix4)scale, parentEntity) { }

        public EntityShotBase(World world, string typeName, int color, Matrix4 scale, EntityShot parentEntity = null)
        : this(world, new ShotProperty(world.ShotTypeProvider.GetShotType(typeName), color, scale), parentEntity) { }

        public EntityShotBase(World world, ShotProperty property, EntityShot parentEntity = null) : base(world, parentEntity)
        {
            try
            {
                Property = property;

                ModelData = World.AddShot(this);

                RootBone.ParentId = ParentEntity is EntityShot entity ? entity.RootBone.BoneId : RootBone.ParentId;

                Property.Type.InitEntity(this);
            }
            catch (Exception e)
            {
                try { Console.WriteLine(World.Executor.FormatException(e)); } catch { }
                Console.WriteLine(e);
            }
        }

        public void AddBoneKeyFrame(PmxBoneData bone, Vector3 pos, Quaternion rot, CubicBezierCurve curve, int frameOffset = 0, int priority = 0)
        {
            var frame = new VmdMotionFrameData(bone.BoneName, World.FrameCount + frameOffset, pos, rot);
            frame.InterpolationPointX1 = frame.InterpolationPointY1 = frame.InterpolationPointZ1 = curve.P1;
            frame.InterpolationPointX2 = frame.InterpolationPointY2 = frame.InterpolationPointZ2 = curve.P2;
            World.KeyFrames.AddBoneKeyFrame(frame, priority);
        }

        public void AddMorphKeyFrame(PmxMorphData morph, float weight, int frameOffset = 0, int priority = 0)
        {
            var frame = new VmdMorphFrameData(morph.MorphName, World.FrameCount + frameOffset, weight);
            World.KeyFrames.AddMorphKeyFrame(frame, priority);
        }

        public PmxMorphData CreateVertexMorph(Func<Vector3, Vector3> func)
        {
            return ModelData.CreateVertexMorph("V" + EntityId, func);
        }
    }
}
