using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsMmdDataIO.Pmx;

namespace CurtainFireMakerPlugin.Entities
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
                Flag = BoneFlags.ROTATE | BoneFlags.MOVE | BoneFlags.VISIBLE | BoneFlags.OP,
            };
            BoneList.Add(centerBone);
        }

        public void SetupBone(ShotModelData data, params PmxBoneData[] bones)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                PmxBoneData bone = bones[i];

                bone.BoneName = "B" + (BoneList.Count - 1);
                bone.Flag = BoneFlags.ROTATE | BoneFlags.MOVE | BoneFlags.OP;
                bone.BoneId = BoneList.Count;

                if (-1 < bone.ParentId && bone.ParentId < bones.Length)
                {
                    bone.ParentId = bones[bone.ParentId].BoneId;
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
