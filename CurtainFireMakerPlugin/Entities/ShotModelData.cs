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
        public PmxMorphData VertexMorph { get; set; }

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

        public PmxMorphData CreateVertexMorph(string morphName, Func<Vector3, Vector3> func)
        {
            if (VertexMorph == null)
            {
                VertexMorph = new PmxMorphData()
                {
                    MorphName = morphName,
                    SlotType = MorphSlotType.RIP,
                    MorphType = MorphType.VERTEX,

                    MorphArray =
                    Enumerable.Range(0, Property.Type.OriginalData.VertexArray.Length)
                    .Select(i => (IPmxMorphTypeData)new PmxMorphVertexData() { Index = i, Position = func((Vector4)Property.Type.OriginalData.VertexArray[i].Pos * Property.Scale) })
                    .ToArray()
                };

                World.PmxModel.Morphs.MorphList.Add(VertexMorph);
            }
            return VertexMorph;
        }
    }
}
