using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.BezierCurve;
using CurtainFireMakerPlugin.Mathematics;
using VecMath;

namespace CurtainFireMakerPlugin.Entities.Motion
{
    public class MotionInterpolation
    {
        public CubicBezierCurve curve;
        private Vector3 startPos;
        private Vector3 endPos;
        public int startFrame;
        public int endFrame;

        public MotionInterpolation(int startFrame, int endFrame, Vector2 p1, Vector2 p2, Vector3 startPos, Vector3 endPos)
        {
            this.curve = new CubicBezierCurve(new Vector2(0, 0), p1, p2, new Vector2(1, 1));

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

                float y1 = this.FuncY(x1);
                float y2 = this.FuncY(x2);

                float changeY = y2 - y1;
                float defaultChangeY = 1.0F / (this.endFrame - this.startFrame);

                return changeY / defaultChangeY;
            }
            else
            {
                return 1.0F / (this.endFrame - this.startFrame);
            }
        }

        public float GetT(float x)
        {
            float a0 = -x;
            float a1 = 3 * this.curve.P1.x;
            float a2 = -3 * (2 * this.curve.P1.x - this.curve.P2.x);
            float a3 = 3 * (this.curve.P1.x - this.curve.P2.x) + 1;

            double[] solution = EquationUtil.SolveCubic(a3, a2, a1, a0);
            double t = solution[0];

            if ((t < 0.0 || 1.0 < t) && solution.Length > 1)
            {
                t = solution[1];
            }
            if ((t < 0.0 || 1.0 < t) && solution.Length > 2)
            {
                t = solution[2];
            }
            return (float)t;
        }

        public float FuncY(float x)
        {
            return this.curve.Y(this.GetT(x));
        }

        public bool ShouldRecord(int frame)
        {
            return this.endFrame < frame;
        }
    }
}
