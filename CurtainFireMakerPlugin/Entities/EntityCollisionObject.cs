using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityCollisionObject : Entity
    {
        public List<MeshTriangle> Meshes { get; } = new List<MeshTriangle>();
        public List<MeshTriangle> SkinnedMeshes { get; set; } = new List<MeshTriangle>();

        public EntityCollisionObject(World world) : base(world) { }

        public override void Frame()
        { 
            base.Frame();

            var worldMat = WorldMat;
            SkinnedMeshes = Meshes.Select(m => MeshTriangle.Transform(m, v => (Vector4)v * worldMat)).ToList();
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
