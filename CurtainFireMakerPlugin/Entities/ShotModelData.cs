using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;

namespace CurtainFireMakerPlugin.Entities
{
    public class ShotModelData
    {
        public PmxMorphData MaterialMorph { get; } = new PmxMorphData();

        public PmxBoneData[] Bones { get; }
        public PmxVertexData[] Vertices { get; }
        public int[] Indices { get; }
        public PmxMaterialData[] Materials { get; }
        public String[] Textures { get; }

        public ShotProperty Property { get; }

        public ShotModelData(ShotProperty property)
        {
            this.Property = property;

            this.Bones = this.Property.Type.CreateBones();
            this.Vertices = this.Property.Type.CreateVertices();
            this.Indices = this.Property.Type.CreateVertexIndices();
            this.Materials = this.Property.Type.CreateMaterials();
            this.Textures = this.Property.Type.CreateTextures();
        }
    }
}
