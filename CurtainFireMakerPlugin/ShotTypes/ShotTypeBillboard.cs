using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;
using CurtainFireMakerPlugin.Entities;
using VecMath;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public class ShotTypeBillboard : ShotType
    {
        public string TexturePath { get; }

        public ShotTypeBillboard(string name, string texturePath) : base(name)
        {
            TexturePath = texturePath;

            Init = e =>
            {
                e.World.FxEffect.InitEntityShot(e, TexturePath);

                int[] indices = Array.ConvertAll(e.ModelData.Materials, m => e.World.PmxModel.MaterialList.IndexOf(m));
                e.World.FxEffect.AddMaterialIndices(indices);
            };
        }

        public override PmxBoneData[] CreateBones() => new PmxBoneData[] { new PmxBoneData() { ParentId = -1 } };

        public override PmxMaterialData[] CreateMaterials()
        {
            return new PmxMaterialData[]{new PmxMaterialData()
            {
                MaterialName = "Billboard",
                Diffuse = new Vector4(1, 1, 1, 1),
                FaceCount = 1
            }};
        }

        public override string[] CreateTextures() => new string[0];

        public override int[] CreateVertexIndices() => new int[] { 0, 1, 2 };

        public override PmxVertexData[] CreateVertices()
        {
            var v1 = new PmxVertexData()
            {
                Pos = new Vector3(0.1F, 0, 0),
                Normal = new Vector3(0, 0, 1),
                BoneId = new int[] { 0 },
                Weight = new float[] { 1 }
            };

            var v2 = new PmxVertexData()
            {
                Pos = new Vector3(0.1F, 0.1F, 0),
                Normal = new Vector3(0, 0, 1),
                BoneId = new int[] { 0 },
                Weight = new float[] { 1 }
            };

            var v3 = new PmxVertexData()
            {
                Pos = new Vector3(0F, 0.1F, 0),
                Normal = new Vector3(0, 0, 1),
                BoneId = new int[] { 0 },
                Weight = new float[] { 1 }
            };

            return new PmxVertexData[] { v1, v2, v3 };
        }
    }
}
