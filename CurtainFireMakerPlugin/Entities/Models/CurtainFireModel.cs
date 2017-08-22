using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsPmx.Data;
using CsPmx;
using CsVmd.Data;

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
            Vertices.SetupVertices(data.Vertices, data.Indices, Array.ConvertAll(data.Materials, m => m.FaceCount), Bones.BoneList.Count);

            Morphs.SetupMaterialMorph(data.MaterialMorph, Materials.MaterialList.Count, data.Materials.Length);

            Materials.SetupMaterials(data.Materials, data.Textures);

            Bones.SetupBone(data, data.Bones);
        }

        public void Finish()
        {
            Morphs.CompressMorph(Materials);
        }

        public void GetData(PmxModelData data)
        {
            var header = new PmxHeaderData()
            {
                Version = 2.0F
            };

            var boneSlot = new PmxSlotData()
            {
                SlotName = "弾ボーン",
                Type = PmxSlotData.SLOT_TYPE_BONE,
                Indices = Enumerable.Range(1, Bones.BoneList.Count - 1).ToArray()
            };

            var morphSlot = new PmxSlotData()
            {
                SlotName = "弾モーフ",
                Type = PmxSlotData.SLOT_TYPE_MORPH,
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

        public void Export(World world)
        {
            var config = Plugin.Instance.Config;
            string exportPath = config.ExportDirPath + "\\" + world.ExportFileName + ".pmx";
            File.Delete(exportPath);

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                var exporter = new PmxExporter(stream);

                var data = new PmxModelData();
                GetData(data);

                data.Header.ModelName = config.ModelName;
                data.Header.Description += config.ModelDescription;

                exporter.Export(data);

                Console.WriteLine("出力完了 : " + world.ExportFileName);
                Console.WriteLine("頂点数 : " + String.Format("{0:#,0}", data.VertexArray.Length));
                Console.WriteLine("面数 : " + String.Format("{0:#,0}", data.VertexIndices.Length / 3));
                Console.WriteLine("材質数 : " + String.Format("{0:#,0}", data.MaterialArray.Length));
                Console.WriteLine("ボーン数 : " + String.Format("{0:#,0}", data.BoneArray.Length));
                Console.WriteLine("モーフ数 : " + String.Format("{0:#,0}", data.MorphArray.Length));
            }
        }
    }
}
