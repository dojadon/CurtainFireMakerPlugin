using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.BezierCurve;
using DxMath;

namespace CurtainFireMakerPlugin.Entities.Motion
{
    public class MotionInterpolation
    {
        private VmdBezierCurve curve;
        private Vector3 startPos;
        private Vector3 endPos;
        public int startFrame;
        public int endFrame;

        public MotionInterpolation(int startFrame, int endFrame, Vector2 p1, Vector2 p2, Vector3 startPos, Vector3 endPos)
        {
            this.curve = new VmdBezierCurve(p1, p2);

            this.startPos = startPos;
            this.endPos = endPos;

            this.startFrame = startFrame;
            this.endFrame = endFrame;
        }

        public bool Within(int frame)
        {
            return this.startFrame <= frame && frame < this.endFrame;
        }

        public double GetChangeAmount(int frame)
        {
            if (this.Within(frame))
            {
                double x1 = (float)(frame - this.startFrame) / (float)(this.endFrame - this.startFrame);
                double x2 = x1 + 1.0 / (this.endFrame - this.startFrame);

                double y1 = this.curve.FuncY(x1);
                double y2 = this.curve.FuncY(x2);

                double changeY = y2 - y1;
                double defaultChangeY = 1.0 / (this.endFrame - this.startFrame);

                return changeY / defaultChangeY;
            }
            else
            {
                return 1.0 / (this.endFrame - this.startFrame);
            }
        }

        public bool ShouldRecord(int frame)
        {
            return this.endFrame < frame;
        }
    }
}
