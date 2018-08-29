using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;
using VecMath.Geometry;

namespace CurtainFireMakerPlugin.Entities
{
    public class RigidNode
    {
        public AABoundingBox BoundingVolume { get; protected set; }
        public List<IGeometry> GeometryShapes { get; } = new List<IGeometry>();

        public List<RigidNode> ChildList { get; } = new List<RigidNode>();

        public int DepthLevel { get; private set; } = 0;

        public RigidNode(AABoundingBox bounding)
        {
            BoundingVolume = bounding;
        }

        public void AddChild(RigidNode child)
        {
            ChildList.Add(child);
            child.DepthLevel = DepthLevel + 1;
        }

        public (float, IGeometry) CalcTimeToCollide(Vector3 pos, Vector3 velocity, float min = 1E+6F)
        {
            IGeometry geo = new NoneGeometry();

            UpdateMinTimeToCollide(pos, velocity, ref min, ref geo);

            return (min, geo);
        }

        public void UpdateMinTimeToCollide(Vector3 pos, Vector3 velocity, ref float collideTime, ref IGeometry collideShape)
        {
            if (!BoundingVolume.CalculateTimeToIntersect(pos, velocity, out float min, out float max) || min > collideTime) return;

            foreach (var shape in GeometryShapes)
            {
                float time = shape.CalculateTimeToIntersectWithRay(pos, velocity);
                if (0 <= time && time < collideTime && shape.IsIntersectWithRay(pos, velocity))
                {
                    collideTime = time;
                    collideShape = shape;
                }
            }

            foreach (var node in ChildList)
            {
                node.UpdateMinTimeToCollide(pos, velocity, ref collideTime, ref collideShape);
            }
        }
    }
}
