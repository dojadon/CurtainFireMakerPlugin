using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Mathematics
{
    public struct CubicBezierCurve
    {
        public static CubicBezierCurve Line = new CubicBezierCurve(new Vector2(0, 0), new Vector2(0.5F, 0.5F), new Vector2(0.5F, 0.5F), new Vector2(1, 1));

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

        public float X(float t)
        {
            float inv = 1 - t;
            return inv * inv * inv * P0.x + 3 * inv * inv * t * P1.x + 3 * inv * t * t * P2.x + t * t * t * P3.x;
        }

        public float Y(float t)
        {
            float inv = 1 - t;
            return inv * inv * inv * P0.y + 3 * inv * inv * t * P1.y + 3 * inv * t * t * P2.y + t * t * t * P3.y;
        }
    }
}
