using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using CsMmdDataIO.Pmx;

namespace CurtainFireMakerPlugin.Entities
{
    public class ModelMaterialCollection
    {
        public List<PmxMaterialData> MaterialList { get; } = new List<PmxMaterialData>();
        public List<string> TextureList { get; } = new List<string>();

        private List<(ShotType type, PmxMaterialData material)> MaterialAndShotTypeList { get; } = new List<(ShotType, PmxMaterialData)>();

        public World World { get; }

        public ModelMaterialCollection(World world)
        {
            World = world;
        }

        public void SetupMaterials(ShotType type, PmxMaterialData[] materials, string[] textures)
        {
            foreach (var texture in textures)
            {
                if (!TextureList.Contains(texture))
                {
                    TextureList.Add(texture);
                }
            }

            int GetTextureId(int i) => (0 <= i && i < textures.Length) ? TextureList.IndexOf(textures[i]) : -1;

            foreach (PmxMaterialData material in materials)
            {
                material.MaterialName = "MA" + MaterialList.Count;
                material.TextureId = GetTextureId(material.TextureId);
                material.SphereId = GetTextureId(material.SphereId);

                MaterialList.Add(material);
                MaterialAndShotTypeList.Add((type, material));
            }
        }

        public void CompressMaterial(List<int> vertexIndices)
        {
            var grouptedMaterialIndices = MaterialAndShotTypeList.ToLookup(t => GetMaterialHashCode(t), t => MaterialList.IndexOf(t.material));
            var removedMaterialIndices = CompressGroupedMaterial(grouptedMaterialIndices, vertexIndices);

            foreach (int removedMaterialIndex in removedMaterialIndices.OrderByDescending(i => i))
            {
                MaterialList.RemoveAt(removedMaterialIndex);
            }

            int GetMaterialHashCode((ShotType type, PmxMaterialData material) tuple)
            {
                int result = 17;

                result = result * 31 + tuple.material.Ambient.GetHashCode();
                result = result * 31 + tuple.material.Diffuse.GetHashCode();
                result = result * 31 + tuple.material.Specular.GetHashCode();
                result = result * 31 + tuple.material.Shininess.GetHashCode();
                result = result * 31 + tuple.material.TextureId;
                result = result * 31 + tuple.material.SphereId;
                result = result * 31 + tuple.type.Id;

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
    }
}
