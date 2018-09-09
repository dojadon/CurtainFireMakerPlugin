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
    public abstract class EntityShotBase : Entity
    {
        public ShotProperty Property { get; }

        public ShotModelData ModelData { get; private set; }
        public PmxBoneData RootBone => ModelData.Bones[0];

        public virtual bool IsReusable => IsRemoved;

        public EntityShotBase(World world, string typeName, int color)
            : this(world, typeName, color, Matrix4.Identity) { }

        public EntityShotBase(World world, string typeName, int color, float scale)
        : this(world, typeName, color, new Matrix3(scale)) { }

        public EntityShotBase(World world, string typeName, int color, Vector3 scale)
        : this(world, typeName, color, new Matrix3(scale)) { }

        public EntityShotBase(World world, string typeName, int color, Matrix3 scale)
        : this(world, typeName, color, (Matrix4)scale) { }

        public EntityShotBase(World world, string typeName, int color, Matrix4 scale)
        : this(world, new ShotProperty(world.ShotTypeProvider.GetShotType(typeName), color, scale)) { }

        public EntityShotBase(World world, ShotProperty property) : base(world)
        {
            try
            {
                Property = property;
                Property.Type.InitEntity(this);
            }
            catch (Exception e)
            {
                try { Console.WriteLine(World.Executor.FormatException(e)); } catch { }
                Console.WriteLine(e);
            }
        }

        public override bool Spawn()
        {
            if (base.Spawn())
            {
                ModelData = World.AddShot(this);
                RootBone.ParentId = Parent is EntityShotBase entity ? entity.RootBone.BoneId : RootBone.ParentId;
                return true;
            }
            return false;
        }

        public void AddBoneKeyFrame(PmxBoneData bone, Vector3 pos, Quaternion rot, CubicBezierCurve curve, int frameOffset = 0, int priority = 0)
        {
            var frame = new VmdMotionFrameData(bone.BoneName, World.FrameCount + frameOffset, pos, rot);
            frame.InterpolationPointX1 = frame.InterpolationPointY1 = frame.InterpolationPointZ1 = curve.P1;
            frame.InterpolationPointX2 = frame.InterpolationPointY2 = frame.InterpolationPointZ2 = curve.P2;
            World.VmdSequence.AddBoneKeyFrame(frame, priority);
        }

        public void AddMorphKeyFrame(PmxMorphData morph, float weight, int frameOffset = 0, int priority = 0)
        {
            var frame = new VmdMorphFrameData(morph.MorphName, World.FrameCount + frameOffset, weight);
            World.VmdSequence.AddMorphKeyFrame(frame, priority);
        }

        public void AddRigid(int id, PmxRigidData rigid)
        {
            rigid.RigidName = EntityId + "_" + id;
            ModelData.AddRigid(id, rigid);
        }

        public PmxMorphData CreateVertexMorph(int id, Func<Vector3, Vector3> func)
        {
            return ModelData.CreateVertexMorph(EntityId + "_" + id, id, func);
        }

        public PmxMorphData CreateUvMorph(int id, MorphType type, Func<Vector3, Vector4> func)
        {
            return ModelData.CreateUVMorph(EntityId + "_" + id, id, type, func);
        }
    }
}
