using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;

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

        public void SetupMaterialsAndTextures(PmxMaterialData[] materials, string[] textures)
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

        private List<int> CompressMaterial(PmxMorphData morph)
        {
            int[] indices = Array.ConvertAll(morph.MorphArray, m => m.Index);
            Array.Sort(indices);

            var materialsList = new List<List<int>>();
            List<int> currentList = null;

            int lastIndex = indices[0];
            foreach (int index in indices)
            {
                if (!(Math.Abs(index - lastIndex) == 1 && Equals(MaterialList[index], MaterialList[lastIndex])))
                {
                    if (currentList != null)
                    {
                        materialsList.Add(currentList);
                    }
                    currentList = new List<int>();
                }
                currentList.Add(index);
                lastIndex = index;
            }

            var removeList = new List<int>();

            foreach (var list in materialsList)
            {
                if (list.Count > 1)
                {
                    var compressedMaterial = MaterialList[list[0]];

                    for (int i = 1; i < list.Count; i++)
                    {
                        compressedMaterial.FaceCount += MaterialList[list[i]].FaceCount;
                        removeList.Add(list[i]);
                    }
                }
            }

            return removeList;

            bool Equals(PmxMaterialData x, PmxMaterialData y)
            {
                return x.Ambient == y.Ambient && x.Diffuse == y.Diffuse && x.Specular == y.Specular && x.Shininess == y.Shininess
                && x.TextureId == y.TextureId && x.SphereId == y.SphereId;
            }
        }
    }
}
