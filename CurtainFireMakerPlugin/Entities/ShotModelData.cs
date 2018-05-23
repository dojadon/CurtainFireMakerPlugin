using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMDataIO.Pmx;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class ShotModelData
    {
        public Dictionary<int, PmxMorphData> Morphs { get; } = new Dictionary<int, PmxMorphData>();

        public PmxBoneData[] Bones { get; }

        public ShotProperty Property { get; }
        public World World { get; }

        public int BoneIndexOffset { get; set; }

        public bool IsInitialized { get; set; } = false;

        public ShotModelData(World world, ShotProperty property)
        {
            Property = property;
            World = world;

            Bones = Property.Type.CreateBones(world, property);
        }

        public PmxMorphData CreateVertexMorph(string name, int id, Func<Vector3, Vector3> func)
        {
            if (!Morphs.ContainsKey(id))
            {
                Morphs[id] = new PmxMorphData()
                {
                    MorphName = name,
                    SlotType = MorphSlotType.RIP,
                    MorphType = MorphType.VERTEX,

                    MorphArray =
                    Enumerable.Range(0, Property.Type.OriginalData.VertexArray.Length)
                    .Select(i => (IPmxMorphTypeData)new PmxMorphVertexData() { Index = i, Position = func((Vector4)Property.Type.OriginalData.VertexArray[i].Pos * Property.Scale) }).ToArray()
                };
                World.PmxModel.Morphs.MorphList.Add(Morphs[id]);
            }
            return Morphs[id];
        }
    }
}
