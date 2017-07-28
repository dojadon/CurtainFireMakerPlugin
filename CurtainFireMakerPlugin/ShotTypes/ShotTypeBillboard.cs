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
                e.World.FxEffect.AddMaterialIndices(e.World.PmxModel.MaterialList.IndexOf(e.ModelData.Materials[0]));
            };
        }

        public override PmxBoneData[] CreateBones() => new PmxBoneData[] { new PmxBoneData() { ParentId = -1 } };

        public override PmxMaterialData[] CreateMaterials() => new PmxMaterialData[]{new PmxMaterialData()
        {
            Diffuse = new Vector4(1, 1, 1, 1),
            FaceCount = 3
        }};

        public override string[] CreateTextures() => new string[0];

        public override int[] CreateVertexIndices() => new int[] { 0, 1, 2 };

        public override PmxVertexData[] CreateVertices()
        {
            var v1 = new PmxVertexData()
            {
                Pos = new Vector3(0.001F, 0, 0),
                Normal = new Vector3(0, 0, 1),
                BoneId = new int[] { 0 },
                Weight = new float[] { 1 }
            };

            var v2 = new PmxVertexData()
            {
                Pos = new Vector3(0.001F, 0.001F, 0),
                Normal = new Vector3(0, 0, 1),
                BoneId = new int[] { 0 },
                Weight = new float[] { 1 }
            };

            var v3 = new PmxVertexData()
            {
                Pos = new Vector3(0F, 0.001F, 0),
                Normal = new Vector3(0, 0, 1),
                BoneId = new int[] { 0 },
                Weight = new float[] { 1 }
            };

            return new PmxVertexData[] { v1, v2, v3 };
        }
    }
}
