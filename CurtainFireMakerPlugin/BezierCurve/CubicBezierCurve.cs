using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Mathematics;

namespace CurtainFireMakerPlugin.BezierCurve
{
    public class CubicBezierCurve
    {
        public Vector2 P0 { get; }
        public Vector2 P1 { get; }
        public Vector2 P2 { get; }
        public Vector2 P3 { get; }

        public CubicBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            this.P0 = p0;
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;
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
