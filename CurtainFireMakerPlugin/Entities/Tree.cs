using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VecMath;
using VecMath.Geometry;

namespace CurtainFireMakerPlugin.Entities
{
    public class OctantNode
    {
        public AABoundingBox AABB { get; }
        public int MaxCount { get; }

        public OctantNode[] ChildNodes { get; private set; } = { };
        public bool HasChild => ChildNodes.Length > 0;

        public List<Entity> Entities { get; } = new List<Entity>();
        public bool HasEntity => Entities.Count > 0;

        public int Level { get; }

        public OctantNode(AABoundingBox aabb, int max, int level = 0)
        {
            AABB = aabb;
            MaxCount = max;
            Level = level;
        }

        public void AddEntity(Entity entity)
        {
            if (HasChild)
            {
                ChildNodes.First(n => n.AABB.IsIntersectWithPoint(entity.Pos)).AddEntity(entity);
                return;
            }

            if (Entities.Count < MaxCount)
            {
                Entities.Add(entity);
            }
            else
            {
                ChildNodes = DivideAABB().Select(b => new OctantNode(b, MaxCount, Level + 1)).ToArray();
                Entities.ForEach(AddEntity);
                Entities.Clear();
            }
        }

        public bool RemoveEntity(Entity entity)
        {
            if (HasChild && ChildNodes.Where(n => n.AABB.IsIntersectWithPoint(entity.Pos)).Any(n => n.RemoveEntity(entity)))
            {
                var entities = GetEntities().ToList();

                if (entities.Count <= MaxCount)
                {
                    Entities.AddRange(entities);
                    ChildNodes = new OctantNode[0];
                }

                return true;
            }
            else
            {
                return Entities.Remove(entity);
            }
        }

        private IEnumerable<AABoundingBox> DivideAABB()
        {
            float GetEmelent(int idx, int max) => max != 0 ? AABB.PosMax[idx] : AABB.PosMin[idx];
            Vector3 center = (AABB.PosMin + AABB.PosMax) * 0.5F;
            return Enumerable.Range(0, 8).Select(i => new Vector3(GetEmelent(0, i & 1), GetEmelent(1, i & 2), GetEmelent(2, i & 4))).Select(v => new AABoundingBox(center, v));
        }

        public IEnumerable<(Entity e, float)> TimeToCollide(Vector3 pos, Vector3 velocity, float range = 0)
        {
            return ChildNodes.Where(n => n.AABB.IsIntersectWithRay(pos, velocity)).SelectMany(n => n.TimeToCollide(pos, velocity, range))
            .Concat(Entities.Select(e => (e, new Sphere(e.Pos, range).CalculateTimeToIntersectWithRay(pos, velocity))));
        }

        public (Entity, float) MinTimeToCollide(Vector3 pos, Vector3 velocity, float exRange = 0)
        {
            return TimeToCollide(pos, velocity, exRange).DefaultIfEmpty((null, 1E+6F)).OrderBy(t => t.Item2).First();
        }

        public Entity Nearest(Vector3 pos)
        {
            return GetEntities().OrderBy(e => (e.Pos - pos).LengthSquare()).DefaultIfEmpty(null).First();
        }

        public IEnumerable<Entity> GetEntities()
        {
            return Entities.Concat(ChildNodes.SelectMany(n => n.GetEntities()));
        }
    }
}
