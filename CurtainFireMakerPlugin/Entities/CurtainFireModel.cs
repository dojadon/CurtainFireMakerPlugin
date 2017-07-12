using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;
using CsPmx;
using CurtainFireMakerPlugin.Mathematics;

namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireModel
    {
        public readonly List<int> indexList = new List<int>();
        public readonly List<PmxVertexData> vertexList = new List<PmxVertexData>();
        public readonly List<PmxMaterialData> materialList = new List<PmxMaterialData>();
        public readonly List<PmxMorphData> morphList = new List<PmxMorphData>();
        public readonly List<PmxBoneData> boneList = new List<PmxBoneData>();
        public readonly List<string> textureList = new List<string>();

        public CurtainFireModel()
        {
            PmxBoneData centerBone = new PmxBoneData();
            centerBone.boneName = "センター";
            centerBone.parentId = -1;
            centerBone.flag = 0x0002 | 0x0004 | 0x0008 | 0x0010;
            this.boneList.Add(centerBone);
        }

        public void InitShotModelData(ShotModelData data)
        {
            if (data.Property.Type.RecordMotion())
            {
                if (data.Property.Type.HasMmdData())
                {
                    this.SetupShotModelData(data);
                }
                else
                {
                    this.SetupBone(data.bones[0]);
                }
            }
        }

        private void SetupShotModelData(ShotModelData data)
        {
            int[] indices = Array.ConvertAll(data.indices, i => i + this.vertexList.Count);
            this.indexList.AddRange(indices);

            PmxVertexData[] vertices = data.vertices;
            foreach (var vertex in vertices)
            {
                vertex.boneId = Array.ConvertAll(vertex.boneId, i => i + this.boneList.Count);
                this.vertexList.Add(vertex);
            }

            string[] textures = data.textures;
            foreach (var texture in textures)
            {
                if (!this.textureList.Contains(texture))
                {
                    this.textureList.Add(texture);
                }
            }

            PmxMaterialData[] materials = data.materials;
            PmxMorphData morph = data.morph;
            morph.morphName = this.morphList.Count.ToString();
            morph.type = 4;
            morph.morphArray = ArrayUtil.Set(new PmxMorphMaterialData[materials.Length], i => new PmxMorphMaterialData());

            for (int i = 0; i < materials.Length; i++)
            {
                morph.morphArray[i].Index = this.materialList.Count + i;
            }
            this.morphList.Add(morph);

            foreach (PmxMaterialData material in materials)
            {
                material.materialName = this.materialList.Count.ToString();
                material.diffuse = new DxMath.Vector4(data.Property.Red, data.Property.Green, data.Property.Blue, 1.0F);
                material.ambient = new DxMath.Vector3(data.Property.Red, data.Property.Green, data.Property.Blue);
                material.textureId = textures.Length > 0 && material.textureId >= 0 ? this.textureList.IndexOf(textures[material.textureId]) : -1;
                this.materialList.Add(material);
            }
            this.SetupBone(data.bones);
        }

        private void SetupBone(params PmxBoneData[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                PmxBoneData bone = data[i];

                bone.boneName = (this.boneList.Count + i - 1).ToString();
                bone.flag = 0x0001 | 0x0002 | 0x0004 | 0x0008 | 0x0010;
                bone.boneId = this.boneList.Count + i;

                if (-1 < bone.parentId && bone.parentId < data.Length)
                {
                    bone.parentId = this.boneList.IndexOf(data[bone.parentId]);
                }
                else
                {
                    bone.parentId = 0;
                }
            }
            this.boneList.AddRange(data);
        }

        public void GetData(PmxModelData data)
        {
            var header = new PmxHeaderData();
            header.version = 2.0F;

            var boneSlot = new PmxSlotData();
            boneSlot.slotName = "弾ボーン";
            boneSlot.type = PmxSlotData.SLOT_TYPE_BONE;
            boneSlot.indices = Enumerable.Range(0, this.boneList.Count).ToArray();

            var morphSlot = new PmxSlotData();
            morphSlot.slotName = "弾モーフ";
            morphSlot.type = PmxSlotData.SLOT_TYPE_MORPH;
            morphSlot.indices = Enumerable.Range(0, this.morphList.Count).ToArray();

            data.Header = header;
            data.VertexIndices = this.indexList.ToArray();
            data.TextureFiles = this.textureList.ToArray();
            data.VertexArray = this.vertexList.ToArray();
            data.MaterialArray = this.materialList.ToArray();
            data.BoneArray = this.boneList.ToArray();
            data.MorphArray = this.morphList.ToArray();
            data.SlotArray = new PmxSlotData[] { boneSlot, morphSlot };

            Console.WriteLine(boneList.Count);
            Console.WriteLine(morphList.Count);
        }
    }
}
