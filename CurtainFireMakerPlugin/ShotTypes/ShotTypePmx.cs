using System;
using CsPmx;
using CsPmx.Data;
using CurtainFireMakerPlugin.Entities;
using VecMath;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public class ShotTypePmx : ShotType
    {
        private PmxModelData Data { get; } = new PmxModelData();
        private Vector3 VertexScale { get; }

        public ShotTypePmx(string name, string path, float size) : this(name, path, new Vector3(size, size, size)) { }

        public ShotTypePmx(string name, string path, Vector3 size) : base(name)
        {
            path = Plugin.Instance.Config.ResourceDirPath + path;
            var inStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (inStream)
            {
                PmxParser parser = new PmxParser(inStream);
                parser.Parse(this.Data);
            }

            VertexScale = size;
        }

        public override PmxVertexData[] CreateVertices()
        {
            PmxVertexData[] result = new PmxVertexData[Data.VertexArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = DeepCopy(Data.VertexArray[i]);
                result[i].Pos = Vector3.Scale(result[i].Pos, VertexScale);
            }
            return result;
        }

        public override int[] CreateVertexIndices()
        {
            var result = new int[this.Data.VertexIndices.Length];
            Array.Copy(this.Data.VertexIndices, result, this.Data.VertexIndices.Length);

            return result;
        }

        public override PmxMaterialData[] CreateMaterials()
        {
            PmxMaterialData[] result = new PmxMaterialData[this.Data.MaterialArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = DeepCopy(this.Data.MaterialArray[i]);
            }
            return result;
        }

        public override String[] CreateTextures()
        {
            String[] result = new String[this.Data.TextureFiles.Length];
            Array.Copy(this.Data.TextureFiles, result, this.Data.TextureFiles.Length);

            return result;
        }

        public override PmxBoneData[] CreateBones()
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

            using (MemoryStream mem = new MemoryStream())
            {
                b.Serialize(mem, target);
                mem.Position = 0;
                result = (T)b.Deserialize(mem);
            }

            return result;
        }
    }
}
