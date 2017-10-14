using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Collections;
using CsMmdDataIO.Pmx;
using CsMmdDataIO.Pmx.Data;
using CsMmdDataIO.Vmd.Data;
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

        public void SetupMaterialMorph(ShotProperty prop, PmxMorphData morph, int materialCount, int appliedMaterialCount)
        {
            morph.MorphName ="MO_" +  prop.Type.Name[0] + MorphList.Count.ToString();
            morph.SlotType = MorphSlotType.RIP;
            morph.MorphType = MorphType.MATERIAL;

            morph.MorphArray = new IPmxMorphTypeData[appliedMaterialCount];

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
            }
            MorphList.Add(morph);
        }

        public void CompressMorph(ModelMaterialCollection materials, ModelVertexCollection vertices)
        {
            var typeMorphDict = new MultiDictionary<MorphType, PmxMorphData>();
            foreach (var morph in MorphList)
            {
                typeMorphDict.Add(morph.MorphType, morph);
            }

            foreach (var morphList in typeMorphDict.Values)
            {
                Compress(morphList, World.KeyFrames.MorphFrameDict);
            }

            typeMorphDict = new MultiDictionary<MorphType, PmxMorphData>();
            foreach (var morph in MorphList)
            {
                typeMorphDict.Add(morph.MorphType, morph);
            }

            var removeMaterialIndices = new List<int>();
            removeMaterialIndices.AddRange(materials.CompressMaterial(typeMorphDict[MorphType.MATERIAL], vertices));

            RemoveElements(typeMorphDict[MorphType.MATERIAL], removeMaterialIndices);

            removeMaterialIndices.Sort((a, b) => b - a);
            foreach (int index in removeMaterialIndices)
            {
                materials.MaterialList.RemoveAt(index);
            }
        }

        private void Compress(List<PmxMorphData> morphList, MultiDictionary<PmxMorphData, VmdMorphFrameData> frameDict)
        {
            var dict = new MultiDictionary<List<long>, PmxMorphData>(new IntegerArrayComparer());

            foreach (var morph in morphList)
            {
                dict.Add(frameDict[morph].ConvertAll(m => m.FrameTime), morph);
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

    class IntegerArrayComparer : IEqualityComparer<List<long>>
    {
        public bool Equals(List<long> x, List<long> y)
        {
            if (x.Count != y.Count)
            {
                return false;
            }
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(List<long> obj)
        {
            int result = 17;
            for (int i = 0; i < obj.Count; i++)
            {
                unchecked
                {
                    result = result * 23 + obj[i].GetHashCode();
                }
            }
            return result;
        }
    }
}
