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

        public IEnumerable<(Entity e, float)> TimeToCollide(Vector3 pos, Vector3 velocity, float exRange = 0)
        {
            return ChildNodes.Where(n => n.AABB.IsIntersectWithRay(pos, velocity)).SelectMany(n => n.TimeToCollide(pos, velocity, exRange))
            .Concat(Entities.Select(e => (e, e.GetExpandSphere(exRange).CalculateTimeToIntersectWithRay(pos, velocity))));
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

    public class KdNode
    {
        public KdNode Child1 { get; }
        public KdNode Child2 { get; }

        public bool IsLeafNode => Child1 == null;

        public Entity Entity { get; }

        public int Level { get; }
        public int Axis => Level % 3;

        public KdNode(IEnumerable<Entity> entities, int level = 0)
        {
            try
            {
                var array = entities.OrderBy(e => e.Pos[level % 3]).ToArray();

                if (array.Length > 1)
                {
                    int mid = array.Length / 2;

                    Entity = array[mid];
                    Child1 = new KdNode(array.Take(mid), level + 1);

                    if (mid + 1 < array.Length)
                    {
                        Child2 = new KdNode(array.Skip(mid + 1), level + 1);
                    }
                }
                else
                {
                    Entity = array[0];
                }
            }
            catch (Exception e)
            {
                try { Console.WriteLine(entities.First().World.Executor.FormatException(e)); } catch { }
                Console.WriteLine(e);
            }
        }

        public float Distance(Vector3 pos) => (Entity.Pos - pos).LengthSquare();

        public float DistanceVertical(Vector3 pos) => (float)Math.Pow(Entity.Pos[Axis] - pos[Axis], 2);

        public KdNode GetNode(Vector3 pos)
        {
            return pos[Axis] < Entity.Pos[Axis] ? Child1 : Child2;
        }

        public KdNode GetLeafNode(Vector3 pos)
        {
            return IsLeafNode ? this : (GetNode(pos) ?? Child1).GetLeafNode(pos);
        }

        public Entity Nearest(Vector3 pos, int maxDepth = 128)
        {
            var node = GetLeafNode(pos);
            return (Nearest(pos, node.Distance(pos), 0, maxDepth) ?? node).Entity;
        }

        public KdNode Nearest(Vector3 pos, float min, int depth, int maxDepth)
        {
            KdNode result = null;

            if (depth > maxDepth) return result;

            var dis = Distance(pos);
            if (dis < min)
            {
                min = dis;
                result = this;
            }

            if (IsLeafNode) return result;

            dis = DistanceVertical(pos);

            if (dis < min)
            {
                var node1 = Child1?.Nearest(pos, min, depth + 1, maxDepth);
                var node2 = Child2?.Nearest(pos, min, depth + 1, maxDepth);

                return node1 == null && node2 == null ? result : (node1 ?? node2);
            }
            else
            {
                var node = GetNode(pos);
                return node?.Nearest(pos, min, depth + 1, maxDepth) ?? result;
            }
        }
    }
}
