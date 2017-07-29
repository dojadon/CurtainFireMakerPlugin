using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;

namespace CurtainFireMakerPlugin.Entities
{
    public class ShotModelData
    {
        public List<EntityShot> EntityList { get; } = new List<EntityShot>();

        public PmxMorphData MaterialMorph { get; } = new PmxMorphData();

        public Dictionary<string, PmxMorphData> MorphDict { get; } = new Dictionary<string, PmxMorphData>();

        public PmxBoneData[] Bones { get; }
        public PmxVertexData[] Vertices { get; }
        public int[] Indices { get; }
        public PmxMaterialData[] Materials { get; }
        public String[] Textures { get; }

        public ShotProperty Property { get; }
        public World World { get; }

        public ShotModelData(ShotProperty property, World world)
        {
            Property = property;
            World = world;

            Bones = Property.Type.CreateBones();
            Vertices = Property.Type.CreateVertices();
            Indices = Property.Type.CreateVertexIndices();
            Materials = Property.Type.CreateMaterials();
            Textures = Property.Type.CreateTextures();
        }

        public void AddMorph(PmxMorphData morph)
        {
            MorphDict.Add(morph.MorphName, morph);
            World.PmxModel.MorphList.Add(morph);
        }
    }
}
