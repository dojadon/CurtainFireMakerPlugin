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
                float unit = 1.0F / Length;

                float x1 = (frame - StartFrame) * unit;
                float x2 = x1 + unit;

                return (FuncY(x2) - FuncY(x1)) * Length;
            }
            else
            {
                return 1.0F;
            }
        }

        public float FuncY(float x)
        {
            float[] t = Curve.SolveTimeFromX(x);

            if (t.Length == 0)
            {
                throw new ArithmeticException($"ベジエ曲線の解が見つかりません : x[ {x} ]");
            }

            float time = t[0];

            if (t.Length > 1)
            {
                for (int i = 1; i < t.Length; i++)
                {
                    if (Math.Abs(x - time) > Math.Abs(x - t[i])) time = t[i];
                }
            }
            return Curve.Y(time);
        }
    }
}
