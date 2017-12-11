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
                BoneFrameDict[bone].RemoveAll(f => f.FrameTime == frame.FrameTime);
                BoneFrameDict[bone].Add(frame);
            }
        }

        public void AddMorphKeyFrame(PmxMorphData morph, VmdMorphFrameData frame)
        {
            if (frame.FrameTime >= 0)
            {
                MorphFrameDict[morph].RemoveAll(f => f.FrameTime == frame.FrameTime);
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
        }

        private void ExportVmd(Stream stream)
        {
            var exporter = new VmdExporter(stream);

            exporter.Export(new VmdMotionData
            {
                Header = new VmdHeaderData { ModelName = World.ModelName },
                MotionFrameArray = BoneFrameDict.Values.SelectMany(list => list).ToArray(),
                MorphFrameArray = MorphFrameDict.Values.SelectMany(list => list).ToArray(),
                PropertyFrameArray = PropertyFrameList.ToArray(),
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
