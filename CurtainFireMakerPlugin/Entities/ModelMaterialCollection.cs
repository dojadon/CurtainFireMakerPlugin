using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using MMDataIO.Pmx;

namespace CurtainFireMakerPlugin.Entities
{
    public class ModelMaterialCollection
    {
        public List<PmxMaterialData> MaterialList { get; } = new List<PmxMaterialData>();
        public Dictionary<PmxMaterialData, string[]> TexturesEachMaterialDict { get; } = new Dictionary<PmxMaterialData, string[]>();

        public PmxMaterialData[] MaterialArray => MaterialList.ToArray();
        public string[] TextureArray { get; private set; }

        public World World { get; }

        public ModelMaterialCollection(World world)
        {
            World = world;
        }

        public void SetupMaterials(PmxMaterialData[] materials, string[] textures)
        {
            foreach (PmxMaterialData material in materials)
            {
                material.MaterialName = "MA" + MaterialList.Count;

                MaterialList.Add(material);
                TexturesEachMaterialDict.Add(material, textures);
            }
        }

        public void CompressMaterial(List<int> vertexIndices)
        {
            var grouptedMaterialIndices = Enumerable.Range(0, MaterialList.Count).ToLookup(i => GetMaterialHashCode(MaterialList[i]));
            var removedMaterialIndices = CompressGroupedMaterial(grouptedMaterialIndices, vertexIndices);

            foreach (int removedMaterialIndex in removedMaterialIndices.OrderByDescending(i => i))
            {
                MaterialList.RemoveAt(removedMaterialIndex);
            }

            int GetMaterialHashCode(PmxMaterialData obj)
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

        private IEnumerable<int> CompressGroupedMaterial(IEnumerable<IEnumerable<int>> groupedMaterialIndices, List<int> vertexIndices)
        {
            var vertexIndicesEachMaterial = CreateVertexIndicesEachMaterial(vertexIndices);
            vertexIndices.Clear();

            var removedMaterialIndices = new List<int>();

            foreach (var materialIndices in groupedMaterialIndices)
            {
                vertexIndices.AddRange(materialIndices.SelectMany(i => vertexIndicesEachMaterial[i]));

                if (materialIndices.Skip(1).Any())
                {
                    removedMaterialIndices.AddRange(materialIndices.Skip(1));
                    MaterialList[materialIndices.First()].FaceCount = materialIndices.Sum(i => MaterialList[i].FaceCount);
                }
            }
            return removedMaterialIndices;
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

        public void FinalizeTextures()
        {
            var textureList = TexturesEachMaterialDict.Values.SelectMany(s => s).Distinct().ToList();

            foreach (var material in MaterialList)
            {
                var textures = TexturesEachMaterialDict[material];
                int GetTextureId(int i) => (0 <= i && i < textures.Length) ? textureList.IndexOf(textures[i]) : -1;

                material.TextureId = GetTextureId(material.TextureId);
                material.SphereId = GetTextureId(material.SphereId);
            }
            TextureArray = textureList.ToArray();
        }
    }
}
