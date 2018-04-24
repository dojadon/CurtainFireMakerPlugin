using System;
using System.Linq;
using System.Collections.Generic;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public abstract class EntityCollisonable : EntityShootable
    {
        protected abstract bool IsCollisionable { get; set; }

        public int NumberOfCollide { get; set; } = 1;

        private Triangle TriagnleToCollide { get; set; }
        private float TimeToCollide { get; set; }

        private bool ShouldUpdateTimeToCollide { get; set; } = true;

        public EntityCollisonable(World world, string typeName, int color, EntityShot parentEntity = null)
        : this(world, typeName, color, Matrix4.Identity, parentEntity) { }

        public EntityCollisonable(World world, string typeName, int color, float scale, EntityShot parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityCollisonable(World world, string typeName, int color, Vector3 scale, EntityShot parentEntity = null)
        : this(world, typeName, color, new Matrix3(scale), parentEntity) { }

        public EntityCollisonable(World world, string typeName, int color, Matrix3 scale, EntityShot parentEntity = null)
        : this(world, typeName, color, (Matrix4)scale, parentEntity) { }

        public EntityCollisonable(World world, string typeName, int color, Matrix4 scale, EntityShot parentEntity = null)
        : this(world, new ShotProperty(world.ShotTypeProvider.GetShotType(typeName), color, scale), parentEntity) { }

        public EntityCollisonable(World world, ShotProperty property, EntityShot parentEntity = null) : base(world, property, parentEntity)
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
                World.RigidObjectList.ForEach(UpdateMinTimeToCollide);
                ShouldUpdateTimeToCollide = false;
            }
        }

        protected override void Record()
        {
            ShouldUpdateTimeToCollide = true;
        }

        public abstract void OnCollided(Vector3 normal, float time);

        private void UpdateMinTimeToCollide(RigidObject rigidObject)
        {
            if (!rigidObject.AABB.IsIntersect(Pos, Velocity)) return;

            foreach (var tri in rigidObject.Mesh)
            {
                float time = tri.CalculateTimeToIntersectWithPlane(Pos, Velocity);
                if (0 <= time && time + FrameCount < TimeToCollide && tri.IsIntersect(Pos, Velocity))
                {
                    TimeToCollide = time + FrameCount;
                    TriagnleToCollide = tri;
                }
            }
            rigidObject.ChildRigidObjectList.ForEach(UpdateMinTimeToCollide);
        }
    }
}
