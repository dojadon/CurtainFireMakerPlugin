using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsMmdDataIO.Pmx;

namespace CurtainFireMakerPlugin.Entities
{
    public class ModelVertexCollection
    {
        public List<PmxVertexData> VertexList { get; } = new List<PmxVertexData>();
        public List<int> Indices { get; } = new List<int>();

        public World World { get; }

        public ModelVertexCollection(World world)
        {
            World = world;
        }

        public void SetupVertices(PmxVertexData[] vertices, IEnumerable<int> indices, int boneCount)
        {
            Indices.AddRange(indices.Select(i => i + VertexList.Count));
            foreach (var vertex in vertices)
            {
                vertex.VertexId = VertexList.Count;
                for (int i = 0; i < vertex.BoneId.Length; i++) vertex.BoneId[i] += boneCount;
                VertexList.Add(vertex);
            }
        }
    }
}
