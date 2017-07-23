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
        private PmxModelData data = new PmxModelData();
        private Vector3 Size { get; }

        public Action<ShotProperty, PmxMaterialData[]> InitMaterials { get; set; } = (prop, materials) =>
        {
            foreach (var material in materials)
            {
                material.Diffuse = new DxMath.Vector4(prop.Red, prop.Green, prop.Blue, 1.0F);
                material.Ambient = new DxMath.Vector3(prop.Red, prop.Green, prop.Blue);
            }
        };

        public ShotTypePmx(string name, string path, double size) : this(name, path, new Vector3(size, size, size)) { }

        public ShotTypePmx(string name, string path, Vector3 size) : base(name)
        {
            var inStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (inStream)
            {
                PmxParser parser = new PmxParser(inStream);
                parser.Parse(this.data);
            }

            Size = size;
        }

        public override PmxVertexData[] GetVertices(ShotProperty property)
        {
            PmxVertexData[] result = new PmxVertexData[this.data.VertexArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = DeepCopy(this.data.VertexArray[i]);
                result[i].Pos.X *= (float)(this.Size.x * property.Size.x);
                result[i].Pos.Y *= (float)(this.Size.y * property.Size.y);
                result[i].Pos.Z *= (float)(this.Size.z * property.Size.z);
            }
            return result;
        }

        public override int[] GetVertexIndices(ShotProperty property)
        {
            var result = new int[this.data.VertexIndices.Length];
            Array.Copy(this.data.VertexIndices, result, this.data.VertexIndices.Length);

            return result;
        }

        public override PmxMaterialData[] GetMaterials(ShotProperty property)
        {
            PmxMaterialData[] result = new PmxMaterialData[this.data.MaterialArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = DeepCopy(this.data.MaterialArray[i]);
            }
            InitMaterials(property, result);
            return result;
        }

        public override String[] GetTextures(ShotProperty property)
        {
            String[] result = new String[this.data.TextureFiles.Length];
            Array.Copy(this.data.TextureFiles, result, this.data.TextureFiles.Length);

            return result;
        }

        public override PmxBoneData[] GetBones(ShotProperty property)
        {
            PmxBoneData[] result = new PmxBoneData[this.data.BoneArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = DeepCopy(this.data.BoneArray[i]);
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
