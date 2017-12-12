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

        public void CompressMorph(IEnumerable<VmdMorphFrameData> morphFrameList)
        {
            var framesEachMorph = morphFrameList.ToLookup(f => f.Name, f => f.FrameTime).ToDictionary(g => g.Key, g => g.ToArray());

            foreach(var removedMorph in MorphList.ToLookup(m => m.MorphType).SelectMany(g => Grouping(g, framesEachMorph)))
            {
                MorphList.Remove(removedMorph);
            }
        }

        private IEnumerable<PmxMorphData> Grouping(IEnumerable<PmxMorphData> morphList, Dictionary<string, int[]> framesEachMorph)
        {
            var removedMorphs = new List<PmxMorphData>();

            foreach (var group in morphList.ToLookup(m => framesEachMorph[m.MorphName], new IntArrayComparer()).Where(g => g.Count() > 1))
            {
                group.First().MorphArray = group.SelectMany(m => m.MorphArray).ToArray();
                removedMorphs.AddRange(group.Skip(1));
            }
            return removedMorphs;
        }
    }

    class IntArrayComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            if (x.Length != y.Length) return false;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i]) return false;
            }
            return true;
        }

        public int GetHashCode(int[] obj)
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
