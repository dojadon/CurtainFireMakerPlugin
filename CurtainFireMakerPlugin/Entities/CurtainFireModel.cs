using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;
using CsPmx;
using CsVmd.Data;
using CurtainFireMakerPlugin.Collections;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireModel
    {
        public List<int> IndexList { get; } = new List<int>();
        public List<PmxVertexData> VertexList { get; } = new List<PmxVertexData>();
        public List<PmxMaterialData> MaterialList { get; } = new List<PmxMaterialData>();
        public List<PmxMorphData> MorphList { get; } = new List<PmxMorphData>();
        public List<PmxBoneData> BoneList { get; } = new List<PmxBoneData>();
        public List<string> TextureList { get; } = new List<string>();

        public List<ShotModelData> ModelDataList { get; } = new List<ShotModelData>();

        private World World { get; }

        public CurtainFireModel(World world)
        {
            World = world;

            PmxBoneData centerBone = new PmxBoneData()
            {
                BoneName = "センター",
                ParentId = -1,
                Flag = 0x0002 | 0x0004 | 0x0008 | 0x0010
            };
            this.BoneList.Add(centerBone);
        }

        public void InitShotModelData(ShotModelData data)
        {
            if (data.Property.Type.RecordMotion)
            {
                if (data.Property.Type.HasMmdData)
                {
                    this.SetupShotModelData(data);
                }
                else
                {
                    this.SetupBone(data, data.Bones[0]);
                }
            }
        }

        private void SetupShotModelData(ShotModelData data)
        {
            int[] indices = Array.ConvertAll(data.Indices, i => i + this.VertexList.Count);
            this.IndexList.AddRange(indices);

            PmxVertexData[] vertices = data.Vertices;
            foreach (var vertex in vertices)
            {
                vertex.BoneId = Array.ConvertAll(vertex.BoneId, i => i + this.BoneList.Count);
                this.VertexList.Add(vertex);
            }

            string[] textures = data.Textures;
            foreach (var texture in textures)
            {
                if (!this.TextureList.Contains(texture))
                {
                    this.TextureList.Add(texture);
                }
            }

            PmxMaterialData[] materials = data.Materials;
            PmxMorphData morph = data.MaterialMorph;
            morph.MorphName = data.Property.Type.Name + "_MOR" + this.MorphList.Count.ToString();
            morph.Type = 4;
            morph.MorphArray = ArrayUtil.Set(new PmxMorphMaterialData[materials.Length], i => new PmxMorphMaterialData());

            for (int i = 0; i < materials.Length; i++)
            {
                morph.MorphArray[i].Index = this.MaterialList.Count + i;
            }
            this.MorphList.Add(morph);

            foreach (PmxMaterialData material in materials)
            {
                material.MaterialName = data.Property.Type.Name + "_MAT" + MaterialList.Count.ToString();
                material.TextureId = textures.Length > 0 && material.TextureId >= 0 ? TextureList.IndexOf(textures[material.TextureId]) : -1;
                MaterialList.Add(material);
            }
            SetupBone(data, data.Bones);

            ModelDataList.Add(data);
        }

        private void SetupBone(ShotModelData data, params PmxBoneData[] bones)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                PmxBoneData bone = bones[i];

                bone.BoneName = data.Property.Type.Name + (this.BoneList.Count - 1).ToString();
                bone.Flag = 0x0002 | 0x0004 | 0x0010;
                bone.BoneId = this.BoneList.Count + i;

                if (-1 < bone.ParentId && bone.ParentId < bones.Length)
                {
                    bone.ParentId = this.BoneList.IndexOf(bones[bone.ParentId]);
                }
                else
                {
                    bone.ParentId = 0;
                }
                this.BoneList.Add(bone);
            }
        }

        public void CompressMorph()
        {
            CurtainFireMotion vmdMotion = World.VmdMotion;

            var typeMorphDict = new MultiDictionary<byte, PmxMorphData>();
            foreach (var morph in vmdMotion.MorphDict.Keys)
            {
                typeMorphDict.Add(morph.Type, morph);
            }

            foreach (var morphList in typeMorphDict.Values)
            {
                this.Compress(morphList, vmdMotion.MorphDict);
            }
        }

        private void Compress(List<PmxMorphData> morphList, MultiDictionary<PmxMorphData, VmdMorphFrameData> frameDataDict)
        {
            var dict = new MultiDictionary<int[], PmxMorphData>(new IntegerArrayComparer());

            foreach (var morph in morphList)
            {
                dict.Add(Array.ConvertAll(frameDataDict[morph].ToArray(), m => m.KeyFrameNo), morph);
            }

            foreach (var key in dict.Keys)
            {
                var removeList = dict[key];

                if (removeList.Count > 1)
                {
                    removeList[0].MorphArray = Compress(removeList);

                    for (int i = 1; i < removeList.Count; i++)
                    {
                        var morph = removeList[i];

                        MorphList.Remove(morph);
                        World.VmdMotion.MorphDict.Remove(morph);
                        World.FxEffect.Effect.MorphControlObjectList.RemoveAll(m => m.MorphName == morph.MorphName);

                        foreach (var pass in World.FxEffect.Effect.DrawObjectPassList.FindAll(p => p.MorphName == morph.MorphName))
                        {
                            pass.MorphName = removeList[0].MorphName;
                        }
                    }
                }
            }

            IPmxMorphTypeData[] Compress(List<PmxMorphData> list)
            {
                var morphTypeDataList = new List<IPmxMorphTypeData>();
                foreach (var morph in list)
                {
                    morphTypeDataList.AddRange(morph.MorphArray);
                }

                return morphTypeDataList.ToArray();
            }
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
                Indices = Enumerable.Range(0, this.BoneList.Count).ToArray()
            };

            var morphSlot = new PmxSlotData()
            {
                SlotName = "弾モーフ",
                Type = PmxSlotData.SLOT_TYPE_MORPH,
                Indices = Enumerable.Range(0, this.MorphList.Count).ToArray()
            };

            data.Header = header;
            data.VertexIndices = this.IndexList.ToArray();
            data.TextureFiles = this.TextureList.ToArray();
            data.VertexArray = this.VertexList.ToArray();
            data.MaterialArray = this.MaterialList.ToArray();
            data.BoneArray = this.BoneList.ToArray();
            data.MorphArray = this.MorphList.ToArray();
            data.SlotArray = new PmxSlotData[] { boneSlot, morphSlot };
        }

        private class IntegerArrayComparer : IEqualityComparer<int[]>
        {
            public bool Equals(int[] x, int[] y)
            {
                if (x.Length != y.Length)
                {
                    return false;
                }
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] != y[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(int[] obj)
            {
                int result = 17;
                for (int i = 0; i < obj.Length; i++)
                {
                    unchecked
                    {
                        result = result * 23 + obj[i];
                    }
                }
                return result;
            }
        }
    }
}
