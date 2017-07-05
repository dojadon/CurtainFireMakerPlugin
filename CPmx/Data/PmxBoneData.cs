using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    [Serializable]
    public class PmxBoneData : IPmxData
    {
        public String boneName = "";
        public String boneNameE = "";
        /** 座標 */
        public Vector3 pos = new Vector3();
        /** ボーンID */
        public int boneId;
        /** 親(前)ボーンID. 無い場合は0xffff(-1). */
        public int parentId;
        /** 子(次)ボーンID. 末端の場合は0. */
        public int arrowId;
        /** flags フラグが収められてる16 bit. {@link jp.sfjp.mikutoga.pmx.BoneFlags BoneFlags} 参照. */
        public short flag;
        /** 外部親のID. */
        public int extraParentId;
        /** 変形階層 */
        public int depth;
        /** 矢先相対座標 */
        public Vector3 posOffset = new Vector3();
        /** 連動親ボーンID. */
        public int linkParent;
        /** 連動比. 負を指定することも可能. */
        public float rate;
        /** 軸の絶対座標 */
        public Vector3 axisVec = new Vector3();
        /** ローカルx軸 */
        public Vector3 localAxisVecX = new Vector3();
        /** ローカルz軸 */
        public Vector3 localAxisVecZ = new Vector3();
        /** IKボーンが接近しようとするIK接続先ボーンID */
        public int ikTargetId;
        /** 再帰演算の深さ */
        public int ikDepth;
        /** 制限角度強度 */
        public float angleLimit;
        /** IK影響下ボーンID */
        public int[] ikChilds = { };
        /** 回転角制御 */
        public Vector3[] ikAngleMin = { };
        /** 回転角制御 */
        public Vector3[] ikAngleMax = { };

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxText(this.boneName);
            exporter.WritePmxText(this.boneNameE);

            exporter.Write(this.pos);
            exporter.WritePmxId(PmxExporter.SIZE_BONE, this.parentId);

            exporter.Write(this.depth);
            exporter.Write(this.flag);

            if (!BoneFlags.OFFSET.check(this.flag))
            {
                exporter.Write(this.posOffset);
            }
            else
            {
                exporter.WritePmxId(PmxExporter.SIZE_BONE, this.arrowId);
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
            this.boneName = parser.ReadPmxText();
            this.boneNameE = parser.ReadPmxText();

            this.pos = parser.ReadVector3();
            this.parentId = parser.ReadPmxId(parser.SizeBone);

            this.depth = parser.ReadInt32();
            this.flag = parser.ReadInt16();

            if (!BoneFlags.OFFSET.check(this.flag))
            {
                this.posOffset = parser.ReadVector3();
            }
            else
            {
                this.arrowId = parser.ReadPmxId(parser.SizeBone);
            }

            if (BoneFlags.ROTATE_LINK.check(this.flag) || BoneFlags.MOVE_LINK.check(this.flag))
            {
                this.linkParent = parser.ReadPmxId(parser.SizeBone);
                this.rate = parser.ReadSingle();
            }

            if (BoneFlags.AXIS_ROTATE.check(this.flag))
            {
                this.axisVec = parser.ReadVector3();
            }

            if (BoneFlags.LOCAL_AXIS.check(this.flag))
            {
                this.localAxisVecX = parser.ReadVector3();
                this.localAxisVecZ = parser.ReadVector3();
            }

            if (BoneFlags.EXTRA.check(this.flag))
            {
                this.extraParentId = parser.ReadInt32();
            }

            if (BoneFlags.IK.check(this.flag))
            {
                this.ikTargetId = parser.ReadPmxId(parser.SizeBone);

                this.ikDepth = parser.ReadInt32();
                this.angleLimit = parser.ReadSingle();

                int boneNum = parser.ReadInt32();
                this.ikChilds = new int[boneNum];
                this.ikAngleMin = ArrayUtil.Set(new Vector3[boneNum], i => new Vector3());
                this.ikAngleMax = ArrayUtil.Set(new Vector3[boneNum], i => new Vector3());

                for (int i = 0; i < boneNum; i++)
                {
                    this.ikChilds[i] = parser.ReadPmxId(parser.SizeBone);

                    int limit = parser.ReadByte();

                    if (limit > 0)
                    {
                        ikAngleMin[i] = parser.ReadVector3();
                        ikAngleMax[i] = parser.ReadVector3();
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
