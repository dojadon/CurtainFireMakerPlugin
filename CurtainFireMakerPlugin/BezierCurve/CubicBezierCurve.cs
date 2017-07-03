using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

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
            return inv * inv * inv * p0.X + 3 * inv * inv * t * p1.X + 3 * inv * t * t * p2.X + t * t * t * p3.X;
        }

        public double Y(double t)
        {
            double inv = 1 - t;
            return inv * inv * inv * p0.Y+ 3 * inv * inv * t * p1.Y + 3 * inv * t * t * p2.Y + t * t * t * p3.Y;
        }
    }
}
