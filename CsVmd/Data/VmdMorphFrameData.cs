using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsVmd.Data
{
    public class VmdMorphFrameData : IVmdData
    {
        public string morphName;
        public int keyFrameNo;
        public float rate;

        public void Export(VmdExporter exporter)
        {
            exporter.WriteVmdText(this.morphName, VmdExporter.MORPH_NAME_LENGTH);
            exporter.Write(this.keyFrameNo);
            exporter.Write(this.rate);
        }
    }
}
