using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CurtainFireMakerPlugin.Collections;
using CsMmdDataIO.Pmx.Data;
using CsMmdDataIO.Vmd;
using CsMmdDataIO.Vmd.Data;

namespace CurtainFireMakerPlugin.Entities.Models
{
    internal class CurtainFireMotion
    {
        public MultiDictionary<PmxBoneData, VmdMotionFrameData> BoneFrameDict { get; }
        public MultiDictionary<PmxMorphData, VmdMorphFrameData> MorphFrameDict { get; }
        public List<VmdPropertyFrameData> PropertyFrameList { get; }

        private World World { get; }

        public CurtainFireMotion(World world)
        {
            World = world;

            BoneFrameDict = new MultiDictionary<PmxBoneData, VmdMotionFrameData>();
            MorphFrameDict = new MultiDictionary<PmxMorphData, VmdMorphFrameData>();
            PropertyFrameList = new List<VmdPropertyFrameData>();
        }

        public void AddBoneKeyFrame(PmxBoneData bone, VmdMotionFrameData frame)
        {
            if (frame.FrameTime >= 0)
            {
                BoneFrameDict[bone].Add(frame);
            }
        }

        public void AddMorphKeyFrame(PmxMorphData morph, VmdMorphFrameData frame)
        {
            if (frame.FrameTime >= 0)
            {
                MorphFrameDict[morph].Add(frame);
            }
        }

        public void AddPropertyKeyFrame(VmdPropertyFrameData frame)
        {
            if (frame.FrameTime >= 0)
            {
                PropertyFrameList.Add(frame);
            }
        }

        public void Finish()
        {
            foreach (var key in MorphFrameDict.Keys)
            {
                if (!World.PmxModel.Morphs.MorphList.Contains(key)) MorphFrameDict[key].Clear();
            }

            foreach (var value in BoneFrameDict.Values)
            {
                DistinctFrames(value);
            }

            foreach (var value in MorphFrameDict.Values)
            {
                DistinctFrames(value);
            }

            DistinctFrames(PropertyFrameList);
        }

        private void DistinctFrames<T>(IList<T> frames) where T : IKeyFrame
        {
            var frameNums = new HashSet<long>();
            var removeList = new List<T>();

            foreach (var frame in frames)
            {
                if (!frameNums.Contains(frame.FrameTime))
                {
                    frameNums.Add(frame.FrameTime);
                }
                else
                {
                    removeList.Add(frame);
                }
            }

            foreach (var frame in removeList)
            {
                frames.Remove(frame);
            }
        }

        private void ExportVmd(Stream stream)
        {
            var motionList = new List<VmdMotionFrameData>();
            foreach (var value in BoneFrameDict.Values)
            {
                motionList.AddRange(value);
            }

            var morphList = new List<VmdMorphFrameData>();
            foreach (var value in MorphFrameDict.Values)
            {
                morphList.AddRange(value);
            }

            var exporter = new VmdExporter(stream);

            exporter.Export(new VmdMotionData
            {
                Header = new VmdHeaderData { ModelName = World.ModelName },
                MotionArray = motionList.ToArray(),
                MorphArray = morphList.ToArray(),
                PropertyArray = PropertyFrameList.ToArray(),
            });
        }

        public void Export()
        {
            string exportPath = World.VmdExportPath;
            File.Delete(exportPath);

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                ExportVmd(stream);
            }
        }
    }
}
