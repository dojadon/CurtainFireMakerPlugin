using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsVmd.Data
{
    public class VmdMotionFrameData : IVmdData
    {
        public string BoneName { get; set; }
        public int KeyFrameNo { get; set; }
        public Vector3 Pos { get; set; } = new Vector3();
        public Quaternion Rot { get; set; } = new Quaternion();

        public byte[] InterpolatePointX { get; set; } = new byte[4];
        public byte[] InterpolatePointY { get; set; } = new byte[4];
        public byte[] InterpolatePointZ { get; set; } = new byte[4];
        public byte[] InterpolatePointR { get; set; } = new byte[4];

        public void Export(VmdExporter exporter)
        {
            exporter.WriteVmdText(this.BoneName, VmdExporter.BONE_NAME_LENGTH);
            exporter.Write(this.KeyFrameNo);
            exporter.Write(this.Pos);
            exporter.Write(this.Rot);
            this.ExportInterpolateData(exporter);
        }

        private void ExportInterpolateData(VmdExporter exporter)
        {
            byte[][] interpolatePoint = new byte[][] { InterpolatePointX, InterpolatePointY, InterpolatePointZ, InterpolatePointR };

            byte[] distPart = new byte[16];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    distPart[i * 4 + j] = interpolatePoint[j][i];
                }
            }

            byte[] dist = new byte[64];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 16 - i; j++)
                {
                    dist[i * 16 + j] = distPart[j];
                }
            }

            dist[31] = dist[46] = dist[61] = 1;
            exporter.Write(dist);
        }
    }
}
