using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;

namespace CurtainFireMakerPlugin.Entities
{
    public class ShotModelData
    {
        public HashSet<EntityShot> OwnerEntities { get; } = new HashSet<EntityShot>();

        public PmxMorphData MaterialMorph { get; } = new PmxMorphData();

        public Dictionary<string, PmxMorphData> MorphDict { get; } = new Dictionary<string, PmxMorphData>();

        public PmxBoneData[] Bones { get; }
        public PmxVertexData[] Vertices { get; }
        public int[] Indices { get; }
        public PmxMaterialData[] Materials { get; }
        public String[] Textures { get; }

        public ShotProperty Property { get; }
        public World World { get; }

        public ShotModelData(World world, ShotProperty property)
        {
            Property = property;
            World = world;

            Bones = Property.Type.GetCreateBones(world, property);
            Vertices = Property.Type.CreateVertices(world, property);
            Indices = Property.Type.CreateVertexIndices(world, property);
            Materials = Property.Type.CreateMaterials(world, property);
            Textures = Property.Type.CreateTextures(world, property);
        }

        public void AddMorph(PmxMorphData morph)
        {
            MorphDict.Add(morph.MorphName, morph);
            World.PmxModel.MorphList.Add(morph);
        }
    }
}
