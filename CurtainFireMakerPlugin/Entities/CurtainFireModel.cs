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

    public class CurtainFireModel
    {
        private PmxHeaderData Header { get; }
        public ModelVertexCollection Vertices { get; }
        public ModelMaterialCollection Materials { get; }
        public ModelMorphCollection Morphs { get; }
        public ModelBoneCollection Bones { get; }

        public PmxModelData ModelData { get; private set; }

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
                NumberOfExtraUv = 2,
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
            }
            Bones.SetupBone(data.Bones);

            data.IsInitialized = true;
        }

        public void AddMorph(PmxMorphData morph)
        {
            Morphs.MorphList.Add(morph);
        }

        public void FinalizeModel(IEnumerable<VmdMorphFrameData> morphFrames)
        {
            var vertices = new List<PmxVertexData>();
            var vertexIndices = new List<int>();
            var materials = new List<PmxMaterialData>();
            var textures = ModelDataEachPropertyDict.Keys.SelectMany(p => p.Type.CreateTextures(World, p)).Distinct().ToArray();
            var morphs = new List<PmxMorphData>();

            foreach (var prop in ModelDataEachPropertyDict.Keys)
            {
                Vertices.CreateVertices(prop.Type, ModelDataEachPropertyDict[prop], vertices.Count, out var propVertices, out var propVertexIndeices);
                Materials.CreateMaterials(prop, textures, ModelDataEachPropertyDict[prop].Count, out var propMaterials);
                Morphs.CompressMorph(ModelDataEachPropertyDict[prop], morphFrames, out var propMorphs);

                vertices.AddRange(propVertices);
                vertexIndices.AddRange(propVertexIndeices);
                materials.AddRange(propMaterials);
                morphs.AddRange(propMorphs);
            }

            ModelData = new PmxModelData
            {
                Header = Header,
                VertexIndices = vertexIndices.ToArray(),
                BoneArray = Bones.BoneArray,
                TextureFiles = textures.ToArray(),
                VertexArray = vertices.ToArray(),
                MaterialArray = materials.ToArray(),
                MorphArray = morphs.ToArray(),
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
                    Indices = Enumerable.Range(0,Math.Min(128,  morphs.Count)).ToArray(),
                    NormalSlot = false,
                },
                new PmxSlotData
                {
                    SlotName = "Time",
                    Type = SlotType.BONE,
                    Indices = new int[]{ 1 },
                },
                //new PmxSlotData
                //{
                //    SlotName = "弾ボーン",
                //    Type = SlotType.BONE,
                //    Indices = Enumerable.Range(1, Bones.BoneList.Count - 1).ToArray()
                //}
                },
            };
        }

        public void Export(dynamic script, string path, string name, string description)
        {
            Header.ModelName = name;
            Header.Description = description;

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                ModelData.Write(new BinaryWriter(stream));
                script.output_pmx_log(ModelData);
            }
        }

        public bool ShouldDrop(dynamic script)
        {
            return script.should_drop_pmxfile(ModelData);
        }
    }
}
