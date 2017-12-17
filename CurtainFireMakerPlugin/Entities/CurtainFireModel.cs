using System;
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
                SlotArray = new PmxSlotData[] { boneSlot, morphSlot },
            };
        }

        public void Export()
        {
            string exportPath = World.PmxExportPath;

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                var data = CreatePmxModelData();
                PmxExporter.Export(data, stream);

                Console.WriteLine("出力完了 : " + World.ExportFileName);
                Console.WriteLine("頂点数 : " + string.Format("{0:#,0}", data.VertexArray.Length));
                Console.WriteLine("面数 : " + string.Format("{0:#,0}", data.VertexIndices.Length / 3));
                Console.WriteLine("材質数 : " + string.Format("{0:#,0}", data.MaterialArray.Length));
                Console.WriteLine("テクスチャ数 : " + string.Format("{0:#,0}", data.TextureFiles.Length));
                Console.WriteLine("ボーン数 : " + string.Format("{0:#,0}", data.BoneArray.Length));
                Console.WriteLine("モーフ数 : " + string.Format("{0:#,0}", data.MorphArray.Length));
            }
        }
    }
}
