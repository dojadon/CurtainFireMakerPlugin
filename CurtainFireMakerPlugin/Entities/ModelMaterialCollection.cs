using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsMmdDataIO.Pmx;
using System.Collections;

namespace CurtainFireMakerPlugin.Entities
{
    public class ModelMaterialCollection 
    {
        public List<PmxMaterialData> MaterialList { get; } = new List<PmxMaterialData>();
        public List<string> TextureList { get; } = new List<string>();

        public World World { get; }

        public ModelMaterialCollection(World world)
        {
            World = world;
        }

        public void SetupMaterials(ShotProperty prop, PmxMaterialData[] materials, string[] textures)
        {
            foreach (var texture in textures)
            {
                if (!TextureList.Contains(texture))
                {
                    TextureList.Add(texture);
                }
            }

            foreach (PmxMaterialData material in materials)
            {
                material.MaterialName = "MA" + MaterialList.Count;

                if (0 <= material.TextureId && material.TextureId < textures.Length)
                {
                    material.TextureId = TextureList.IndexOf(textures[material.TextureId]);
                }
                else
                {
                    material.TextureId = -1;
                }

                if (0 <= material.SphereId && material.SphereId < textures.Length)
                {
                    material.SphereId = TextureList.IndexOf(textures[material.SphereId]);
                }
                else
                {
                    material.SphereId = -1;
                }
                MaterialList.Add(material);
            }
        }

        public void CompressMaterial(ModelVertexCollection vertices)
        {
            var materialDict = new MultiDictionary<int, int>();
            for (int i = 0; i < MaterialList.Count; i++)
            {
                materialDict.Add(GetHashCode(MaterialList[i]), i);
            }

            var removeIndices = CompressGroupedMaterial(materialDict.Values.ToList(), vertices);

            removeIndices.Sort((a, b) => b - a);
            foreach (int index in removeIndices)
            {
                MaterialList.RemoveAt(index);
            }

            int GetHashCode(PmxMaterialData obj)
            {
                int result = 17;

                result = result * 31 + obj.Ambient.GetHashCode();
                result = result * 31 + obj.Diffuse.GetHashCode();
                result = result * 31 + obj.Specular.GetHashCode();
                result = result * 31 + obj.Shininess.GetHashCode();
                result = result * 31 + obj.TextureId;
                result = result * 31 + obj.SphereId;

                return result;
            }
        }

        private List<int> CompressGroupedMaterial(List<List<int>> groupedMaterialIndices, ModelVertexCollection vertices)
        {
            var vertexIndicesList = CreateVertexIndicesEachMaterial(vertices.Indices);
            var newVertexIndicesList = new List<List<int>>();

            var removeList = new List<int>();

            foreach (var materialIndices in groupedMaterialIndices)
            {
                var vertexIndices = vertexIndicesList[materialIndices[0]];

                if (materialIndices.Count > 1)
                {
                    int addFaceCount = 0;

                    for (int i = 1; i < materialIndices.Count; i++)
                    {
                        int index = materialIndices[i];

                        addFaceCount += MaterialList[index].FaceCount;
                        vertexIndices.AddRange(vertexIndicesList[index]);
                        removeList.Add(index);
                    }
                    MaterialList[materialIndices[0]].FaceCount += addFaceCount;
                }
                newVertexIndicesList.Add(vertexIndices);
            }

            var list = new List<int>();
            foreach (var vertexIndices in newVertexIndicesList)
            {
                list.AddRange(vertexIndices);
            }

            World.PmxModel.Vertices.Indices.Clear();
            World.PmxModel.Vertices.Indices.AddRange(list);

            return removeList;
        }

        private List<List<int>> CreateVertexIndicesEachMaterial(List<int> vertexIndices)
        {
            var vertexIndicesEachMaterial = new List<List<int>>();

            int total = 0;
            foreach (var material in MaterialList)
            {
                vertexIndicesEachMaterial.Add(vertexIndices.GetRange(total, material.FaceCount));
                total += material.FaceCount;
            }
            return vertexIndicesEachMaterial;
        }
    }
}
