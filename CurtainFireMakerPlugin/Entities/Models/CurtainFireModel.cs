using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsMmdDataIO.Pmx.Data;
using CsMmdDataIO.Pmx;
using CsMmdDataIO.Vmd.Data;

namespace CurtainFireMakerPlugin.Entities.Models
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
                SetupShotModelData(data);
            }
            else
            {
                Bones.SetupBone(data, data.Bones[0]);
            }
        }

        private void SetupShotModelData(ShotModelData data)
        {
            Vertices.SetupVertices(data.Vertices, data.Indices, from m in data.Materials select m.FaceCount, Bones.BoneList.Count);

            Morphs.SetupMaterialMorph(data.Property, data.MaterialMorph, Materials.MaterialList.Count, data.Materials.Length);

            Materials.SetupMaterials(data.Property, data.Materials, data.Textures);

            Bones.SetupBone(data, data.Bones);
        }

        public void FinalizeModel()
        {
            Morphs.CompressMorph(Materials, Vertices);
        }

        public void GetData(PmxModelData data)
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

            data.Header = header;
            data.VertexIndices = Vertices.IndexList.ToArray();
            data.TextureFiles = Materials.TextureList.ToArray();
            data.VertexArray = Vertices.VertexList.ToArray();
            data.MaterialArray = Materials.MaterialList.ToArray();
            data.BoneArray = Bones.BoneList.ToArray();
            data.MorphArray = Morphs.MorphList.ToArray();
            data.SlotArray = new PmxSlotData[] { boneSlot, morphSlot };
        }

        public void Export()
        {
            string exportPath = World.PmxExportPath;
            File.Delete(exportPath);

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                var exporter = new PmxExporter(stream);

                var data = new PmxModelData();
                GetData(data);

                exporter.Export(data);

                Console.WriteLine("出力完了 : " + World.ExportFileName);
                Console.WriteLine("頂点数 : " + String.Format("{0:#,0}", data.VertexArray.Length));
                Console.WriteLine("面数 : " + String.Format("{0:#,0}", data.VertexIndices.Length / 3));
                Console.WriteLine("材質数 : " + String.Format("{0:#,0}", data.MaterialArray.Length));
                Console.WriteLine("ボーン数 : " + String.Format("{0:#,0}", data.BoneArray.Length));
                Console.WriteLine("モーフ数 : " + String.Format("{0:#,0}", data.MorphArray.Length));
            }
        }
    }
}
