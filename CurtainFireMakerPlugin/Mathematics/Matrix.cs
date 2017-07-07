using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Mathematics
{
    public struct Matrix
    {
        private const double EPS = 1.0E-10;

        public double m00;
        public double m01;
        public double m02;
        public double m03;
        public double m10;
        public double m11;
        public double m12;
        public double m13;
        public double m20;
        public double m21;
        public double m22;
        public double m23;
        public double m30;
        public double m31;
        public double m32;
        public double m33;

        public Matrix(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21,
            double m22, double m23, double m30, double m31, double m32, double m33)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;

            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;

            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;

            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public static Matrix Identity()
        {
            var m1 = new Matrix();

            m1.m00 = 1.0;
            m1.m01 = 0.0;
            m1.m02 = 0.0;
            m1.m03 = 0.0;

            m1.m10 = 0.0;
            m1.m11 = 1.0;
            m1.m12 = 0.0;
            m1.m13 = 0.0;

            m1.m20 = 0.0;
            m1.m21 = 0.0;
            m1.m22 = 1.0;
            m1.m23 = 0.0;

            m1.m30 = 0.0;
            m1.m31 = 0.0;
            m1.m32 = 0.0;
            m1.m33 = 1.0;

            return m1;
        }

        public static Matrix FromQuaternion(Quaternion q)
        {
            var m1 = Identity();

            double ww = q.w * q.w;
            double xx = q.x * q.x;
            double yy = q.y * q.y;
            double zz = q.z * q.z;

            m1.m00 = 1 - 2 * (yy + zz);
            m1.m01 = 2 * (q.x * q.y + q.w * q.z);
            m1.m02 = 2 * (q.x * q.z - q.w * q.y);

            m1.m10 = 2 * (q.x * q.y - q.w * q.z);
            m1.m11 = 1 - 2 * (zz + xx);
            m1.m12 = 2 * (q.y * q.z + q.w * q.x);

            m1.m20 = 2 * (q.x * q.z + q.w * q.y);
            m1.m21 = 2 * (q.y * q.z - q.w * q.x);
            m1.m22 = 1 - 2 * (xx + yy);

            return m1;
        }

        public static Matrix Mul(Matrix m1, Matrix m2)
        {
            var m3 = new Matrix();

            m3.m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20 + m1.m03 * m2.m30;
            m3.m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21 + m1.m03 * m2.m31;
            m3.m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22 + m1.m03 * m2.m32;
            m3.m03 = m1.m00 * m2.m03 + m1.m01 * m2.m13 + m1.m02 * m2.m23 + m1.m03 * m2.m33;

            m3.m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20 + m1.m13 * m2.m30;
            m3.m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21 + m1.m13 * m2.m31;
            m3.m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22 + m1.m13 * m2.m32;
            m3.m13 = m1.m10 * m2.m03 + m1.m11 * m2.m13 + m1.m12 * m2.m23 + m1.m13 * m2.m33;

            m3.m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20 + m1.m23 * m2.m30;
            m3.m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21 + m1.m23 * m2.m31;
            m3.m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22 + m1.m23 * m2.m32;
            m3.m23 = m1.m20 * m2.m03 + m1.m21 * m2.m13 + m1.m22 * m2.m23 + m1.m23 * m2.m33;

            m3.m30 = m1.m30 * m2.m00 + m1.m31 * m2.m10 + m1.m32 * m2.m20 + m1.m33 * m2.m30;
            m3.m31 = m1.m30 * m2.m01 + m1.m31 * m2.m11 + m1.m32 * m2.m21 + m1.m33 * m2.m31;
            m3.m32 = m1.m30 * m2.m02 + m1.m31 * m2.m12 + m1.m32 * m2.m22 + m1.m33 * m2.m32;
            m3.m33 = m1.m30 * m2.m03 + m1.m31 * m2.m13 + m1.m32 * m2.m23 + m1.m33 * m2.m33;

            return m3;
        }

        public static Matrix LookAt(Vector3 eye, Vector3 at, Vector3 upward)
        {
            var m1 = Identity();

            var z = +(eye - at);
            var x = +(upward ^ z);
            var y = +(z ^ x);

            m1.m00 = x.x;
            m1.m01 = x.y;
            m1.m02 = x.z;

            m1.m10 = y.x;
            m1.m11 = y.y;
            m1.m12 = y.z;

            m1.m20 = z.x;
            m1.m21 = z.y;
            m1.m22 = z.z;

            return m1;
        }

        public static Matrix Inverse(Matrix m1)
        {
            Matrix m2;
            int[] row = new int[4];

            if (LuDecomposition(m1, row, out m2))
            {
                throw new Exception("An exception occurred in solve the inverse matrix");
            }

            return LuBacksubstitution(m2, row);
        }

        private static bool LuDecomposition(Matrix m1, int[] row_perm, out Matrix result)
        {
            var matrix0 = (double[])m1;
            var row_scale = new double[4];
            result = new double[16];

            // Determine implicit scaling information by looping over rows
            {
                int i, j;
                int ptr, rs;
                double big, temp;

                ptr = 0;
                rs = 0;

                // For each row ...
                i = 4;
                while (i-- != 0)
                {
                    big = 0.0;

                    // For each column, find the largest element in the row
                    j = 4;
                    while (j-- != 0)
                    {
                        temp = matrix0[ptr++];
                        temp = Math.Abs(temp);
                        if (temp > big)
                        {
                            big = temp;
                        }
                    }

                    // Is the matrix singular?
                    if (big == 0.0) { return false; }
                    row_scale[rs++] = 1.0 / big;
                }
            }

            {
                int j;
                int mtx;

                mtx = 0;

                // For all columns, execute Crout's method
                for (j = 0; j < 4; j++)
                {
                    int i, imax, k;
                    int target, p1, p2;
                    double sum, big, temp;

                    // Determine elements of upper diagonal matrix U
                    for (i = 0; i < j; i++)
                    {
                        target = mtx + 4 * i + j;
                        sum = matrix0[target];
                        k = i;
                        p1 = mtx + 4 * i;
                        p2 = mtx + j;
                        while (k-- != 0)
                        {
                            sum -= matrix0[p1] * matrix0[p2];
                            p1++;
                            p2 += 4;
                        }
                        matrix0[target] = sum;
                    }

                    // Search for largest pivot element and calculate
                    // intermediate elements of lower diagonal matrix L.
                    big = 0.0;
                    imax = -1;
                    for (i = j; i < 4; i++)
                    {
                        target = mtx + 4 * i + j;
                        sum = matrix0[target];
                        k = j;
                        p1 = mtx + 4 * i;
                        p2 = mtx + j;
                        while (k-- != 0)
                        {
                            sum -= matrix0[p1] * matrix0[p2];
                            p1++;
                            p2 += 4;
                        }
                        matrix0[target] = sum;

                        // Is this the best pivot so far?
                        if ((temp = row_scale[i] * Math.Abs(sum)) >= big)
                        {
                            big = temp;
                            imax = i;
                        }
                    }

                    if (imax < 0) { return false; }

                    // Is a row exchange necessary?
                    if (j != imax)
                    {
                        // Yes: exchange rows
                        k = 4;
                        p1 = mtx + 4 * imax;
                        p2 = mtx + 4 * j;
                        while (k-- != 0)
                        {
                            temp = matrix0[p1];
                            matrix0[p1++] = matrix0[p2];
                            matrix0[p2++] = temp;
                        }

                        // Record change in scale factor
                        row_scale[imax] = row_scale[j];
                    }

                    // Record row permutation
                    row_perm[j] = imax;

                    // Is the matrix singular
                    if (matrix0[mtx + 4 * j + j] == 0.0) { return false; }

                    // Divide elements of lower diagonal matrix L by pivot
                    if (j != 4 - 1)
                    {
                        temp = 1.0 / matrix0[mtx + 4 * j + j];
                        target = mtx + 4 * (j + 1) + j;
                        i = 3 - j;
                        while (i-- != 0)
                        {
                            matrix0[target] *= temp;
                            target += 4;
                        }
                    }
                }
            }

            result = matrix0;

            return true;
        }

        private static Matrix LuBacksubstitution(Matrix m1, int[] row_perm)
        {
            double[] matrix1 = (double[])m1;
            double[] matrix2 = (double[])Identity();

            int i, ii, ip, j, k;
            int rp;
            int cv, rv;

            // rp = row_perm;
            rp = 0;

            // For each column vector of matrix2 ...
            for (k = 0; k < 4; k++)
            {
                // cv = &(matrix2[0][k]);
                cv = k;
                ii = -1;

                // Forward substitution
                for (i = 0; i < 4; i++)
                {
                    double sum;

                    ip = row_perm[rp + i];
                    sum = matrix2[cv + 4 * ip];
                    matrix2[cv + 4 * ip] = matrix2[cv + 4 * i];
                    if (ii >= 0)
                    {
                        // rv = &(matrix1[i][0]);
                        rv = i * 4;
                        for (j = ii; j <= i - 1; j++)
                        {
                            sum -= matrix1[rv + j] * matrix2[cv + 4 * j];
                        }
                    }
                    else if (sum != 0.0)
                    {
                        ii = i;
                    }
                    matrix2[cv + 4 * i] = sum;
                }

                // Backsubstitution
                // rv = &(matrix1[3][0]);
                rv = 3 * 4;
                matrix2[cv + 4 * 3] /= matrix1[rv + 3];

                rv -= 4;
                matrix2[cv + 4 * 2] = (matrix2[cv + 4 * 2] - matrix1[rv + 3] * matrix2[cv + 4 * 3]) / matrix1[rv + 2];

                rv -= 4;
                matrix2[cv + 4 * 1] = (matrix2[cv + 4 * 1] - matrix1[rv + 2] * matrix2[cv + 4 * 2] - matrix1[rv + 3] * matrix2[cv + 4 * 3])
                        / matrix1[rv + 1];

                rv -= 4;
                matrix2[cv + 4 * 0] = (matrix2[cv + 4 * 0] - matrix1[rv + 1] * matrix2[cv + 4 * 1] - matrix1[rv + 2] * matrix2[cv + 4 * 2]
                        - matrix1[rv + 3] * matrix2[cv + 4 * 3]) / matrix1[rv + 0];
            }

            return matrix2;
        }

        public static Matrix operator ~(Matrix m1) => Inverse(m1);

        public static Matrix operator +(Matrix m1, Vector3 v1)
        {
            var m2 = m1;

            m2.m03 += v1.x;
            m2.m13 += v1.y;
            m2.m23 += v1.z;

            return m2;
        }

        public static Matrix operator -(Matrix m1, Vector3 v1)
        {
            var m2 = m1;

            m2.m03 -= v1.x;
            m2.m13 -= v1.y;
            m2.m23 -= v1.z;

            return m2;
        }

        public static Matrix operator *(Matrix m1, Matrix m2) => Mul(m1, m2);

        public static implicit operator Matrix(Quaternion q1) => FromQuaternion(q1);

        public static explicit operator Vector3(Matrix m1) => new Vector3(m1.m03, m1.m13, m1.m23);

        public static explicit operator double[] (Matrix m)
        {
            var result = new double[16];

            int index = 0;
            result[index++] = m.m00;
            result[index++] = m.m01;
            result[index++] = m.m02;
            result[index++] = m.m03;

            result[index++] = m.m10;
            result[index++] = m.m11;
            result[index++] = m.m12;
            result[index++] = m.m13;

            result[index++] = m.m20;
            result[index++] = m.m21;
            result[index++] = m.m22;
            result[index++] = m.m23;

            result[index++] = m.m30;
            result[index++] = m.m31;
            result[index++] = m.m32;
            result[index++] = m.m33;

            return result;
        }

        public static implicit operator Matrix(double[] src)
        {
            var m = new Matrix();

            int index = 0;
            m.m00 = src[index++];
            m.m01 = src[index++];
            m.m02 = src[index++];
            m.m03 = src[index++];

            m.m10 = src[index++];
            m.m11 = src[index++];
            m.m12 = src[index++];
            m.m13 = src[index++];

            m.m20 = src[index++];
            m.m21 = src[index++];
            m.m22 = src[index++];
            m.m23 = src[index++];

            m.m30 = src[index++];
            m.m31 = src[index++];
            m.m32 = src[index++];
            m.m33 = src[index++];

            return m;
        }
    }
}
