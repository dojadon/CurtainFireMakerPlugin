using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMDataIO.Pmx;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class ModelVertexCollection
    {
        public World World { get; }

        public MultiDictionary<ShotProperty, int> BoneCountEachPropertyDict { get; } = new MultiDictionary<ShotProperty, int>();

        private Random Random { get; } = new Random();

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

                foreach (var morph in data.Morphs.Values.Where(ShouldAddIndex))
                {
                    Enumerable.Range(0, morph.MorphArray.Length).ForEach(i => morph.MorphArray[i].Index += vertexOffset + vertexList.Count);
                }

                bool ShouldAddIndex(PmxMorphData m)
                {
                    return (m.MorphType & (MorphType.VERTEX | MorphType.UV | MorphType.EXUV1 | MorphType.EXUV2 | MorphType.EXUV3 | MorphType.EXUV4)) > 0;
                }

                Vector4 exuv1 = data.Property.Type.GetExtraUv1();
                Vector4 exuv2 = data.Property.Type.GetExtraUv2();
                Vector4 exuv3 = data.Property.Type.GetExtraUv3();
                Vector4 exuv4 = data.Property.Type.GetExtraUv4();

                vertexList.AddRange(data.Property.Type.CreateVertices(World, data.Property).Select(v => SetupVertex(v, data.BoneIndexOffset)));

                PmxVertexData SetupVertex(PmxVertexData vertex, int boneOffset)
                {
                    vertex.BoneId = vertex.BoneId.Select(i => i + boneOffset).ToArray();

                    vertex.ExtraUv1 = exuv1;
                    vertex.ExtraUv2 = exuv2;
                    vertex.ExtraUv3 = exuv3;
                    vertex.ExtraUv4 = exuv4;
                    return vertex;
                }
            }

            vertices = vertexList.ToArray();
            indices = indicesEachMaterial.SelectMany(list => list).ToArray();
        }
    }
}
