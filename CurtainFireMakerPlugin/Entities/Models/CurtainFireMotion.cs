using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CsMmdDataIO.Pmx.Data;
using CsMmdDataIO.Mvd.Data;

namespace CurtainFireMakerPlugin.Entities.Models
{
    internal class CurtainFireMotion
    {
        public List<string> NameList { get; } = new List<string>();

        public MvdNameList NameListSection { get; } = new MvdNameList();

        public MvdModelPropertyData PropertySection { get; } = new MvdModelPropertyData()
        {
        };

        public Dictionary<PmxBoneData, MvdBoneData> BoneSectionDict { get; } = new Dictionary<PmxBoneData, MvdBoneData>();
        public Dictionary<PmxMorphData, MvdMorphData> MorphSectionDict { get; } = new Dictionary<PmxMorphData, MvdMorphData>();

        private World World { get; }

        public CurtainFireMotion(World world)
        {
            World = world;
        }

        public void AddMvdPropertyFrame(MvdModelPropertyFrame frame)
        {
            PropertySection.Frames.Add(frame);
        }

        public void AddMvdBoneFrame(PmxBoneData bone, MvdBoneFrame frame)
        {
            if (frame.FrameTime < 0) return;

            var name = bone.BoneName;

            if (!NameList.Contains(name))
            {
                NameListSection.Names[NameList.Count] = name;
                NameList.Add(name);
            }

            if (!BoneSectionDict.ContainsKey(bone))
            {
                BoneSectionDict[bone] = new MvdBoneData()
                {
                    Key = NameList.IndexOf(name),
                    ParentClipId = -1,
                    StageCount = 1,
                };
            }

            BoneSectionDict[bone].Frames.Add(frame);
        }

        public void AddMvdMorphFrame(PmxMorphData morph, MvdMorphFrame frame)
        {
            if (frame.FrameTime < 0) return;

            var name = morph.MorphName;

            if (!NameList.Contains(name))
            {
                NameListSection.Names[NameList.Count] = name;
                NameList.Add(name);
            }

            if (!MorphSectionDict.ContainsKey(morph))
            {
                MorphSectionDict[morph] = new MvdMorphData()
                {
                    Key = NameList.IndexOf(name),
                    ParentClipId = -1,
                };
            }

            MorphSectionDict[morph].Frames.Add(frame);
        }

        public void Finish()
        {
            foreach (var section in BoneSectionDict.Values)
            {
                DistinctFrames(section.Frames);
            }

            foreach (var section in MorphSectionDict.Values)
            {
                DistinctFrames(section.Frames);
            }
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

        private void ExportMvd(Stream outStream)
        {
            var doc = new MvdDocument()
            {
                Version = 1.0F,
                Encoding = Encoding.Unicode,
            };

            var obj = new MvdObject()
            {
                ObjectName = "弾幕",
                EnglishObjectName = "Curtain Fire",
                KeyFps = World.Scene.KeyFramePerSec,
            };

            obj.Sections.Add(NameListSection);

            foreach (var section in BoneSectionDict.Values)
            {
                obj.Sections.Add(section);
            }

            foreach (var section in MorphSectionDict.Values)
            {
                obj.Sections.Add(section);
            }

            doc.Objects.Add(obj);

            doc.Write(outStream);
        }

        public void Export(World world)
        {
            var config = Plugin.Instance.Config;

            string exportPath = config.ExportDirPath + "\\" + world.ExportFileName + ".mvd";
            File.Delete(exportPath);

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                ExportMvd(stream);
            }
        }
    }
}
