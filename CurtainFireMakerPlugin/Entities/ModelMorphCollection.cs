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
            var framesEachMorph = morphFrameList.ToLookup(f => f.Name);

            foreach (var group in MorphList.ToLookup(m => m.MorphType))
            {
                Compress(group, framesEachMorph);
            }
        }

        private void Compress(IEnumerable<PmxMorphData> morphList, ILookup<string, VmdMorphFrameData> framesEachMorph)
        {
            var groupedMorphs = morphList.ToLookup(m => (from f in framesEachMorph[m.MorphName] select f.FrameTime).ToArray(), new IntegerArrayComparer());

            foreach (var group in groupedMorphs)
            {
                var morphs = group.ToList();

                if (morphs.Count > 1)
                {
                    morphs[0].MorphArray = morphs.SelectMany(m => m.MorphArray).ToArray();

                    for (int i = 1; i < morphs.Count; i++)
                    {
                        MorphList.Remove(morphs[i]);
                    }
                }
            }
        }
    }

    class IntegerArrayComparer : IEqualityComparer<int[]>
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
