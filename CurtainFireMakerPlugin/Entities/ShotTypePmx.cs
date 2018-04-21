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

        public override PmxModelData OriginalData { get; } = new PmxModelData();

        private DateTime LastWriteTime { get; set; } = DateTime.MinValue;

        private float VertexScale { get; }

        public ShotTypePmx(string name, string path, float scale) : base(name)
        {
            PmxFilePath = Plugin.ResourceDirPath + path;
            VertexScale = scale;

            ReadPmxData();
        }

        private void ReadPmxData()
        {
            if (LastWriteTime != (LastWriteTime = File.GetLastWriteTime(PmxFilePath)))
            {
                using (var stream = new FileStream(PmxFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    OriginalData.Read(new BinaryReader(stream));
                }
                Array.ForEach(OriginalData.VertexArray, v => v.Pos *= VertexScale);
            }
        }

        public override void InitWorld(World world)
        {
            base.InitWorld(world);

            ReadPmxData();
        }

        public override bool HasMesh => OriginalData.VertexArray.Length > 0;

        public override PmxVertexData[] CreateVertices(World wolrd, ShotProperty prop)
        {
            PmxVertexData[] result = new PmxVertexData[OriginalData.VertexArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var clone = result[i] = CloneUtil.Clone(OriginalData.VertexArray[i]);
                clone.Pos = (Vector4)clone.Pos * prop.Scale;
            }
            return result;
        }

        public override int[] CreateVertexIndices(World wolrd, ShotProperty prop)
        {
            int[] result = new int[OriginalData.VertexIndices.Length];
            Array.Copy(OriginalData.VertexIndices, result, OriginalData.VertexIndices.Length);

            return result;
        }

        public override PmxMaterialData[] CreateMaterials(World wolrd, ShotProperty prop)
        {
            PmxMaterialData[] result = new PmxMaterialData[OriginalData.MaterialArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = CloneUtil.Clone(OriginalData.MaterialArray[i]);
            }
            return result;
        }

        public override string[] CreateTextures(World wolrd, ShotProperty prop)
        {
            string[] result = new string[OriginalData.TextureFiles.Length];
            Array.Copy(OriginalData.TextureFiles, result, OriginalData.TextureFiles.Length);

            return result;
        }

        public override PmxBoneData[] CreateBones(World wolrd, ShotProperty prop)
        {
            PmxBoneData[] result = new PmxBoneData[OriginalData.BoneArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = CloneUtil.Clone(OriginalData.BoneArray[i]);
            }
            return result;
        }
    }
}
