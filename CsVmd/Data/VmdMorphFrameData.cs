using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsVmd.Data
{
    public class VmdMorphFrameData : IVmdData
    {
        public string MorphName { get; set; }
        public int KeyFrameNo { get; set; }
        public float Rate { get; set; }

        public void Export(VmdExporter exporter)
        {
            exporter.WriteVmdText(this.MorphName, VmdExporter.MORPH_NAME_LENGTH);
            exporter.Write(this.KeyFrameNo);
            exporter.Write(this.Rate);
        }
    }
}
