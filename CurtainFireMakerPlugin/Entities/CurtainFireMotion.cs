using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using MMDataIO.Pmx;
using MMDataIO.Vmd;

namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireMotion
    {
        public Dictionary<(string name, int time), (VmdMotionFrameData frame, int priority)> BoneFrameDict { get; }
        public Dictionary<(string name, int time), (VmdMorphFrameData frame, int priority)> MorphFrameDict { get; }
        public List<VmdPropertyFrameData> PropertyFrames { get; }

        private World World { get; }

        public CurtainFireMotion(World world)
        {
            World = world;

            BoneFrameDict = new Dictionary<(string name, int time), (VmdMotionFrameData frame, int priority)>();
            MorphFrameDict = new Dictionary<(string name, int time), (VmdMorphFrameData frame, int priority)>();
            PropertyFrames = new List<VmdPropertyFrameData>();
        }

        public void AddBoneKeyFrame(VmdMotionFrameData frame, int priority)
        {
            (string, int) key = (frame.Name, frame.FrameTime);

            if (frame.FrameTime >= 0 && (!BoneFrameDict.ContainsKey(key) || (BoneFrameDict[key].priority <= priority)))
            {
                BoneFrameDict[key] = (frame, priority);
            }
        }

        public void AddMorphKeyFrame(VmdMorphFrameData frame, int priority)
        {
            (string, int) key = (frame.Name, frame.FrameTime);

            if (frame.FrameTime >= 0 && (!MorphFrameDict.ContainsKey(key) || (MorphFrameDict[key].priority <= priority)))
            {
                MorphFrameDict[key] = (frame, priority);
            }
        }

        public void AddPropertyKeyFrame(VmdPropertyFrameData frame)
        {
            if (frame.FrameTime >= 0)
            {
                PropertyFrames.Add(frame);
            }
        }

        public void FinalizeKeyFrame(IEnumerable<PmxMorphData> morphs)
        {
            var morphNames = morphs.Select(m => m.MorphName);
            foreach (var key in MorphFrameDict.Keys.Where(k => !morphNames.Contains(k.name)).ToArray())
            {
                MorphFrameDict.Remove(key);
            }
        }

        private VmdMotionData CreateVmdMotionData(string name)
        {
            return new VmdMotionData
            {
                Header = new VmdHeaderData { ModelName = name },
                MotionFrameArray = BoneFrameDict.Values.Select(t => t.frame).ToArray(),
                MorphFrameArray = MorphFrameDict.Values.Select(t => t.frame).ToArray(),
                PropertyFrameArray = PropertyFrames.ToArray(),
            };
        }

        private VmdMotionData MotionData{ get; set; }

        public void Export(string path, string name)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                MotionData = CreateVmdMotionData(name);
                MotionData.Write(new BinaryWriter(stream));

                World.Script.output_vmd_log(MotionData);
            }
        }

        public bool ShouldDrop()
        {
            return World.Script.should_drop_vmdfile(MotionData);
        }
    }
}
