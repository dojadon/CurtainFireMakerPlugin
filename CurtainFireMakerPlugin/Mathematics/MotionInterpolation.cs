using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Mathematics
{
    public class MotionInterpolation
    {
        public CubicBezierCurve Curve { get; }
        public int StartFrame { get; }
        public int EndFrame { get; }
        public int Length { get; }

        public MotionInterpolation(int startFrame, int length, Vector2 p1, Vector2 p2)
        {
            Curve = new CubicBezierCurve(new Vector2(0, 0), p1, p2, new Vector2(1, 1));

            StartFrame = startFrame;
            Length = length;
            EndFrame = StartFrame + Length;
        }

        public bool Within(int frame)
        {
            return StartFrame <= frame && frame < EndFrame;
        }

        public float GetChangeAmount(int frame)
        {
            if (Within(frame))
            {
                float x1 = (frame - StartFrame) / (float)(Length);
                float x2 = x1 + 1.0F / Length;

                float y1 = FuncY(x1);
                float y2 = FuncY(x2);

                float changeY = y2 - y1;
                float defaultChangeY = 1.0F / Length;

                return changeY / defaultChangeY;
            }
            else
            {
                return 1.0F;
            }
        }

        public float GetT(float x)
        {
            if (x == 1) x = 0.999999F;
            float a0 = -x;
            float a1 = 3 * Curve.P1.x;
            float a2 = -3 * (2 * Curve.P1.x - Curve.P2.x);
            float a3 = 3 * (Curve.P1.x - Curve.P2.x) + 1;

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
            return Curve.Y(GetT(x));
        }

        public bool ShouldRecord(int frame)
        {
            return EndFrame < frame;
        }
    }
}
