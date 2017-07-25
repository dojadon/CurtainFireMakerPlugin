using System;
using CsPmx;
using CsPmx.Data;
using CurtainFireMakerPlugin.Entities;
using CurtainFireMakerPlugin.Mathematics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public class ShotTypePmx : ShotType
    {
        private PmxModelData Data { get; } = new PmxModelData();
        private Vector3 VertexScale { get; }

        public Action<ShotProperty, PmxMaterialData[]> InitMaterials { get; set; } = (prop, materials) =>
        {
            foreach (var material in materials)
            {
                material.Diffuse = new DxMath.Vector4(prop.Red, prop.Green, prop.Blue, 1.0F);
                material.Ambient = new DxMath.Vector3(prop.Red, prop.Green, prop.Blue);
            }
        };

        public ShotTypePmx(string name, string path, float size) : this(name, path, new Vector3(size, size, size)) { }

        public ShotTypePmx(string name, string path, Vector3 size) : base(name)
        {
            var inStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (inStream)
            {
                PmxParser parser = new PmxParser(inStream);
                parser.Parse(this.Data);
            }

            VertexScale = size;
        }

        public override PmxVertexData[] GetVertices(ShotProperty property)
        {
            PmxVertexData[] result = new PmxVertexData[Data.VertexArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = DeepCopy(Data.VertexArray[i]);
                result[i].Pos.X = result[i].Pos.X * VertexScale.x * property.VertexScale.x + property.VertexOffset.x;
                result[i].Pos.Y = result[i].Pos.Y * VertexScale.y * property.VertexScale.y + property.VertexOffset.y;
                result[i].Pos.Z = result[i].Pos.Z * VertexScale.z * property.VertexScale.z + property.VertexOffset.z;
            }
            return result;
        }

        public override int[] GetVertexIndices(ShotProperty property)
        {
            var result = new int[this.Data.VertexIndices.Length];
            Array.Copy(this.Data.VertexIndices, result, this.Data.VertexIndices.Length);

            return result;
        }

        public override PmxMaterialData[] GetMaterials(ShotProperty property)
        {
            PmxMaterialData[] result = new PmxMaterialData[this.Data.MaterialArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = DeepCopy(this.Data.MaterialArray[i]);
            }
            InitMaterials(property, result);
            return result;
        }

        public override String[] GetTextures(ShotProperty property)
        {
            String[] result = new String[this.Data.TextureFiles.Length];
            Array.Copy(this.Data.TextureFiles, result, this.Data.TextureFiles.Length);

            return result;
        }

        public override PmxBoneData[] GetBones(ShotProperty property)
        {
            PmxBoneData[] result = new PmxBoneData[this.Data.BoneArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = DeepCopy(this.Data.BoneArray[i]);
            }
            return result;
        }

        public static T DeepCopy<T>(T target)
        {
            T result;
            BinaryFormatter b = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();

            try
            {
                b.Serialize(mem, target);
                mem.Position = 0;
                result = (T)b.Deserialize(mem);
            }
            finally
            {
                mem.Close();
            }

            return result;
        }
    }
}
