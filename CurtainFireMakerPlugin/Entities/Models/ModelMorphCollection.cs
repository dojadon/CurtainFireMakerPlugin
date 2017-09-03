using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Collections;
using CsPmx;
using CsPmx.Data;
using CsVmd.Data;
using VecMath;

namespace CurtainFireMakerPlugin.Entities.Models
{
    public class ModelMorphCollection
    {
        public List<PmxMorphData> MorphList { get; } = new List<PmxMorphData>();

        public World World { get; }

        public ModelMorphCollection(World world)
        {
            World = world;
        }

        public void SetupMaterialMorph(PmxMorphData morph, int materialCount, int appliedMaterialCount)
        {
            morph.MorphName = MorphList.Count.ToString();
            morph.Type = 4;

            morph.MorphArray = new PmxMorphMaterialData[appliedMaterialCount];

            for (int i = 0; i < appliedMaterialCount; i++)
            {
                morph.MorphArray[i] = new PmxMorphMaterialData()
                {
                    CalcType = 0,
                    Ambient = new Vector3(1, 1, 1),
                    Diffuse = new Vector4(1, 1, 1, 0),
                    Specular = new Vector3(1, 1, 1),
                    Shininess = 1.0F,
                    Edge = new Vector4(1, 1, 1, 1),
                    EdgeThick = 1.0F,
                    Texture = new Vector4(1, 1, 1, 1),
                    SphereTexture = new Vector4(1, 1, 1, 1),
                    ToonTexture = new Vector4(1, 1, 1, 1),
                };
            }

            for (int i = 0; i < appliedMaterialCount; i++)
            {
                morph.MorphArray[i].Index = materialCount + i;
                morph.MorphId = MorphList.Count + i;
            }
            MorphList.Add(morph);
        }

        public void CompressMorph(ModelMaterialCollection materials, ModelVertexCollection vertices)
        {
            CurtainFireMotion vmdMotion = World.VmdMotion;

            var typeMorphDict = new MultiDictionary<byte, PmxMorphData>();
            foreach (var morph in MorphList)
            {
                typeMorphDict.Add(morph.MorphArray[0].GetMorphType(), morph);
            }

            foreach (var morphList in typeMorphDict.Values)
            {
                Compress(morphList, vmdMotion.MorphDict);
            }

            typeMorphDict = new MultiDictionary<byte, PmxMorphData>();
            foreach (var morph in MorphList)
            {
                typeMorphDict.Add(morph.MorphArray[0].GetMorphType(), morph);
            }

            var removeMaterialIndices = new List<int>();
            removeMaterialIndices.AddRange(materials.CompressMaterial(typeMorphDict[PmxMorphData.MORPHTYPE_MATERIAL], vertices));

            RemoveElements(typeMorphDict[PmxMorphData.MORPHTYPE_MATERIAL], removeMaterialIndices);

            removeMaterialIndices.Sort((a, b) => b - a);
            foreach (int index in removeMaterialIndices)
            {
                materials.MaterialList.RemoveAt(index);
            }
        }

        private void Compress(List<PmxMorphData> morphList, MultiDictionary<PmxMorphData, VmdMorphFrameData> frameDataDict)
        {
            var dict = new MultiDictionary<int[], PmxMorphData>(new IntegerArrayComparer());

            foreach (var morph in morphList)
            {
                dict.Add(Array.ConvertAll(frameDataDict[morph].ToArray(), m => m.KeyFrameNo), morph);
            }

            foreach (var key in dict.Keys)
            {
                var removeList = dict[key];

                if (removeList.Count > 1)
                {
                    removeList[0].MorphArray = Compress(removeList);

                    for (int i = 1; i < removeList.Count; i++)
                    {
                        var morph = removeList[i];

                        MorphList.Remove(morph);
                        World.VmdMotion.MorphDict.Remove(morph);
                    }
                }
            }
        }

        private IPmxMorphTypeData[] Compress(List<PmxMorphData> list)
        {
            var morphTypeDataList = new List<IPmxMorphTypeData>();
            foreach (var morph in list)
            {
                morphTypeDataList.AddRange(morph.MorphArray);
            }
            return morphTypeDataList.ToArray();
        }

        private void RemoveElements(List<PmxMorphData> morphList, List<int> removeIndices)
        {
            foreach (var morph in morphList)
            {
                var typeList = new List<IPmxMorphTypeData>();

                foreach (var typeMorph in morph.MorphArray)
                {
                    if (!removeIndices.Contains(typeMorph.Index))
                    {
                        typeMorph.Index -= removeIndices.FindAll(i => i < typeMorph.Index).Count;
                        typeList.Add(typeMorph);
                    }
                }
                morph.MorphArray = typeList.ToArray();
            }
        }
    }

    class IntegerArrayComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(int[] obj)
        {
            int result = 17;
            for (int i = 0; i < obj.Length; i++)
            {
                unchecked
                {
                    result = result * 23 + obj[i];
                }
            }
            return result;
        }
    }
}
