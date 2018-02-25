using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMDataIO.Pmx;

namespace CurtainFireMakerPlugin.Entities
{
    public class ModelVertexCollection
    {
        public World World { get; }

        public MultiDictionary<ShotProperty, int> BoneCountEachPropertyDict { get; } = new MultiDictionary<ShotProperty, int>();

        public ModelVertexCollection(World world)
        {
            World = world;
        }

        public void CreateVertices(ShotType type, IEnumerable<ShotModelData> dataList, int vertexOffset, out PmxVertexData[] vertices, out int[] indices)
        {
            var indicesEachMaterial = type.OriginalData.MaterialArray.Select(m => new List<int>()).ToArray();
            var vertexList = new List<PmxVertexData>();

            foreach (var data in dataList)
            {
                for (int i = 0, totalFaceCount = 0; i < data.Property.Type.OriginalData.MaterialArray.Length; i++)
                {
                    int faceCount = data.Property.Type.OriginalData.MaterialArray[i].FaceCount;
                    indicesEachMaterial[i].AddRange(data.Property.Type.OriginalData.VertexIndices.Skip(totalFaceCount).Take(faceCount).Select(idx => idx + vertexOffset + vertexList.Count));
                    totalFaceCount += faceCount;
                }

                if (data.VertexMorph != null)
                {
                    Enumerable.Range(0, data.VertexMorph.MorphArray.Length).ForEach(i => data.VertexMorph.MorphArray[i].Index += vertexOffset + vertexList.Count);
                }

                vertexList.AddRange(data.Property.Type.CreateVertices(World, data.Property).Select(v => SetupVertex(v, data.BoneIndexOffset)));
            }

            vertices = vertexList.ToArray();
            indices = indicesEachMaterial.SelectMany(list => list).ToArray();

            PmxVertexData SetupVertex(PmxVertexData vertex, int boneOffset)
            {
                vertex.BoneId = vertex.BoneId.Select(i => i + boneOffset).ToArray();
                return vertex;
            }
        }
    }
}
