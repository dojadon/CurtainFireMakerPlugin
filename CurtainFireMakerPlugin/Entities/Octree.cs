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
        private bool IsDivided => ChildNodes.Length > 0;

        public List<Entity> Entities { get; } = new List<Entity>();

        public OctantNode(AABoundingBox aabb, int max)
        {
            AABB = aabb;
            MaxCount = max;
        }

        public bool AddEntity(Entity entity)
        {
            if (!IsDivided)
            {
                if (Entities.Count < MaxCount)
                {
                    Entities.Add(entity);
                    return true;
                }

                ChildNodes = DivideAABB().Select(b => new OctantNode(b, MaxCount)).ToArray();
                Entities.ForEach(e => AddEntity(e));
                Entities.Clear();
            }

            return ChildNodes.Any(n => n.AABB.IsIntersectWithPoint(entity.Pos) && n.AddEntity(entity));
        }

        private IEnumerable<AABoundingBox> DivideAABB()
        {
            float GetEmelent(int idx, int max) => max != 0 ? AABB.PosMax[idx] : AABB.PosMin[idx];
            Vector3 center = (AABB.PosMin + AABB.PosMax) * 0.5F;
            return Enumerable.Range(0, 8).Select(i => new Vector3(GetEmelent(0, i & 1), GetEmelent(1, i & 2), GetEmelent(2, i & 4))).Select(v => new AABoundingBox(center, v));
        }

        public void CalcMinTimeToCollideWithEntity(Vector3 pos, Vector3 velocity, ref float min)
        {
            if (IsDivided)
            {
                foreach (var node in ChildNodes.Where(n => n.AABB.IsIntersectWithRay(pos, velocity)))
                {
                    node.CalcMinTimeToCollideWithEntity(pos, velocity, ref min);
                }
            }
            else
            {
                foreach (var entity in Entities.OrderBy(e => (e.Pos - pos).LengthSquare()))
                {
                    if (min != (min = Math.Min(min, entity.Sphere.CalculateTimeToIntersectWithRay(pos, velocity)))) return;
                }
            }
        }
    }
}
