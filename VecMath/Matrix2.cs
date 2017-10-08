using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    public struct Matrix2
    {
        public static readonly Matrix2 Identity = new Matrix2(Vector2.UnitX, Vector2.UnitY);

        public const int Size = 2;

        public float[,] Coefficients { get; }

        public float this[int i, int j] { get => Coefficients[i, j]; set => Coefficients[i, j] = value; }

        public float M00 { get => Coefficients[0, 0]; set => Coefficients[0, 0] = value; }
        public float M01 { get => Coefficients[0, 1]; set => Coefficients[0, 1] = value; }
        public float M10 { get => Coefficients[1, 0]; set => Coefficients[1, 0] = value; }
        public float M11 { get => Coefficients[1, 1]; set => Coefficients[1, 1] = value; }

        public Matrix2(float[,] coefficients)
        {
            Coefficients = coefficients;
        }

        public Matrix2(float m00, float m01, float m10, float m11) : this(new float[,] { { m00, m01 }, { m10, m11 } }) { }

        public Matrix2(Vector2 x, Vector2 y) : this(x.x, x.y, y.x, y.y) { }

        public static Matrix2 Mul(Matrix2 m1, Matrix2 m2)
        {
            var m3 = new Matrix2();

            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    m3[row, column] = Row(m1, row) * Column(m2, column);
                }
            }
            return m3;
        }

        public static Matrix2 Mul(Matrix2 m, float f)
        {
            var m3 = new Matrix2();

            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    m3[row, column] = m[row, column] * f;
                }
            }
            return m3;
        }

        public static Vector2 Row(Matrix2 m, int index) => new Vector2(m[index, 0], m[index, 1]);

        public static Vector2 Column(Matrix2 m, int index) => new Vector2(m[0, index], m[1, index]);

        public float Det() => M00 * M11 - M01 * M10;
    }
}
