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
        public Triangle[] Mesh { get; protected set; }

        public List<RigidNode> ChildList { get; } = new List<RigidNode>();

        public int DepthLevel { get; private set; } = 0;

        public RigidNode(Triangle[] mesh, AABoundingBox bounding)
        {
            Mesh = mesh;
            BoundingVolume = bounding;
        }

        public void AddChild(RigidNode child)
        {
            ChildList.Add(child);
            child.DepthLevel = DepthLevel + 1;
        }
    }
}
