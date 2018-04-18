using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class RigidObject
    {
        public AABoundingBox AABB { get; protected set; }
        public Triangle[] Mesh { get; protected set; }

        public List<RigidObject> ChildRigidObjectList { get; } = new List<RigidObject>();

        public RigidObject(Triangle[] mesh, AABoundingBox aabb)
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

        public bool IsIntersect(Vector3 pos, Vector3 vec)
        {
            float max = float.MaxValue;
            float min = float.MinValue;

            for (int i = 0; i < 3; i++)
            {
                float v = vec[i] != 0 ? vec[i] : 1E-7F;
                float t1 = (Pos1[i] - pos[i]) / v;
                float t2 = (Pos2[i] - pos[i]) / v;

                float near = Math.Min(t1, t2);
                float far = Math.Max(t1, t2);

                min = Math.Max(min, near);
                max = Math.Min(max, far);

                if (min > max) return false;
            }
            return true;
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

        public bool IsIntersect(Vector3 pos, Vector3 vec)
        {
            var cross1 = (Pos2 - Pos1) ^ (pos - Pos1);
            var cross2 = (Pos3 - Pos2) ^ (pos - Pos2);
            var cross3 = (Pos1 - Pos3) ^ (pos - Pos3);

            var dot1 = cross1 * vec;
            var dot2 = cross2 * vec;
            var dot3 = cross3 * vec;

            return !(dot1 > 0 ^ dot2 > 0) && !(dot2 > 0 ^ dot3 > 0);
        }

        public float CalculateTimeToIntersectWithPlane(Vector3 pos, Vector3 vec)
        {
            float dot = Normal * vec;

            if (Math.Abs(dot) <= 1E-6) return 1E+6F;

            return Normal * (Pos1 - pos) / dot;
        }
    }
}
