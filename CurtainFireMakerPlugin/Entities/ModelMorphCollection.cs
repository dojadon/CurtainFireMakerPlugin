using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsMmdDataIO.Pmx;
using CsMmdDataIO.Vmd;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class ModelMorphCollection
    {
        public List<PmxMorphData> MorphList { get; } = new List<PmxMorphData>();
        public World World { get; }

        public ModelMorphCollection(World world)
        {
            World = world;
        }

        public void CompressMorph()
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
        }

        private void Compress(List<PmxMorphData> morphList, MultiDictionary<PmxMorphData, VmdMorphFrameData> frameDict)
        {
            var dict = new MultiDictionary<List<int>, PmxMorphData>(new IntegerArrayComparer());

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
                        morphList.Remove(morph);
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
    }

    class IntegerArrayComparer : IEqualityComparer<List<int>>
    {
        public bool Equals(List<int> x, List<int> y)
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

        public int GetHashCode(List<int> obj)
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
