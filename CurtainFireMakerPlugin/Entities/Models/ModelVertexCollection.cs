using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsMmdDataIO.Pmx;

namespace CurtainFireMakerPlugin.Entities.Models
{
    public class ModelVertexCollection
    {
        public List<PmxVertexData> VertexList { get; } = new List<PmxVertexData>();
        public List<int> IndexList { get; } = new List<int>();
        public List<List<int>> IndexOfEachMaterialList { get; } = new List<List<int>>();

        public World World { get; }

        public ModelVertexCollection(World world)
        {
            World = world;
        }

        public void SetupVertices(PmxVertexData[] vertices, IEnumerable<int> indices, IEnumerable<int> faceCount, int boneCount)
        {
            List<int> convertedIndices = (from index in indices select index + VertexList.Count).ToList();
            IndexList.AddRange(convertedIndices);

            int total = 0;
            foreach (int count in faceCount)
            {
                IndexOfEachMaterialList.Add(convertedIndices.GetRange(total, count));
                total += count;
            }

            foreach (var vertex in vertices)
            {
                vertex.VertexId = VertexList.Count;
                for (int i = 0; i < vertex.BoneId.Length; i++) vertex.BoneId[i] += boneCount;
                VertexList.Add(vertex);
            }
        }
    }
}
