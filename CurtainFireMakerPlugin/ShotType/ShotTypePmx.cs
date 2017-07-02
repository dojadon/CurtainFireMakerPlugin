using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPmx;
using CPmx.Data;
using CurtainFireMakerPlugin.Entity;

namespace CurtainFireMakerPlugin.ShotType
{
    class ShotTypePmx : ShotType
    {
        private PmxModelData data;

        public ShotTypePmx(String name, String pmxFilePath) : base(name)
        {
        }

        override public PmxVertexData[] GetVertices(EntityShot entity)
        {
            PmxVertexData[] result = new PmxVertexData[this.data.VertexArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
            }
            return result;
        }

        override public int[] GetVertexIndices(EntityShot entity)
        {
            var result = new int[this.data.VertexIndices.Length];
            Array.Copy(this.data.VertexIndices, result, this.data.VertexIndices.Length);

            return result;
        }

        override public PmxMaterialData[] GetMaterials(EntityShot entity)
        {
            PmxMaterialData[] result = new PmxMaterialData[this.data.MaterialArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
            }
            return result;
        }

        override public String[] GetTextures(EntityShot entity)
        {
            String[] result = new String[this.data.TextureFiles.Length];
            for (int i = 0; i < result.Length; i++)
            {
            }
            return result;
        }

        override public PmxBoneData[] GetBones(EntityShot entity)
        {
            PmxBoneData[] result = new PmxBoneData[this.data.BoneArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
            }
            return result;
        }
    }
}
