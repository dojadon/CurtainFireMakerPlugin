using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPmx.Data
{
    public class PmxModelData : IPmxData
    {
        public PmxHeaderData Header { get; set; }
        public PmxVertexData[] VertexArray { get; set; }
        public PmxMaterialData[] MaterialArray { get; set; }
        public PmxBoneData[] BoneArray { get; set; }
        public PmxSlotData[] SlotArray { get; set; }
        public int[] VertexIndices { get; set; }
        public string[] TextureFiles { get; set; }

        public void Export(PmxExporter exporter)
        {
            this.ExportPmxData(this.Header, exporter);
            this.ExportPmxData(this.VertexArray, exporter);
            this.ExportData(this.VertexIndices, (i, ex) => ex.Write(i), exporter);
            this.ExportData(this.TextureFiles, (s, ex) => ex.WritePmxText(s), exporter);
            this.ExportPmxData(this.MaterialArray, exporter);
            this.ExportPmxData(this.BoneArray, exporter);

            this.ExportPmxData(this.SlotArray, exporter);
            exporter.Write(0);//Number of Rigid
            exporter.Write(0);//Number of Joint
        }

        private void ExportData<T>(T[] data, Action<T, PmxExporter> action, PmxExporter exporter)
        {
            exporter.Write(data.Length);
            Array.ForEach(data, d => action.Invoke(d, exporter));
        }

        private void ExportPmxData(IPmxData data, PmxExporter exporter)
        {
            data.Export(exporter);
        }

        private void ExportPmxData(IPmxData[] data, PmxExporter exporter)
        {
            exporter.Write(data.Length);
            Array.ForEach(data, d => d.Export(exporter));
        }
    }
}
