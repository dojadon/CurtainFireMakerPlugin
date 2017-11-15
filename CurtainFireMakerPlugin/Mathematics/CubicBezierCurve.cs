using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Mathematics
{
    public struct CubicBezierCurve
    {
        public static readonly CubicBezierCurve Line = new CubicBezierCurve(new Vector2(0, 0), new Vector2(0.5F, 0.5F), new Vector2(0.5F, 0.5F), new Vector2(1, 1));

        public Vector2 P0 { get; }
        public Vector2 P1 { get; }
        public Vector2 P2 { get; }
        public Vector2 P3 { get; }

        public CubicBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        public float[] SolveTimeFromX(float x, float eps = 1.0E-4F)
        {
            float a0 = P0.x - x;
            float a1 = 3 * (-P0.x + P1.x);
            float a2 = 3 * (P0.x - 2 * P1.x + P2.x);
            float a3 = -P0.x + P3.x + 3 * (P1.x - P2.x);

            double[] solution = EquationUtil.SolveCubic(a3, a2, a1, a0);

            var set = new HashSet<float>();
            foreach (double d in solution)
            {
                if (0.0 - eps <= d && d <= 1.0 + eps) set.Add((float)d);
            }
            return set.ToArray();
        }

        public float X(float t) => GetPosition(t, P0.x, P1.x, P2.x, P3.x);

        public float Y(float t) => GetPosition(t, P0.y, P1.y, P2.y, P3.y);

        public float GetPosition(float t, float p0, float p1, float p2, float p3)
        {
            float inv = 1 - t;
            return inv * inv * inv * p0 + 3 * inv * inv * t * p1 + 3 * inv * t * t * p2 + t * t * t * p3;
        }
    }
}
