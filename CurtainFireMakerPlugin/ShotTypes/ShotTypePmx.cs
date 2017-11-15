using System;
using CsMmdDataIO.Pmx;
using CsMmdDataIO.Pmx.Data;
using CurtainFireMakerPlugin.Entities;
using VecMath;
using System.IO;
using CsMmdDataIO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public class ShotTypePmx : ShotType
    {
        private PmxModelData Data { get; } = new PmxModelData();

        public ShotTypePmx(string name, string path, float size) : this(name, path, new Vector3(size, size, size)) { }

        public ShotTypePmx(string name, string path, Vector3 size) : base(name)
        {
            path = Plugin.Instance.Config.ResourceDirPath + "\\" + path;
            var inStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (inStream)
            {
                PmxParser parser = new PmxParser(inStream);
                parser.Parse(Data);
            }

            for (int i = 0; i < Data.VertexArray.Length; i++)
            {
                var vertex = Data.VertexArray[i];
                vertex.Pos = Vector3.Scale(vertex.Pos, size);
            }
        }

        public override bool HasMesh => true;

        public override PmxVertexData[] CreateVertices(World wolrd, ShotProperty prop)
        {
            PmxVertexData[] result = new PmxVertexData[Data.VertexArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = CloneUtil.Clone(Data.VertexArray[i]);
            }
            return result;
        }

        public override int[] CreateVertexIndices(World wolrd, ShotProperty prop)
        {
            //int[] result = new int[Data.VertexIndices.Length];
            //Array.Copy(Data.VertexIndices, result, Data.VertexIndices.Length);

            return Data.VertexIndices;
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
            //string[] result = new string[Data.TextureFiles.Length];
            //Array.Copy(Data.TextureFiles, result, Data.TextureFiles.Length);

            return Data.TextureFiles;
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
