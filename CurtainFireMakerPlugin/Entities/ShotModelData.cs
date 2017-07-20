using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;

namespace CurtainFireMakerPlugin.Entities
{
    internal class ShotModelData
    {
        public PmxMorphData Morph { get; } = new PmxMorphData();

        public PmxBoneData[] Bones { get; }
        public PmxVertexData[] Vertices { get; }
        public int[] Indices { get; }
        public PmxMaterialData[] Materials { get; }
        public String[] Textures { get; }

        public ShotProperty Property { get; }

        public ShotModelData(ShotProperty property)
        {
            this.Property = property;

            this.Bones = this.Property.Type.GetBones(this.Property);
            this.Vertices = this.Property.Type.GetVertices(this.Property);
            this.Indices = this.Property.Type.GetVertexIndices(this.Property);
            this.Materials = this.Property.Type.GetMaterials(this.Property);
            this.Textures = this.Property.Type.GetTextures(this.Property);
        }
    }
}
