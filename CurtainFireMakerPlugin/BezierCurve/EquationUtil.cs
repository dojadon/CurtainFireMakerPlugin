using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.BezierCurve
{
    public static class EquationUtil
    {
        public static double[] SolveQuadratic(double a2, double a1, double a0)
        {
            double[] solution;

            if (a2 != 0.0)
            {
                double b1 = a1 / 2.0;
                double D = b1 * b1 - a2 * a0;

                if (D > 0.0)
                {
                    double D_SQR = Math.Sqrt(D);

                    solution = new double[2];
                    solution[0] = (-b1 + D_SQR) / a2;
                    solution[1] = (-b1 - D_SQR) / a2;
                }
                else if (D == 0.0)
                {
                    solution = new double[1];
                    solution[0] = -b1 / a2;
                }
                else
                {
                    solution = new double[0];
                }
            }
            else
            {
                solution = new double[1];
                solution[0] = -a0 / a1;
            }
            return solution;
        }

        public static double[] SolveCubic(double a3, double a2, double a1, double a0)
        {
            double[] solution;

            if (a0 == 0)
            {
                solution = SolveQuadratic(a2, a1, a0);
            }
            else
            {
                // Normalize coefficients.
                double denom = a3;
                a3 = a2 / denom;
                a2 = a1 / denom;
                a1 = a0 / denom;

                // Commence solution.
                double a_over_3 = a3 / 3.0;
                double Q = (3 * a2 - a3 * a3) / 9.0;
                double Q_CUBE = Q * Q * Q;
                double R = (9 * a3 * a2 - 27 * a1 - 2 * a3 * a3 * a3) / 54.0;
                double R_SQR = R * R;
                double D = Q_CUBE + R_SQR;

                if (D < 0.0)
                {
                    // Three unequal real roots.
                    solution = new double[3];
                    double theta = Math.Acos(R / Math.Sqrt(-Q_CUBE));
                    double SQRT_Q = Math.Sqrt(-Q);
                    solution[0] = 2.0 * SQRT_Q * Math.Cos(theta / 3.0) - a_over_3;
                    solution[1] = 2.0 * SQRT_Q * Math.Cos((theta + Math.PI * 2.0) / 3.0) - a_over_3;
                    solution[2] = 2.0 * SQRT_Q * Math.Cos((theta + Math.PI * 4.0) / 3.0) - a_over_3;
                }
                else if (D > 0.0)
                {
                    // One real root.
                    solution = new double[1];
                    double SQRT_D = Math.Sqrt(D);
                    double S = Math.Pow(R + SQRT_D, 1 / 3);
                    double T = Math.Pow(R - SQRT_D, 1 / 3);
                    solution[0] = S + T - a_over_3;
                }
                else
                {
                    // Three real roots, at least two equal.
                    solution = new double[2];
                    double CBRT_R = Math.Pow(R, 1 / 3);
                    solution[0] = 2 * CBRT_R - a_over_3;
                    solution[1] = CBRT_R - a_over_3;
                }
            }
            return solution;
        }

        public static double[] SolveQuartic(double a4, double a3, double a2, double a1, double a0)
        {
            double[] solution;

            if (a4 != 0.0)
            {
                if (a3 == 0.0 && a1 == 0.0)
                {// biquadratic equation
                    solution = SolveQuadratic(a4, a2, a0);

                    for (int i = 0; i < solution.Length; i++)
                    {
                        solution[i] = Math.Sqrt(solution[i]);
                    }
                }
                else
                {
                    double inv = 1 / a4;
                    double A0 = a0 * inv;
                    double A1 = a1 * inv;
                    double A2 = a2 * inv;
                    double A3 = a3 * inv;

                    double B3 = A3 / 4;
                    double B3_2 = B3 * B3;
                    double B3_3 = B3_2 * B3;
                    double B3_4 = B3_2 * B3_2;

                    double p = A2 - 6 * B3_2;
                    double q = A1 - 2 * A2 * B3 + 8 * B3_3;
                    double r = A0 - A1 * B3 + A2 * B3_2 - 3 * B3_4;

                    // if (q == 0.0)
                    // {
                    // solution = QuadraticEquation.solve(1, p, r);
                    //
                    // for (int i = 0; i < solution.length; i++)
                    // {
                    // solution[i] = Math.sqrt(solution[i]);
                    // }
                    // }
                    // else
                    {
                        double u = SolveU(p, q, r);
                        solution = SolveY(p, q, u);
                    }

                    for (int i = 0; i < solution.Length; i++)
                    {
                        solution[i] = solution[i] - B3;
                    }
                }
            }
            else
            {// cubic equation
                solution = SolveCubic(a3, a2, a1, a0);
            }

            return solution;
        }

        private static double[] SolveY(double p, double q, double u)
        {
            double uSqrt = Math.Sqrt(u);

            double a0 = (p * u + u * u - q * uSqrt) / (2 * u);
            double a1 = uSqrt;
            double a2 = 1;

            double[] solution1 = SolveQuadratic(a2, a1, a0);

            a0 = (p * u + u * u + q * uSqrt) / (2 * u);
            a1 = -uSqrt;

            double[] solution2 = SolveQuadratic(a2, a1, a0);

            var list1 = solution1.ToList();
            var list2 = solution2.ToList();

            list1.AddRange(list2);

            list1.Sort();

            return list1.ToArray();
        }

        private static double SolveU(double p, double q, double r)
        {
            double a0 = -q * q;
            double a1 = p * p - 4 * r;
            double a2 = 2 * p;
            double a3 = 1;

            double[] solution = SolveCubic(a3, a2, a1, a0);

            return solution[0];
        }
    }
}
