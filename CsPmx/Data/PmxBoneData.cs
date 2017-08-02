using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxBoneData : IPmxData
    {
        public String BoneName { get; set; } = "";
        public String BoneNameE { get; set; } = "";
        /** 座標 */
        public Vector3 Pos { get; set; }
        /** ボーンID */
        public int BoneId { get; set; }
        /** 親(前)ボーンID. 無い場合は0xffff(-1). */
        public int ParentId { get; set; }
        /** 子(次)ボーンID. 末端の場合は0. */
        public int ArrowId { get; set; }
        /** flags フラグが収められてる16 bit. {@link jp.sfjp.mikutoga.pmx.BoneFlags BoneFlags} 参照. */
        public short Flag { get; set; }
        /** 外部親のID. */
        public int ExtraParentId { get; set; }
        /** 変形階層 */
        public int Depth { get; set; }
        /** 矢先相対座標 */
        public Vector3 PosOffset { get; set; }
        /** 連動親ボーンID. */
        public int LinkParent { get; set; }
        /** 連動比. 負を指定することも可能. */
        public float Rate { get; set; }
        /** 軸の絶対座標 */
        public Vector3 AxisVec { get; set; }
        /** ローカルx軸 */
        public Vector3 LocalAxisVecX { get; set; }
        /** ローカルz軸 */
        public Vector3 LocalAxisVecZ { get; set; }
        /** IKボーンが接近しようとするIK接続先ボーンID */
        public int IkTargetId { get; set; }
        /** 再帰演算の深さ */
        public int IkDepth { get; set; }
        /** 制限角度強度 */
        public float AngleLimit { get; set; }
        /** IK影響下ボーンID */
        public int[] IkChilds { get; set; } = { };
        /** 回転角制御 */
        public Vector3[] IkAngleMin { get; set; } = { };
        /** 回転角制御 */
        public Vector3[] IkAngleMax { get; set; } = { };

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxText(this.BoneName);
            exporter.WritePmxText(this.BoneNameE);

            exporter.Write(this.Pos);
            exporter.WritePmxId(PmxExporter.SIZE_BONE, this.ParentId);

            exporter.Write(this.Depth);
            exporter.Write(this.Flag);

            if (!BoneFlags.OFFSET.check(this.Flag))
            {
                exporter.Write(this.PosOffset);
            }
            else
            {
                exporter.WritePmxId(PmxExporter.SIZE_BONE, this.ArrowId);
            }

            //if (BoneFlags.ROTATE_LINK.check(this.flag) || BoneFlags.MOVE_LINK.check(this.flag))
            //{
            //    exporter.WritePmxId(PmxExporter.SIZE_BONE, this.linkParent);
            //    exporter.Write(this.rate);
            //}

            //if (BoneFlags.AXIS_ROTATE.check(this.flag))
            //{
            //    exporter.Write(this.axisVec);
            //}

            //if (BoneFlags.LOCAL_AXIS.check(this.flag))
            //{
            //    exporter.Write(this.localAxisVecX);
            //    exporter.Write(this.localAxisVecZ);
            //}

            //if (BoneFlags.EXTRA.check(this.flag))
            //{
            //    exporter.Write(this.extraParentId);
            //}

            //if (BoneFlags.IK.check(this.flag))
            //{
            //    exporter.WritePmxId(PmxExporter.SIZE_BONE, this.ikTargetId);

            //    exporter.Write(this.ikDepth);
            //    exporter.Write(this.angleLimit);

            //    int boneNum = this.ikChilds.Length;

            //    Vector3 zeroVec = new Vector3();

            //    for (int i = 0; i < boneNum; i++)
            //    {
            //        int ikElement = this.ikChilds[i];
            //        exporter.WritePmxId(PmxExporter.SIZE_BONE, ikElement);

            //        int limit = this.ikAngleMin[i].Equals(zeroVec) && this.ikAngleMax[i].Equals(zeroVec) ? 0 : 1;
            //        exporter.Write((byte)limit);

            //        if (limit > 0)
            //        {
            //            exporter.Write(this.ikAngleMin[i]);
            //            exporter.Write(this.ikAngleMax[i]);
            //        }
            //    }
            //}
        }

        public void Parse(PmxParser parser)
        {
            this.BoneName = parser.ReadPmxText();
            this.BoneNameE = parser.ReadPmxText();

            this.Pos = parser.ReadVector3();
            this.ParentId = parser.ReadPmxId(parser.SizeBone);

            this.Depth = parser.ReadInt32();
            this.Flag = parser.ReadInt16();

            if (!BoneFlags.OFFSET.check(this.Flag))
            {
                this.PosOffset = parser.ReadVector3();
            }
            else
            {
                this.ArrowId = parser.ReadPmxId(parser.SizeBone);
            }

            if (BoneFlags.ROTATE_LINK.check(this.Flag) || BoneFlags.MOVE_LINK.check(this.Flag))
            {
                this.LinkParent = parser.ReadPmxId(parser.SizeBone);
                this.Rate = parser.ReadSingle();
            }

            if (BoneFlags.AXIS_ROTATE.check(this.Flag))
            {
                this.AxisVec = parser.ReadVector3();
            }

            if (BoneFlags.LOCAL_AXIS.check(this.Flag))
            {
                this.LocalAxisVecX = parser.ReadVector3();
                this.LocalAxisVecZ = parser.ReadVector3();
            }

            if (BoneFlags.EXTRA.check(this.Flag))
            {
                this.ExtraParentId = parser.ReadInt32();
            }

            if (BoneFlags.IK.check(this.Flag))
            {
                this.IkTargetId = parser.ReadPmxId(parser.SizeBone);

                this.IkDepth = parser.ReadInt32();
                this.AngleLimit = parser.ReadSingle();

                int boneNum = parser.ReadInt32();
                this.IkChilds = new int[boneNum];
                this.IkAngleMin = ArrayUtil.Set(new Vector3[boneNum], i => new Vector3());
                this.IkAngleMax = ArrayUtil.Set(new Vector3[boneNum], i => new Vector3());

                for (int i = 0; i < boneNum; i++)
                {
                    this.IkChilds[i] = parser.ReadPmxId(parser.SizeBone);

                    int limit = parser.ReadByte();

                    if (limit > 0)
                    {
                        IkAngleMin[i] = parser.ReadVector3();
                        IkAngleMax[i] = parser.ReadVector3();
                    }
                }
            }
        }
    }

    class BoneFlags
    {
        /** オフセット. (0:のときオフセット. 1:のときボーン.) */
        public static readonly BoneFlags OFFSET = new BoneFlags(0x0001);
        /** 回転. */
        public static readonly BoneFlags ROTATE = new BoneFlags(0x0002);
        /** 移動. */
        public static readonly BoneFlags MOVE = new BoneFlags(0x0004);
        /** 表示. */
        public static readonly BoneFlags VISIBLE = new BoneFlags(0x0008);
        /** 操作. */
        public static readonly BoneFlags OP = new BoneFlags(0x0010);
        /** IK. */
        public static readonly BoneFlags IK = new BoneFlags(0x0020);
        /** ローカル付与フラグ. */
        public static readonly BoneFlags LINK = new BoneFlags(0x0080);
        /** 回転付与. */
        public static readonly BoneFlags ROTATE_LINK = new BoneFlags(0x0100);
        /** 移動付与. */
        public static readonly BoneFlags MOVE_LINK = new BoneFlags(0x0200);
        /** 回転軸固定. */
        public static readonly BoneFlags AXIS_ROTATE = new BoneFlags(0x0400);
        /** ローカル座標軸. */
        public static readonly BoneFlags LOCAL_AXIS = new BoneFlags(0x0800);
        /** 物理後変形 */
        public static readonly BoneFlags PHYSICAL = new BoneFlags(0x1000);
        /** 外部親変形 */
        public static readonly BoneFlags EXTRA = new BoneFlags(0x2000);

        public static readonly BoneFlags[] FLAGS = { OFFSET, ROTATE, MOVE, VISIBLE, OP, IK, LINK, ROTATE_LINK, MOVE_LINK, AXIS_ROTATE, LOCAL_AXIS, PHYSICAL, EXTRA };

        private readonly short encoded;
        /**
         * コンストラクタ。
         * @param code 符号化int値
         */
        private BoneFlags(int code) : this((short)code)
        {
        }

        /**
         * コンストラクタ。
         * @param code 符号化short値
         */
        private BoneFlags(short code)
        {
            this.encoded = code;
        }

        /**
         * short値からデコードする。
         * @param code short値
         * @return デコードされた列挙子。該当するものがなければnull
         */
        public static BoneFlags decode(short code)
        {
            BoneFlags result = null;

            foreach (BoneFlags type in FLAGS)
            {
                if (type.encode() == code)
                {
                    result = type;
                    break;
                }
            }

            return result;
        }

        /**
         * short値にエンコードする。
         * @return short値
         */
        public short encode()
        {
            return this.encoded;
        }

        /**
         * フラグがonかテストする.
         * @param objective テスト対象.
         * @return on なら true
         */
        public bool check(short objective)
        {
            return ((objective & this.encoded) > 0);
        }
    }
}
