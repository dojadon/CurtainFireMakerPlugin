using System;
using System.Collections.Generic;
using System.Linq;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public abstract class EntityCollisonable : EntityShootable
    {
        protected abstract bool IsCollisionable { get; set; }

        private float TimeToCollide { get; set; }

        private bool ShouldUpdateTimeToCollide { get; set; } = true;

        private static float Epsilon { get; set; } = 0.001F;

        public EntityCollisonable(World world, Entity parentEntity = null) : base(world, parentEntity)
        {
            TimeToCollide = world.MaxFrame;
        }

        protected override void OnVelocityUpdated()
        {
            base.OnVelocityUpdated();

            ShouldUpdateTimeToCollide = true;
        }

        public override void Frame()
        {
            if (IsCollisionable && FrameCount >= Math.Floor(TimeToCollide))
            {
                OnCollided(TimeToCollide - (float)Math.Floor(TimeToCollide));
                IsCollisionable = false;
            }

            base.Frame();

            if (IsCollisionable)
            {
                UpdateMinTimeToCollideWithObject();
            }
        }

        public virtual void OnCollided(float time) { }

        protected void UpdateMinTimeToCollideWithObject()
        {
            UpdateMinTimeToCollideWithObject(World.CollisonObjectList.Where(c => ShouldUpdateTimeToCollide || (c.IsUpdatedLocalMat)));
            ShouldUpdateTimeToCollide = false;
        }

        private void UpdateMinTimeToCollideWithObject(IEnumerable<EntityCollisionObject> updatedCollisionObjects)
        {
            foreach (var collisionObj in updatedCollisionObjects)
            {
                TimeToCollide = Math.Min(TimeToCollide, CalculateMinTimeToCollideWithObject(collisionObj, TimeToCollide));
            }
        }

        private float CalculateMinTimeToCollideWithObject(EntityCollisionObject collisionObj, float minTime)
        {
            foreach (var tri in collisionObj.Meshes)
            {
                float time = CalcTimeToCollideWithPlane(tri.Pos1, tri.Normal);

                if (0 <= time && time + FrameCount < minTime && IsCollidedWithTriangle(tri, Pos + Velocity * time))
                {
                    minTime = time + FrameCount;
                }
            }
            return minTime;
        }

        protected virtual bool IsCollidedWithAABB(AABoundingBox box)
        {
            Vector3 min = new Vector3(Math.Min(box.Pos1.x, box.Pos2.x), Math.Min(box.Pos1.y, box.Pos2.y), Math.Min(box.Pos1.z, box.Pos2.z));
            Vector3 max = new Vector3(Math.Max(box.Pos1.x, box.Pos2.x), Math.Max(box.Pos1.y, box.Pos2.y), Math.Max(box.Pos1.z, box.Pos2.z));

            return min <= Pos && Pos <= max;
        }

        protected virtual bool IsCollidedWithTriangle(MeshTriangle tri, Vector3 pos)
        {
            var cross1 = (tri.Pos2 - tri.Pos1) ^ (Pos - tri.Pos1);
            var cross2 = (tri.Pos3 - tri.Pos2) ^ (Pos - tri.Pos2);
            var cross3 = (tri.Pos1 - tri.Pos3) ^ (Pos - tri.Pos3);

            var dot1 = cross1 * Velocity;
            var dot2 = cross2 * Velocity;
            var dot3 = cross3 * Velocity;

            return !(dot1 > 0 ^ dot2 > 0) && !(dot2 > 0 ^ dot3 > 0);
        }

        protected virtual float CalcTimeToCollideWithPlane(Vector3 planePos, Vector3 normal)
        {
            float dot = normal * Velocity;

            if (Math.Abs(dot) <= Epsilon) return float.MaxValue;

            return normal * (planePos - Pos) / dot;
        }
    }
}
