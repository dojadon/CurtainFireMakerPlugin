using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsPmx.Data;

namespace CurtainFireMakerPlugin.Entities.Models
{
    public class ModelBoneCollection
    {
        public List<PmxBoneData> BoneList { get; } = new List<PmxBoneData>();
        public World World { get; }

        public ModelBoneCollection(World world)
        {
            World = world;

            PmxBoneData centerBone = new PmxBoneData()
            {
                BoneName = "センター",
                ParentId = -1,
                Flag = 0x0002 | 0x0004 | 0x0008 | 0x0010
            };
            BoneList.Add(centerBone);
        }

        public void SetupBone(ShotModelData data, params PmxBoneData[] bones)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                PmxBoneData bone = bones[i];

                bone.BoneName = data.Property.Type.Name[0] + (BoneList.Count - 1).ToString();
                bone.Flag = 0x0002 | 0x0004 | 0x0010;
                bone.BoneId = BoneList.Count + i;

                if (-1 < bone.ParentId && bone.ParentId < bones.Length)
                {
                    bone.ParentId = BoneList.IndexOf(bones[bone.ParentId]);
                }
                else
                {
                    bone.ParentId = 0;
                }
                BoneList.Add(bone);
            }
        }
    }
}
