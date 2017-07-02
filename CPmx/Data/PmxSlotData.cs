using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPmx.Data
{
    public class PmxSlotData : IPmxData
    {
        public const byte SLOT_TYPE_BONE = 0;
        public const byte SLOT_TYPE_MORPH = 1;

        public String slotName = "";
        public String slotNameE = "";

        public bool normalSlot = true;
        public byte type;
        public int[] indices;

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxText(this.slotName);
            exporter.WritePmxText(this.slotNameE);

            exporter.Write((byte)(this.normalSlot ? 0 : 1));

            int elementCount = this.indices.Length;
            exporter.Write(elementCount);

            byte size = this.type == SLOT_TYPE_BONE ? PmxExporter.SIZE_BONE : PmxExporter.SIZE_MORPH;

            for (int i = 0; i < elementCount; i++)
            {
                exporter.Write(this.type);

                int id = this.indices[i];
                exporter.WritePmxId(size, id);
            }
        }

        public void Parse(PmxParser parser)
        {
            this.slotName = parser.ReadPmxText();
            this.slotNameE = parser.ReadPmxText();

            this.normalSlot = parser.ReadByte() == 0;

            int elementCount = parser.ReadInt32();
            this.indices = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                byte type = parser.ReadByte();
                byte size = type == SLOT_TYPE_BONE ? PmxExporter.SIZE_BONE : PmxExporter.SIZE_MORPH;

               this.indices[i] = parser.ReadPmxId(size);
            }
        }
    }
}
