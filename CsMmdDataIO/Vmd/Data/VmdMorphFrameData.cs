using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Vmd.Data
{
    public class VmdMorphFrameData : IVmdData, IKeyFrame
    {
        public string MorphName { get; set; }
        public long FrameTime { get; set; }
        public float Weigth { get; set; }

        public void Export(VmdExporter exporter)
        {
            exporter.WriteTextWithFixedLength(MorphName, VmdExporter.MORPH_NAME_LENGTH);
            exporter.Write((int)FrameTime);
            exporter.Write(Weigth);
        }
    }
}
