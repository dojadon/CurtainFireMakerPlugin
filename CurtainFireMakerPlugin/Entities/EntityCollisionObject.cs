using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityCollisionObject : Entity
    {
        public Vector3[] Vertices { get; set; }
        public int[] VertexIndices { get; set; }

        public override int FramePriority => -1;

        public EntityCollisionObject(World world) : base(world) { }

        public override void Frame()
        {
            base.Frame();

            var worldMat = WorldMat;
            var skinnedVertices = Vertices.Select(v => (Vector3)((Vector4)v * worldMat)).ToArray();

            for (int i = 0; i < VertexIndices.Length; i += 3)
            {
                var pos1 = skinnedVertices[VertexIndices[i + 0]];
                var pos2 = skinnedVertices[VertexIndices[i + 1]];
                var pos3 = skinnedVertices[VertexIndices[i + 2]];

                var mesh = new MeshTriangle(pos1, pos2, pos3);

                foreach (var entity in World.EntityList.Where(e => e.IsCollisionable && e.IsCollided(mesh)))
                {
                    entity.OnCollided();
                }
            }
        }
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
            Normal = Pos1 ^ Pos2;
        }

        public MeshTriangle(MeshTriangle mesh) : this(mesh.Pos1, mesh.Pos1, mesh.Pos3)
        {
        }

        public static MeshTriangle Transform(MeshTriangle mesh, Func<Vector3, Vector3> transform)
        {
            return new MeshTriangle(transform(mesh.Pos1), transform(mesh.Pos2), transform(mesh.Pos3));
        }
    }
}
