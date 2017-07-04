using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Collections;

namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireMotion
    {
        private MultiDictionary<string, MotionFrameData> motionMap = new MultiDictionary<string, MotionFrameData>();
        private MultiDictionary<string, MorphFrameData> morphMap = new MultiDictionary<string, MorphFrameData>();

        public void AddVmdMotion(string boneName, MotionFrameData motion)
        {
            this.AddVmdMotion(boneName, motion, false);
        }

        public void AddVmdMotion(string boneName, MotionFrameData motion, bool replace)
        {
            var list = this.motionMap[boneName];

            if (replace)
            {
                list.RemoveAll(m => m.FrameNumber == motion.FrameNumber);
                list.Add(motion);
            }
            else
            {
                if (list.Exists(m => m.FrameNumber == motion.FrameNumber))
                {
                    list.Add(motion);
                }
            }
        }

        public void AddVmdMorph(string morphName, MorphFrameData morph)
        {
            var list = this.morphMap[morphName];

            list.RemoveAll(m => m.FrameNumber == morph.FrameNumber);
            list.Add(morph);
        }
    }
}
