using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CsVmd.Data
{
    public class VmdMotionFrameData : IVmdData
    {
        public string boneName;
        public int keyFrameNo;
        public Vector3 pos = new Vector3();
        public Quaternion rot = new Quaternion();

        public byte[] interpolatePointX = new byte[4];
        public byte[] interpolatePointY = new byte[4];
        public byte[] interpolatePointZ = new byte[4];
        public byte[] interpolatePointR = new byte[4];

        public void Export(VmdExporter exporter)
        {
            exporter.WriteVmdText(this.boneName, VmdExporter.BONE_NAME_LENGTH);
            exporter.Write(this.keyFrameNo);
            exporter.Write(this.pos);
            exporter.Write(this.rot);
            this.ExportInterpolateData(exporter);
        }

        private void ExportInterpolateData(VmdExporter exporter)
        {
            byte[][] interpolatePoint = new byte[][] { interpolatePointX, interpolatePointY, interpolatePointZ, interpolatePointR };

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
