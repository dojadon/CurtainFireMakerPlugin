using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using CsMmdDataIO.Pmx;
using CsMmdDataIO.Vmd;

namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireMotion
    {
        public Dictionary<(string, int), VmdMotionFrameData> BoneFrameDict { get; } = new Dictionary<(string, int), VmdMotionFrameData>();
        private Dictionary<(string, int), int> BoneFramePriorityDict { get; } = new Dictionary<(string, int), int>();

        public Dictionary<(string, int), VmdMorphFrameData> MorphFrameDict { get; } = new Dictionary<(string, int), VmdMorphFrameData>();
        private Dictionary<(string, int), int> MorphFramePriorityDict { get; } = new Dictionary<(string, int), int>();

        public List<VmdPropertyFrameData> PropertyFrames { get; }

        private World World { get; }

        public CurtainFireMotion(World world)
        {
            World = world;

            PropertyFrames = new List<VmdPropertyFrameData>();
        }

        public void AddBoneKeyFrame(PmxBoneData bone, VmdMotionFrameData frame, int priority)
        {
            (string, int) key = (frame.Name, frame.FrameTime);

            if (BoneFrameDict.ContainsKey(key))
            {
                Console.WriteLine("{0}, {1}", BoneFrameDict[key].FrameTime, frame.FrameTime);
            }

            if (frame.FrameTime >= 0 && (!BoneFramePriorityDict.ContainsKey(key) || (BoneFramePriorityDict[key] <= priority)))
            {
                BoneFrameDict[key] = frame;
                BoneFramePriorityDict[key] = priority;
            }
        }

        public void AddMorphKeyFrame(PmxMorphData morph, VmdMorphFrameData frame, int priority)
        {
            (string, int) key = (frame.Name, frame.FrameTime);

            if (frame.FrameTime >= 0 && (!MorphFramePriorityDict.ContainsKey(key) || (MorphFramePriorityDict[key] <= priority)))
            {
                MorphFrameDict[key] = frame;
                MorphFramePriorityDict[key] = priority;
            }
        }

        public void AddPropertyKeyFrame(VmdPropertyFrameData frame)
        {
            if (frame.FrameTime >= 0)
            {
                PropertyFrames.Add(frame);
            }
        }

        public int GetKeyFrameHashCode<T>(T obj) where T : IElementKeyFrame
        {
            int result = 17;
            result = result * 23 + obj.FrameTime;
            result = result * 23 + obj.Name.GetHashCode();
            return result;
        }

        public void FinalizeKeyFrame(IEnumerable<PmxMorphData> morphs)
        {
            var morphNames = morphs.Select(m => m.MorphName);
            foreach (var key in MorphFrameDict.Keys)
            {
                if (!morphNames.Contains(key.Item1)) MorphFrameDict.Remove(key);
            }
        }

        private VmdMotionData CreateVmdMotionData()
        {
            return new VmdMotionData
            {
                Header = new VmdHeaderData { ModelName = World.ModelName },
                MotionFrameArray = BoneFrameDict.Values.ToArray(),
                MorphFrameArray = MorphFrameDict.Values.ToArray(),
                PropertyFrameArray = PropertyFrames.ToArray(),
            };
        }

        public void Export()
        {
            string exportPath = World.VmdExportPath;

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                var data = CreateVmdMotionData();
                VmdExporter.Export(data, stream);
            }
        }
    }
}
