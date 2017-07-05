using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;

namespace CurtainFireMakerPlugin.Entities
{
    internal class ShotModelData
    {
        public PmxMorphData morph = new PmxMorphData();

        public PmxBoneData[] bones;
        public PmxVertexData[] vertices;
        public int[] indices;
        public PmxMaterialData[] materials;
        public String[] textures;

        private readonly ShotProperty property;
        public ShotProperty Property => this.property;

        public ShotModelData(ShotProperty property)
        {
            this.property = property;

            this.bones = this.Property.Type.GetBones(this.Property);
            this.vertices = this.Property.Type.GetVertices(this.Property);
            this.indices = this.Property.Type.GetVertexIndices(this.Property);
            this.materials = this.Property.Type.GetMaterials(this.Property);
            this.textures = this.Property.Type.GetTextures(this.Property);
        }
    }
}
