using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MMDataIO.Pmx;
using MMDataIO.Vmd;

namespace CurtainFireMakerPlugin.Entities
{
    class ShotPropertyComparer : IEqualityComparer<ShotProperty>
    {
        public bool Equals(ShotProperty x, ShotProperty y)
        {
            return x.Type == y.Type && x.Color == y.Color;
        }

        public int GetHashCode(ShotProperty obj)
        {
            int result = 17;
            result = result * 31 + obj.Type.Name.GetHashCode();
            result = result * 31 + obj.Color;
            return result;
        }
    }

    internal class CurtainFireModel
    {
        private PmxHeaderData Header { get; }
        public ModelVertexCollection Vertices { get; }
        public ModelMaterialCollection Materials { get; }
        public ModelMorphCollection Morphs { get; }
        public ModelBoneCollection Bones { get; }

        private PmxModelData ModelData { get; set; }

        public MultiDictionary<ShotProperty, ShotModelData> ModelDataEachPropertyDict { get; } = new MultiDictionary<ShotProperty, ShotModelData>(new ShotPropertyComparer());

        private World World { get; }

        public CurtainFireModel(World world)
        {
            World = world;

            Vertices = new ModelVertexCollection(World);
            Materials = new ModelMaterialCollection(World);
            Morphs = new ModelMorphCollection(World);
            Bones = new ModelBoneCollection(World);

            Header = new PmxHeaderData
            {
                Version = 2.0F,
                ModelName = World.ModelName,
                Description = World.ModelDescription,
                VertexIndexSize = 4,
                TextureIndexSize = 1,
                MaterialIndexSize = 1,
                BoneIndexSize = 4,
                MorphIndexSize = 1,
                RigidIndexSize = 1,
            };
        }

        public void InitShotModelData(ShotModelData data)
        {
            if (data.Property.Type.HasMesh)
            {
                data.BoneIndexOffset = Bones.BoneList.Count;
                ModelDataEachPropertyDict[data.Property].Add(data);

                bool PropertyEquals(ShotProperty prop)
                {
                    return prop.Type == data.Property.Type && prop.Color == data.Property.Color;
                }
            }
            Bones.SetupBone(data.Bones);

            data.IsInitialized = true;
        }

        public void FinalizeModel(IEnumerable<VmdMorphFrameData> morphFrames)
        {
            var vertices = new List<PmxVertexData>();
            var vertexIndices = new List<int>();
            var materials = new List<PmxMaterialData>();
            var textures = ModelDataEachPropertyDict.Keys.SelectMany(p => p.Type.CreateTextures(World, p)).Distinct().ToArray();

            foreach (var prop in ModelDataEachPropertyDict.Keys)
            {
                Vertices.CreateVertices(prop.Type, ModelDataEachPropertyDict[prop], vertices.Count, out var propVertices, out var propVertexIndeices);
                Materials.CreateMaterials(prop, textures, ModelDataEachPropertyDict[prop].Count, out var propMaterials);

                vertices.AddRange(propVertices);
                vertexIndices.AddRange(propVertexIndeices);
                materials.AddRange(propMaterials);
            }

            Morphs.CompressMorph(morphFrames);

            ModelData = new PmxModelData
            {
                Header = Header,
                VertexIndices = vertexIndices.ToArray(),
                TextureFiles = textures.ToArray(),
                VertexArray = vertices.ToArray(),
                MaterialArray = materials.ToArray(),
                BoneArray = Bones.BoneArray,
                MorphArray = Morphs.MorphArray,
                SlotArray = new[]
                {
                new PmxSlotData
                {
                    SlotName = "Root",
                    Type = SlotType.BONE,
                    Indices = new int[]{ 0 },
                    NormalSlot = false,
                },
                new PmxSlotData
                {
                    SlotName = "表情",
                    Type = SlotType.MORPH,
                    Indices =Enumerable.Range(0, Morphs.MorphList.Count).ToArray(),
                    NormalSlot = false,
                },
                new PmxSlotData
                {
                    SlotName = "弾ボーン",
                    Type = SlotType.BONE,
                    Indices = Enumerable.Range(1, Bones.BoneList.Count - 1).ToArray()
                }
                },
            };
        }

        public void Export()
        {
            using (var stream = new FileStream(World.PmxExportPath, FileMode.Create, FileAccess.Write))
            {
                ModelData.Write(new BinaryWriter(stream));
                World.Script.output_pmx_log(ModelData);
            }
        }
    }
}
