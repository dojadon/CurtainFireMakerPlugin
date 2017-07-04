using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPmx.Data;

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
        }
    }
}
