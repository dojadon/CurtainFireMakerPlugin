using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsMmdDataIO.Mvd.Data
{
    public class MvdModelPropertyData : MvdFixedItemSection
    {
        public IList<MvdModelPropertyFrame> Frames
        {
            get;
            set;
        }

        public int[] IKBones
        {
            get;
            set;
        }

        public int ModelRelationCount
        {
            get;
            set;
        }

        public MvdModelPropertyData()
            : base(MvdTag.ModelProperty)
        {
            this.Frames = new List<MvdModelPropertyFrame>();
            this.IKBones = new int[0];
        }

        protected override void Read(MvdDocument document, MvdObject obj, BinaryReader br)
        {
            // フォーマットバグ対策
            if (this.MinorType >= 1)
                this.RawItemSize += 4;

            base.Read(document, obj, br);
        }

        protected override void ReadExtensionRegion(MvdDocument document, MvdObject obj, BinaryReader br)
        {
            if (br.GetRemainingLength() >= 4)
                this.IKBones = Enumerable.Range(0, br.ReadInt32()).Select(_ => br.ReadInt32()).ToArray();

            if (this.MinorType >= 3)
                this.ModelRelationCount = br.ReadInt32();
        }

        protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
        {
            this.Frames.Add(MvdModelPropertyFrame.Parse(this, br));
        }

        protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
        {
            bw.Write(this.IKBones.Length);
            this.IKBones.ForEach(bw.Write);

            if (this.MinorType >= 3)
                bw.Write(this.ModelRelationCount);
        }

        public override void Write(MvdDocument document, BinaryWriter bw)
        {
            this.MinorType = 3;
            this.RawCount = this.Frames.Count;

            base.Write(document, bw);
        }

        protected override void BeforeWriteHeader(MvdDocument document, BinaryWriter bw)
        {
            // フォーマットバグ対策
            if (this.MinorType >= 1)
                this.RawItemSize -= 4;
        }

        protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
        {
            this.Frames[index].Write(this, bw);
        }
    }
}
