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
        public MeshTriangle[] Meshes { get; protected set; }
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

        public AABoundingBox Transform(Matrix4 mat)
        {
            return new AABoundingBox((Vector4)Pos1 * mat, (Vector4)Pos2 * mat);
        }

        public static AABoundingBox operator *(AABoundingBox box, Matrix4 m) => box.Transform(m);
    }

    public struct MeshTriangle
    {
        public Vector3 Pos1 { get; set; }
        public Vector3 Pos2 { get; set; }
        public Vector3 Pos3 { get; set; }
        public Vector3 Normal { get; set; }

        public MeshTriangle(Vector3 pos1, Vector3 pos2, Vector3 pos3)
        {
            Pos1 = pos1;
            Pos2 = pos2;
            Pos3 = pos3;
            Normal = (Pos3 - Pos1) ^ (Pos2 - Pos1);
        }

        public MeshTriangle(MeshTriangle mesh) : this(mesh.Pos1, mesh.Pos1, mesh.Pos3)
        {
        }

        public MeshTriangle Transform(Matrix4 mat)
        {
            return new MeshTriangle((Vector4)Pos1 * mat, (Vector4)Pos2 * mat, (Vector4)Pos3 * mat);
        }

        public static MeshTriangle operator *(MeshTriangle mesh, Matrix4 m) => mesh.Transform(m);
    }
}
