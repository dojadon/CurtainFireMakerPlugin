using System;
using System.Linq;
using System.Collections.Generic;
using VecMath;
using VecMath.Geometry;

namespace CurtainFireMakerPlugin.Entities
{
    public abstract class EntityCollisonable : EntityShootable
    {
        protected abstract bool IsCollisionable { get; set; }

        public int NumberOfCollide { get; set; } = 1;

        private Triangle TriagnleToCollide { get; set; }
        private float TimeToCollide { get; set; }

        private bool ShouldUpdateTimeToCollide { get; set; } = true;

        public EntityCollisonable(World world, string typeName, int color, EntityShotBase parentEntity = null)
        : this(world, typeName, color, Matrix4.Identity, parentEntity) { }

        public EntityCollisonable(World world, string typeName, int color, float scale, EntityShotBase parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityCollisonable(World world, string typeName, int color, Vector3 scale, EntityShotBase parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityCollisonable(World world, string typeName, int color, Matrix3 scale, EntityShotBase parentEntity = null)
        : this(world, typeName, color, (Matrix4)scale, parentEntity) { }

        public EntityCollisonable(World world, string typeName, int color, Matrix4 scale, EntityShotBase parentEntity = null)
        : this(world, new ShotProperty(world.ShotTypeProvider.GetShotType(typeName), color, scale), parentEntity) { }

        public EntityCollisonable(World world, ShotProperty property, EntityShotBase parentEntity = null) : base(world, property, parentEntity)
        {
            TimeToCollide = 1E+5F;
        }

        public override void Frame()
        {
            if (IsCollisionable && FrameCount >= Math.Floor(TimeToCollide))
            {
                OnCollided(TriagnleToCollide.Normal, TimeToCollide - (float)Math.Floor(TimeToCollide));
                if (--NumberOfCollide <= 0)
                {
                    IsCollisionable = false;
                }
            }

            base.Frame();

            if (IsCollisionable && ShouldUpdateTimeToCollide)
            {
                TimeToCollide = 1E+5F;
                UpdateMinTimeToCollide(World.RootRigid);
                ShouldUpdateTimeToCollide = false;
            }
        }

        protected override void Record()
        {
            ShouldUpdateTimeToCollide = true;
        }

        public abstract void OnCollided(Vector3 normal, float time);

        private void UpdateMinTimeToCollide(RigidNode rigidObject)
        {
            if (!rigidObject.BoundingVolume.IsIntersect(Pos, Velocity)) return;

            foreach (var tri in rigidObject.Mesh)
            {
                float time = tri.CalculateTimeToIntersect(Pos, Velocity);
                if (0 <= time && time + FrameCount < TimeToCollide && tri.IsIntersect(Pos, Velocity))
                {
                    TimeToCollide = time + FrameCount;
                    TriagnleToCollide = tri;
                }
            }
            rigidObject.ChildList.ForEach(UpdateMinTimeToCollide);
        }
    }
}
