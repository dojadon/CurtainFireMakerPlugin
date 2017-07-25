using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;
using CurtainFireMakerPlugin.Mathematics;

namespace CurtainFireMakerPlugin.BezierCurve
{
    public class VmdBezierCurve : CubicBezierCurve
    {
        public static readonly VmdBezierCurve Line = new VmdBezierCurve(new Vector2(0.5F, 0.5F), new Vector2(0.5F, 0.5F));

        public VmdBezierCurve(Vector2 p1, Vector2 p2) : base(new Vector2(0, 0), p1, p2, new Vector2(1, 1))
        {
        }

        public float GetT(float x)
        {
            float a0 = -x;
            float a1 = 3 * this.P1.x;
            float a2 = -3 * (2 * this.P1.x - this.P2.x);
            float a3 = 3 * (this.P1.x - this.P2.x) + 1;

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
            return (float) t;
        }

        public float FuncY(float x)
        {
            return this.Y(this.GetT(x));
        }
    }
}
