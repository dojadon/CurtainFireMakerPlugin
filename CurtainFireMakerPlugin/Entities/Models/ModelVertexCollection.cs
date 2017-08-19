using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;

namespace CurtainFireMakerPlugin.Entities.Models
{
    public class ModelVertexCollection
    {
        public List<PmxVertexData> VertexList { get; } = new List<PmxVertexData>();
        public List<int> IndexList { get; } = new List<int>();

        public World World { get; }

        public ModelVertexCollection(World world)
        {
            World = world;
        }

        public void SetupVertices(PmxVertexData[] vertices, int[] indices, int boneCount)
        {
            IndexList.AddRange(Array.ConvertAll(indices, i => i + VertexList.Count));

            foreach (var vertex in vertices)
            {
                vertex.VertexId = VertexList.Count;
                vertex.BoneId = Array.ConvertAll(vertex.BoneId, i => i + boneCount);
            }
            VertexList.AddRange(vertices);
        }
    }
}
