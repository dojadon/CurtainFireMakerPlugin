using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class StaticRigidObject
    {
        public AABoundingBox AABB { get; protected set; }
        public Triangle[] Mesh { get; protected set; }

        public List<StaticRigidObject> ChildRigidObjectList { get; } = new List<StaticRigidObject>();

        public StaticRigidObject(Triangle[] mesh, AABoundingBox aabb)
        {
            Mesh = mesh;
            AABB = aabb;
        }
    }

    public struct AABoundingBox
    {
        public Vector3 Pos1 { get; set; }
        public Vector3 Pos2 { get; set; }

        public AABoundingBox(Vector3 pos1, Vector3 pos2)
        {
            Pos1 = pos1;
            Pos2 = pos2;
        }
    }

    public struct Triangle
    {
        public Vector3 Pos1 { get; set; }
        public Vector3 Pos2 { get; set; }
        public Vector3 Pos3 { get; set; }
        public Vector3 Normal { get; set; }

        public Triangle(Vector3 pos1, Vector3 pos2, Vector3 pos3)
        {
            Pos1 = pos1;
            Pos2 = pos2;
            Pos3 = pos3;
            Normal = +((Pos3 - Pos1) ^ (Pos2 - Pos1));
        }
    }
}
