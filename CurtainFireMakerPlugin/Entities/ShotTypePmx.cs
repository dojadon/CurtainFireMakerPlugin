using System;
using MMDataIO.Pmx;
using VecMath;
using System.IO;
using MMDataIO;

namespace CurtainFireMakerPlugin.Entities
{
    public class ShotTypePmx : ShotType
    {
        private string PmxFilePath { get; }

        private PmxModelData Data { get; } = new PmxModelData();

        private DateTime LastWriteTime { get; set; } = DateTime.MinValue;

        private float VertexScale { get; }

        public ShotTypePmx(string name, string path, float scale) : base(name)
        {
            PmxFilePath = Configuration.ResourceDirPath + path;
            VertexScale = scale;

            ReadPmxData();
        }

        private void ReadPmxData()
        {
            if (LastWriteTime != (LastWriteTime = File.GetLastWriteTime(PmxFilePath)))
            {
                using (var stream = new FileStream(PmxFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    Data.Read(new BinaryReader(stream));
                }
            }
        }

        public override void InitWorld(World world)
        {
            base.InitWorld(world);

            ReadPmxData();
        }

        public override bool HasMesh => Data.VertexArray.Length > 0;

        public override PmxVertexData[] CreateVertices(World wolrd, ShotProperty prop)
        {
            PmxVertexData[] result = new PmxVertexData[Data.VertexArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var clone = result[i] = CloneUtil.Clone(Data.VertexArray[i]);
                clone.Pos = (Vector4)clone.Pos * VertexScale * prop.Scale;
            }
            return result;
        }

        public override int[] CreateVertexIndices(World wolrd, ShotProperty prop)
        {
            int[] result = new int[Data.VertexIndices.Length];
            Array.Copy(Data.VertexIndices, result, Data.VertexIndices.Length);

            return result;
        }

        public override PmxMaterialData[] CreateMaterials(World wolrd, ShotProperty prop)
        {
            PmxMaterialData[] result = new PmxMaterialData[Data.MaterialArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = CloneUtil.Clone(Data.MaterialArray[i]);
            }
            return result;
        }

        public override string[] CreateTextures(World wolrd, ShotProperty prop)
        {
            string[] result = new string[Data.TextureFiles.Length];
            Array.Copy(Data.TextureFiles, result, Data.TextureFiles.Length);

            return result;
        }

        public override PmxBoneData[] CreateBones(World wolrd, ShotProperty prop)
        {
            PmxBoneData[] result = new PmxBoneData[Data.BoneArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = CloneUtil.Clone(Data.BoneArray[i]);
            }
            return result;
        }
    }
}
