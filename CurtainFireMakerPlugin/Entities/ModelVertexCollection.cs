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

        public void CreateVertices(ShotProperty prop, IEnumerable<ShotModelData> dataList, int vertexOffset, out PmxVertexData[] vertices, out int[] indices)
        {
            var indicesEachMaterial = prop.Type.OriginalData.MaterialArray.Select(m => new List<int>()).ToArray();
            var vertexList = new List<PmxVertexData>();

            foreach (var data in dataList)
            {
                for (int i = 0, totalFaceCount = 0; i < prop.Type.OriginalData.MaterialArray.Length; i++)
                {
                    int faceCount = prop.Type.OriginalData.MaterialArray[i].FaceCount;
                    indicesEachMaterial[i].AddRange(prop.Type.OriginalData.VertexIndices.Skip(totalFaceCount).Take(faceCount).Select(idx => idx + vertexOffset + vertexList.Count));
                    totalFaceCount += faceCount;
                }

                if (data.VertexMorph != null)
                {
                    //    Enumerable.Range(0, data.VertexMorph.MorphArray.Length).ForEach(i => Console.WriteLine(data.VertexMorph.MorphArray[i].Index));
                    Enumerable.Range(0, data.VertexMorph.MorphArray.Length).ForEach(i => data.VertexMorph.MorphArray[i].Index += vertexOffset + vertexList.Count);
                }

                vertexList.AddRange(prop.Type.CreateVertices(World, prop).Select(v => SetupVertex(v, data.BoneIndexOffset)));
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
