using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Mathematics;

namespace CurtainFireMakerPlugin.BezierCurve
{
    public class VmdBezierCurve : CubicBezierCurve
    {
        public VmdBezierCurve(Vector2 p1, Vector2 p2) : base(new Vector2(0, 0), p1, p2, new Vector2(1, 1))
        {
        }

        public double GetT(double x)
        {
            double a0 = -x;
            double a1 = 3 * this.p1.x;
            double a2 = -3 * (2 * this.p1.x - this.p2.x);
            double a3 = 3 * (this.p1.x - this.p2.x) + 1;

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
            return t;
        }

        public double FuncY(double x)
        {
            return this.Y(this.GetT(x));
        }
    }
}
