using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMDataIO.Pmx;
using MMDataIO.Vmd;

namespace CurtainFireMakerPlugin.Entities
{
    public class ModelMorphCollection
    {
        public World World { get; }

        public List<PmxMorphData> MorphList { get; } = new List<PmxMorphData>();

        public ModelMorphCollection(World world)
        {
            World = world;
        }

        public void CompressMorph(IEnumerable<ShotModelData> dataList, IEnumerable<VmdMorphFrameData> morphFrameList, out PmxMorphData[] morphs)
        {
            var morphList = dataList.SelectMany(d => d.Morphs.Values).Union(MorphList).ToList();
            var framesEachMorph = morphFrameList.ToLookup(f => f.Name, f => f.FrameTime).ToDictionary(g => g.Key, g => g.ToArray());

            morphs = morphList.Except(morphList.ToLookup(m => m.MorphType).SelectMany(g => Grouping(g, framesEachMorph))).ToArray();
        }

        private IEnumerable<PmxMorphData> Grouping(IEnumerable<PmxMorphData> morphList, Dictionary<string, int[]> framesEachMorph)
        {
            var removedMorphs = new List<PmxMorphData>();

            foreach (var group in morphList.GroupBy(m => GetHashCode(framesEachMorph[m.MorphName])).Where(g => g.Skip(1).Any()))
            {
                group.First().MorphArray = group.SelectMany(m => m.MorphArray).ToArray();
                removedMorphs.AddRange(group.Skip(1));
            }
            return removedMorphs;

            int GetHashCode(int[] obj)
            {
                int result = 17;
                foreach (int i in obj)
                {
                    unchecked
                    {
                        result = result * 23 + i;
                    }
                }
                return result;
            }
        }
    }
}
