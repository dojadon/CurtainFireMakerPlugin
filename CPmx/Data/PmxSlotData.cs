using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxSlotData : IPmxData
    {
        public const byte SLOT_TYPE_BONE = 0;
        public const byte SLOT_TYPE_MORPH = 1;

        public String SlotName { get; set; } = "";
        public String SlotNameE { get; set; } = "";

        public bool NormalSlot { get; set; } = true;
        public byte Type { get; set; }
        public int[] Indices { get; set; }

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxText(this.SlotName);
            exporter.WritePmxText(this.SlotNameE);

            exporter.Write((byte)(this.NormalSlot ? 0 : 1));

            int elementCount = this.Indices.Length;
            exporter.Write(elementCount);

            byte size = this.Type == SLOT_TYPE_BONE ? PmxExporter.SIZE_BONE : PmxExporter.SIZE_MORPH;

            for (int i = 0; i < elementCount; i++)
            {
                exporter.Write(this.Type);

                int id = this.Indices[i];
                exporter.WritePmxId(size, id);
            }
        }

        public void Parse(PmxParser parser)
        {
            this.SlotName = parser.ReadPmxText();
            this.SlotNameE = parser.ReadPmxText();

            this.NormalSlot = parser.ReadByte() == 0;

            int elementCount = parser.ReadInt32();
            this.Indices = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                byte type = parser.ReadByte();
                byte size = type == SLOT_TYPE_BONE ? parser.SizeBone : parser.SizeMorph;

                this.Indices[i] = parser.ReadPmxId(size);
            }
        }
    }
}
