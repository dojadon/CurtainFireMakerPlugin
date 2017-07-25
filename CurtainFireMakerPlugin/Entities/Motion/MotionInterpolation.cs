using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.BezierCurve;
using VecMath;

namespace CurtainFireMakerPlugin.Entities.Motion
{
    public class MotionInterpolation
    {
        public VmdBezierCurve curve;
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

        public float GetChangeAmount(int frame)
        {
            if (this.Within(frame))
            {
                float x1 = (float)(frame - this.startFrame) / (float)(this.endFrame - this.startFrame);
                float x2 = x1 + 1.0F / (this.endFrame - this.startFrame);

                float y1 = this.curve.FuncY(x1);
                float y2 = this.curve.FuncY(x2);

                float changeY = y2 - y1;
                float defaultChangeY = 1.0F / (this.endFrame - this.startFrame);

                return changeY / defaultChangeY;
            }
            else
            {
                return 1.0F / (this.endFrame - this.startFrame);
            }
        }

        public bool ShouldRecord(int frame)
        {
            return this.endFrame < frame;
        }
    }
}
