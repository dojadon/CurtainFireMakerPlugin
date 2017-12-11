using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using CsMmdDataIO.Pmx;
using CsMmdDataIO.Vmd;

namespace CurtainFireMakerPlugin.Entities
{
    class KeyFrameEqualityComparer<T> : IEqualityComparer<T> where T : IElementKeyFrame
    {
        public bool Equals(T x, T y)
        {
            return x.FrameTime == y.FrameTime && x.Name == y.Name;
        }

        public int GetHashCode(T obj)
        {
            int result = 17;
            result = result * 23 + obj.FrameTime;
            result = result * 23 + obj.Name.GetHashCode();

            return result;
        }
    }

    internal class CurtainFireMotion
    {
        public HashSet<VmdMotionFrameData> BoneFrames { get; }
        public HashSet<VmdMorphFrameData> MorphFrames { get; }
        public List<VmdPropertyFrameData> PropertyFrames { get; }

        private World World { get; }

        public CurtainFireMotion(World world)
        {
            World = world;

            BoneFrames = new HashSet<VmdMotionFrameData>(new KeyFrameEqualityComparer<VmdMotionFrameData>());
            MorphFrames = new HashSet<VmdMorphFrameData>(new KeyFrameEqualityComparer<VmdMorphFrameData>());
            PropertyFrames = new List<VmdPropertyFrameData>();
        }

        public void AddBoneKeyFrame(PmxBoneData bone, VmdMotionFrameData frame, bool replace)
        {
            if (frame.FrameTime >= 0 && (replace || !BoneFrames.Contains(frame)))
            {
                BoneFrames.Add(frame);
            }
        }

        public void AddMorphKeyFrame(PmxMorphData morph, VmdMorphFrameData frame, bool replace)
        {
            if (frame.FrameTime >= 0 && (replace || !MorphFrames.Contains(frame)))
            {
                MorphFrames.Add(frame);
            }
        }

        public void AddPropertyKeyFrame(VmdPropertyFrameData frame)
        {
            if (frame.FrameTime >= 0)
            {
                PropertyFrames.Add(frame);
            }
        }

        public void FinalizeKeyFrame()
        {
            var morphNames = World.PmxModel.Morphs.MorphList.Select(m => m.MorphName).ToList();
            MorphFrames.RemoveWhere(f => !morphNames.Contains(f.Name));
        }

        private void ExportVmd(Stream stream)
        {
            var exporter = new VmdExporter(stream);

            exporter.Export(new VmdMotionData
            {
                Header = new VmdHeaderData { ModelName = World.ModelName },
                MotionFrameArray = BoneFrames.ToArray(),
                MorphFrameArray = MorphFrames.ToArray(),
                PropertyFrameArray = PropertyFrames.ToArray(),
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
