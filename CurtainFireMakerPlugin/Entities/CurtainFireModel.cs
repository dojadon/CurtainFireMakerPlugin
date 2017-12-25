﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsMmdDataIO.Pmx;
using CsMmdDataIO.Vmd;

namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireModel
    {
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
            Materials.SetupMaterials(data.Materials, data.Textures);
        }

        public void FinalizeModel(IEnumerable<VmdMorphFrameData> morphFrames)
        {
            Materials.CompressMaterial(Vertices.Indices);
            Morphs.CompressMorph(morphFrames);
        }

        public PmxModelData CreatePmxModelData()
        {
            var header = new PmxHeaderData
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

            var centerBoneSlot = new PmxSlotData
            {
                SlotName = "センター",
                Type = SlotType.BONE,
                Indices = new[] { 0 },
            };

            var boneSlot = new PmxSlotData
            {
                SlotName = "弾ボーン",
                Type = SlotType.BONE,
                Indices = Enumerable.Range(1, Bones.BoneList.Count - 1).ToArray()
            };

            var morphSlot = new PmxSlotData
            {
                SlotName = "弾モーフ",
                Type = SlotType.MORPH,
                Indices = Enumerable.Range(0, Morphs.MorphList.Count).ToArray()
            };

            return new PmxModelData
            {
                Header = header,
                VertexIndices = Vertices.Indices.ToArray(),
                TextureFiles = Materials.TextureList.ToArray(),
                VertexArray = Vertices.VertexList.ToArray(),
                MaterialArray = Materials.MaterialList.ToArray(),
                BoneArray = Bones.BoneList.ToArray(),
                MorphArray = Morphs.MorphList.ToArray(),
                SlotArray = new PmxSlotData[] { centerBoneSlot, boneSlot, morphSlot },
            };
        }

        public void Export()
        {
            string exportPath = World.PmxExportPath;

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                var data = CreatePmxModelData();
                data.Write(new BinaryWriter(stream));

                World.Plugin.Script.output_pmx_log(data);
            }
        }
    }
}
