using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPmx.Data
{
    [Serializable]
    public class PmxModelData : IPmxData
    {
        public PmxHeaderData Header { get; set; } = new PmxHeaderData();
        public PmxVertexData[] VertexArray { get; set; }
        public PmxMaterialData[] MaterialArray { get; set; }
        public PmxBoneData[] BoneArray { get; set; }
        public PmxMorphData[] MorphArray { get; set; }
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
            this.ExportPmxData(this.MorphArray, exporter);
            this.ExportPmxData(this.SlotArray, exporter);
            exporter.Write(0);//Number of Rigid
            exporter.Write(0);//Number of Joint
        }

        public void Parse(PmxParser parser)
        {
            this.ParsePmxData(this.Header, parser);
            this.VertexArray = this.ParsePmxData(len => ArrayUtil.Set(new PmxVertexData[len], i => new PmxVertexData()), parser);
            this.VertexIndices = this.ParseData(len => new int[len], (p, i) => p.ReadPmxId(parser.SizeVertex), parser);
            this.TextureFiles = this.ParseData(len => new string[len], (p, i) => p.ReadPmxText(), parser);
            this.MaterialArray = this.ParsePmxData(len => ArrayUtil.Set(new PmxMaterialData[len], i => new PmxMaterialData()), parser);
            this.BoneArray = this.ParsePmxData(len => ArrayUtil.Set(new PmxBoneData[len], i => new PmxBoneData()), parser);
            this.MorphArray = this.ParsePmxData(len => ArrayUtil.Set(new PmxMorphData[len], i => new PmxMorphData()), parser);
            this.SlotArray = this.ParsePmxData(len => ArrayUtil.Set(new PmxSlotData[len], i => new PmxSlotData()), parser);
        }

        private void ExportData<T>(T[] data, Action<T, PmxExporter> action, PmxExporter exporter)
        {
            exporter.Write(data.Length);
            Array.ForEach(data, d => action.Invoke(d, exporter));
        }

        private void ExportPmxData<T>(T data, PmxExporter exporter) where T : IPmxData
        {
            data.Export(exporter);
        }

        private void ExportPmxData<T>(T[] data, PmxExporter exporter) where T : IPmxData
        {
            exporter.Write(data.Length);
            Array.ForEach(data, d => d.Export(exporter));
        }

        private T[] ParseData<T>(Func<int, T[]> func, Func<PmxParser, T, T> valueFunc, PmxParser parser)
        {
            int len = parser.ReadInt32();
            Console.WriteLine("length : " + len);
            T[] array = func.Invoke(len);

            for (int i = 0; i < len; i++)
            {
                array[i] = valueFunc.Invoke(parser, array[i]);
            }
            return array;
        }

        private void ParsePmxData<T>(T data, PmxParser parser) where T : IPmxData
        {
            data.Parse(parser);
        }

        private T[] ParsePmxData<T>(Func<int, T[]> func, PmxParser parser) where T : IPmxData
        {
            int len = parser.ReadInt32();
            Console.WriteLine("length : " + len);
            T[] array = func.Invoke(len);
            for (int i = 0; i < len; i++)
            {
                array[i].Parse(parser);
            }
            return array;
        }
    }
}
