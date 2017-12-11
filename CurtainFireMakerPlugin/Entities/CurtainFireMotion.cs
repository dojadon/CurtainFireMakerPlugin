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
        public List<VmdMotionFrameData> BoneFrameList { get; }
        public List<VmdMorphFrameData> MorphFrameList { get; }
        public List<VmdPropertyFrameData> PropertyFrameList { get; }

        private World World { get; }

        public CurtainFireMotion(World world)
        {
            World = world;

            BoneFrameList = new List<VmdMotionFrameData>();
            MorphFrameList = new List<VmdMorphFrameData>();
            PropertyFrameList = new List<VmdPropertyFrameData>();
        }

        public void AddBoneKeyFrame(PmxBoneData bone, VmdMotionFrameData frame)
        {
            if (frame.FrameTime >= 0)
            {
                BoneFrameList.RemoveAll(f => f.FrameTime == frame.FrameTime && f.BoneName == frame.BoneName);
                BoneFrameList.Add(frame);
            }
        }

        public void AddMorphKeyFrame(PmxMorphData morph, VmdMorphFrameData frame)
        {
            if (frame.FrameTime >= 0)
            {
                MorphFrameList.RemoveAll(f => f.FrameTime == frame.FrameTime && f.MorphName == frame.MorphName);
                MorphFrameList.Add(frame);
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
                MotionFrameArray = BoneFrameList.ToArray(),
                MorphFrameArray = MorphFrameList.ToArray(),
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
