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

        public void SetupBone(params PmxBoneData[] bones)
        {
            foreach (var bone in bones)
            {
                bone.BoneName = "B" + (BoneList.Count - 1);
                bone.BoneId = BoneList.Count;
                bone.Flag = BoneFlags.ROTATE | BoneFlags.MOVE | BoneFlags.VISIBLE | BoneFlags.OP;

                if (-1 < bone.ParentId && bone.ParentId < bones.Length - 1)
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
