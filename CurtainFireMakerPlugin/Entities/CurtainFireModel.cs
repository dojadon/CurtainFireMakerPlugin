using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MMDataIO.Pmx;
using MMDataIO.Vmd;

namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireModel
    {
        private PmxHeaderData Header { get; }
        public ModelVertexCollection Vertices { get; }
        public ModelMaterialCollection Materials { get; }
        public ModelMorphCollection Morphs { get; }
        public ModelBoneCollection Bones { get; }

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
                BoneIndexSize = 2,
                MorphIndexSize = 1,
                RigidIndexSize = 1,
            };
        }

        public void InitShotModelData(ShotModelData data)
        {
            if (data.Property.Type.HasMesh)
            {
                SetupMeshData(data);
            }
            Bones.SetupBone(data.Bones);
        }

        private void SetupMeshData(ShotModelData data)
        {
            Vertices.SetupVertices(data.Vertices, data.Indices, Bones.BoneList.Count);
            Materials.SetupMaterials(data.Property, data.Materials, data.Textures);
        }

        public void FinalizeModel(IEnumerable<VmdMorphFrameData> morphFrames)
        {
            Materials.FinalizeTextures();

            Materials.CompressMaterial(Vertices.Indices);
            Morphs.CompressMorph(morphFrames);
        }

        public PmxModelData CreatePmxModelData()
        {
            return new PmxModelData
            {
                Header = Header,
                VertexIndices = Vertices.VertexIndexArray,
                TextureFiles = Materials.TextureArray,
                VertexArray = Vertices.VertexArray,
                MaterialArray = Materials.MaterialArray,
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
                var data = CreatePmxModelData();
                data.Write(new BinaryWriter(stream));

                World.Script.output_pmx_log(data);
            }
        }
    }
}
