using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Collections;
using CsPmx.Data;
using VecMath;
using System.Collections;

namespace CurtainFireMakerPlugin.Entities.Models
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

        public void SetupMaterials(PmxMaterialData[] materials, string[] textures)
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
                material.MaterialName = MaterialList.Count.ToString();

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
                material.MaterialId = MaterialList.Count;
                MaterialList.Add(material);
            }
        }

        public List<int> CompressMaterial(List<PmxMorphData> morphList, ModelVertexCollection vertices)
        {
            var groupedMaterialIndices = new List<List<int>>();

            foreach (var morph in morphList)
            {
                int[] indices = Array.ConvertAll(morph.MorphArray, m => m.Index);
                Array.Sort(indices);

                var materialDict = new MultiDictionary<int, int>();

                foreach (int index in indices)
                {
                    materialDict.Add(GetHashCode(MaterialList[index]), index);
                }

                foreach (var materialIndices in materialDict.Values)
                {
                    groupedMaterialIndices.Add(materialIndices);
                }
            }

            return CompressGroupedMaterial(groupedMaterialIndices, vertices);

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
            var vertexIndicesList = vertices.IndexOfEachMaterialList;
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

            World.PmxModel.Vertices.IndexOfEachMaterialList.Clear();
            World.PmxModel.Vertices.IndexOfEachMaterialList.AddRange(newVertexIndicesList);

            var list = new List<int>();
            foreach (var vertexIndices in newVertexIndicesList)
            {
                list.AddRange(vertexIndices);
            }

            World.PmxModel.Vertices.IndexList.Clear();
            World.PmxModel.Vertices.IndexList.AddRange(list);

            return removeList;
        }
    }
}
