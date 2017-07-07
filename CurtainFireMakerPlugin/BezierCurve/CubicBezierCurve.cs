using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Mathematics;

namespace CurtainFireMakerPlugin.BezierCurve
{
    public class CubicBezierCurve
    {
        public readonly Vector2 p0;
        public readonly Vector2 p1;
        public readonly Vector2 p2;
        public readonly Vector2 p3;

        public CubicBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public double X(double t)
        {
            double inv = 1 - t;
            return inv * inv * inv * p0.x + 3 * inv * inv * t * p1.x + 3 * inv * t * t * p2.x+ t * t * t * p3.x;
        }

        public double Y(double t)
        {
            double inv = 1 - t;
            return inv * inv * inv * p0.y+ 3 * inv * inv * t * p1.y + 3 * inv * t * t * p2.y + t * t * t * p3.y;
        }
    }
}
