using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsVmd.Data
{
    public class VmdHeaderData : IVmdData
    {
        public const string HEADER = "Vocaloid Motion Data 0002";
        public string ModelName { get; set; }

        public void Export(VmdExporter exporter)
        {
            exporter.WriteVmdText(HEADER, VmdExporter.HEADER_LENGTH);
            exporter.WriteVmdText(ModelName, VmdExporter.MODEL_NAME_LENGTH);
        }
    }
}
