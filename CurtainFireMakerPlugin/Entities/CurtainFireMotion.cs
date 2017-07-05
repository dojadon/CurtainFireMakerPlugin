using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Collections;
using CsVmd.Data;

namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireMotion
    {
        private List<VmdMotionFrameData> motionList = new List<VmdMotionFrameData>();
        private List<VmdMorphFrameData> morphList = new List<VmdMorphFrameData>();

        public void AddVmdMotion(VmdMotionFrameData motion, bool replace = false)
        {
            if (replace)
            {
                motionList.RemoveAll(m => m == null || m.boneName.Equals(motion.boneName) && m.keyFrameNo == motion.keyFrameNo);
                motionList.Add(motion);
            }
            else
            {
                if (!motionList.Exists(m => m == null || m.boneName.Equals(motion.boneName) && m.keyFrameNo == motion.keyFrameNo))
                {
                    motionList.Add(motion);
                }
            }
        }

        public void AddVmdMorph(VmdMorphFrameData morph)
        {
            morphList.RemoveAll(m => m == null || m.morphName.Equals(morph.morphName) && m.keyFrameNo == morph.keyFrameNo);
            morphList.Add(morph);
        }

        public void GetData(VmdMotionData data)
        {
            data.Header.modelName = "弾幕";

            data.MotionArray = this.motionList.ToArray();
            data.MorphArray = this.morphList.ToArray();
        }
    }
}
