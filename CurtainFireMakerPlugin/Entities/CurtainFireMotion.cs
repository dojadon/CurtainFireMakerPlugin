using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Collections;
using CsVmd.Data;
using CsVmd;
using CsPmx.Data;

namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireMotion
    {
        private List<VmdMotionFrameData> motionList = new List<VmdMotionFrameData>();
        public MultiDictionary<PmxMorphData, VmdMorphFrameData> MorphDict { get; } = new MultiDictionary<PmxMorphData, VmdMorphFrameData>();

        private World World { get; }

        public CurtainFireMotion(World world)
        {
            World = world;
        }

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

        public void Export(World world)
        {
            var config = Plugin.Instance.Config;

            string fileName = config.ScriptFileName.Replace(".py", "");
            string exportPath = config.ExportDirPath + "\\" + world.ExportFileName + ".vmd";
            File.Delete(exportPath);

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                var exporter = new VmdExporter(stream);

                var data = new VmdMotionData();
                GetData(data);

                data.Header.ModelName = config.ModelName;

                exporter.Export(data);
            }
        }
    }
}
