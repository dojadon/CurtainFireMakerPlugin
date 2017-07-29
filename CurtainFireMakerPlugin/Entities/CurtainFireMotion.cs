using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Collections;
using CsVmd.Data;
using CsPmx.Data;

namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireMotion
    {
        private List<VmdMotionFrameData> motionList = new List<VmdMotionFrameData>();
        public MultiDictionary<PmxMorphData, VmdMorphFrameData> MorphDict { get; } = new MultiDictionary<PmxMorphData, VmdMorphFrameData>();

        public void AddVmdMotion(VmdMotionFrameData motion)
        {
            if (!motionList.Exists(m => m == null || m.BoneName.Equals(motion.BoneName) && m.KeyFrameNo == motion.KeyFrameNo))
            {
                motionList.Add(motion);
            }
        }

        public void AddVmdMorph(VmdMorphFrameData frameData, PmxMorphData morph)
        {
            if (!MorphDict[morph].Exists(m => m.MorphName == frameData.MorphName && m.KeyFrameNo == frameData.KeyFrameNo))
            {
                MorphDict[morph].Add(frameData);
            }
        }

        public void Finish(CurtainFireModel pmxModel)
        {
            var removeList = new List<PmxMorphData>();

            foreach (var morph in MorphDict.Keys)
            {
                if (!pmxModel.MorphList.Contains(morph))
                {
                    removeList.Add(morph);
                }
            }

            foreach (var key in removeList)
            {
                MorphDict.Remove(key);
            }
        }

        public void GetData(VmdMotionData data)
        {
            data.MotionArray = this.motionList.ToArray();

            var list = new List<VmdMorphFrameData>();
            foreach (var value in MorphDict.Values)
            {
                list.AddRange(value);
            }
            data.MorphArray = list.ToArray();
        }
    }
}
