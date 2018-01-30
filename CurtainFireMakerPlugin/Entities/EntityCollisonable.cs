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

        private Triangle MeshToCollide { get; set; }
        private float TimeToCollide { get; set; }

        private bool ShouldUpdateTimeToCollide { get; set; } = true;

        public EntityCollisonable(World world, Entity parentEntity = null) : base(world, parentEntity)
        {
            TimeToCollide = world.MaxFrame;
        }

        public override void Frame()
        {
            if (IsCollisionable && FrameCount >= Math.Floor(TimeToCollide))
            {
                OnCollided(MeshToCollide, TimeToCollide - (float)Math.Floor(TimeToCollide));
                if (--NumberOfCollide <= 0)
                {
                    IsCollisionable = false;
                }
            }

            base.Frame();

            if (IsCollisionable && ShouldUpdateTimeToCollide)
            {
                UpdateMinTimeToCollideWithObject(World.RigidObjectList);
                ShouldUpdateTimeToCollide = false;
            }
        }

        protected override void Record()
        {
            ShouldUpdateTimeToCollide = true;
        }

        public virtual void OnCollided(Triangle tri, float time) { }

        private void UpdateMinTimeToCollideWithObject(IEnumerable<StaticRigidObject> rigidObjectList)
        {
            foreach (var rigidObject in rigidObjectList.Where(c => IsIntersectWithAABB(c.AABB)))
            {
                UpdateMinTimeToCollideWithObject(rigidObject);
            }
        }

        private void UpdateMinTimeToCollideWithObject(StaticRigidObject rigidObject)
        {
            foreach (var tri in rigidObject.Mesh)
            {
                float time = CalculateTimeToIntersectWithPlane(tri.Pos1, tri.Normal);
                if (0 <= time && time + FrameCount < TimeToCollide && IsIntersectWithTriangle(tri))
                {
                    TimeToCollide = time + FrameCount;
                    MeshToCollide = tri;
                }
            }
            UpdateMinTimeToCollideWithObject(rigidObject.ChildRigidObjectList);
        }

        protected virtual bool IsIntersectWithAABB(AABoundingBox box)
        {
            float max = float.MaxValue;
            float min = float.MinValue;

            for (int i = 0; i < 3; i++)
            {
                float v = Velocity[i] != 0 ? Velocity[i] : 1E-7F;
                float t1 = (box.Pos1[i] - Pos[i]) / v;
                float t2 = (box.Pos2[i] - Pos[i]) / v;

                float near = Math.Min(t1, t2);
                float far = Math.Max(t1, t2);

                min = Math.Max(min, near);
                max = Math.Min(max, far);

                if (min > max) return false;
            }
            return true;
        }

        protected virtual bool IsIntersectWithTriangle(Triangle tri)
        {
            var cross1 = (tri.Pos2 - tri.Pos1) ^ (Pos - tri.Pos1);
            var cross2 = (tri.Pos3 - tri.Pos2) ^ (Pos - tri.Pos2);
            var cross3 = (tri.Pos1 - tri.Pos3) ^ (Pos - tri.Pos3);

            var dot1 = cross1 * Velocity;
            var dot2 = cross2 * Velocity;
            var dot3 = cross3 * Velocity;

            return !(dot1 > 0 ^ dot2 > 0) && !(dot2 > 0 ^ dot3 > 0);
        }

        protected virtual float CalculateTimeToIntersectWithPlane(Vector3 planePos, Vector3 normal)
        {
            float dot = normal * Velocity;

            if (Math.Abs(dot) <= Epsilon) return float.MaxValue;

            return normal * (planePos - Pos) / dot;
        }
    }
}
