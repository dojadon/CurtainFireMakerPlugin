using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsVmd.Data
{
    public class VmdMotionData : IVmdData
    {
        public VmdHeaderData Header { get; set; } = new VmdHeaderData();
        public VmdMotionFrameData[] MotionArray { get; set; } = { };
        public VmdMorphFrameData[] MorphArray { get; set; } = { };

        public void Export(VmdExporter exporter)
        {
            this.ExportVmdData(this.Header, exporter);
            this.ExportVmdData(this.MotionArray, exporter);
            this.ExportVmdData(this.MorphArray, exporter);
        }

        private void ExportVmdData<T>(T[] data, VmdExporter exporter) where T : IVmdData
        {
            int len = data.Length;
            exporter.Write(len);

            for (int i = 0; i < len; i++)
            {
                data[i].Export(exporter);
            }
        }

        private void ExportVmdData<T>(T data, VmdExporter exporter) where T : IVmdData
        {
            data.Export(exporter);
        }
    }
}
