using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    public class PmxMorphMaterialData : IPmxMorphTypeData
    {
        public int[] indices;

        public byte[] calcType;
        public Vector4[] diffuse;
        public Vector3[] specular;
        public float[] shininess;
        public Vector3[] ambient;
        public Vector4[] edge;
        public float[] edgeThick;
        public Vector4[] texture;
        public Vector4[] sphereTexture;
        public Vector4[] toonTexture;

        public void Export(PmxExporter exporter)
        {
            exporter.Write(this.indices.Length);
            for (int i = 0; i < this.indices.Length; i++)
            {
                exporter.WritePmxId(PmxExporter.SIZE_MATERIAL, this.indices[i]);

                exporter.Write(this.calcType[i]);
                exporter.Write(this.diffuse[i]);
                exporter.Write(this.specular[i]);
                exporter.Write(this.shininess[i]);
                exporter.Write(this.ambient[i]);
                exporter.Write(this.edge[i]);
                exporter.Write(this.edgeThick[i]);
                exporter.Write(this.texture[i]);
                exporter.Write(this.sphereTexture[i]);
                exporter.Write(this.toonTexture[i]);
            }
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_MATERIAL;
        }

        public void SetIndices(int[] indices)
        {
            this.indices = indices;

            this.calcType = new byte[indices.Length];
            this.diffuse = ArrayUtil.Set(new Vector4[indices.Length], i => new Vector4());
            this.specular = ArrayUtil.Set(new Vector3[indices.Length], i => new Vector3());
            this.shininess = new float[indices.Length];
            this.ambient = ArrayUtil.Set(new Vector3[indices.Length], i => new Vector3());
            this.edge = ArrayUtil.Set(new Vector4[indices.Length], i => new Vector4());
            this.edgeThick = new float[indices.Length];
            this.texture = ArrayUtil.Set(new Vector4[indices.Length], i=> new Vector4());
            this.sphereTexture = ArrayUtil.Set(new Vector4[indices.Length], i=> new Vector4());
            this.toonTexture = ArrayUtil.Set(new Vector4[indices.Length], i=> new Vector4());
        }
    }
}
