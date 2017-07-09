using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Mathematics
{
    public struct Matrix
    {
        public static readonly Matrix Identity = new Matrix(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, Vector3.Zero);

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

        public Matrix(Vector3 x, Vector3 y, Vector3 z, Vector3 trans)
        {
            this.m00 = x.x;
            this.m01 = x.y;
            this.m02 = x.z;
            this.m03 = trans.x;

            this.m10 = y.x;
            this.m11 = y.y;
            this.m12 = y.z;
            this.m13 = trans.y;

            this.m20 = z.x;
            this.m21 = z.y;
            this.m22 = z.z;
            this.m23 = trans.z;

            this.m30 = 0;
            this.m31 = 0;
            this.m32 = 0;
            this.m33 = 1;
        }

        public Matrix(Vector3 x, Vector3 y, Vector3 z) : this(x, y, z, Vector3.Zero)
        {
        }

        public static Matrix RotationQuaternion(Quaternion q)
        {
            var m1 = Identity;

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

        public static Matrix RotationAxisAngle(Vector3 axis, double angle)
        {
            var m1 = Identity;

            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            Vector3 x = Vector3.UnitX;
            Vector3 n = axis.x * axis;
            x = cos * (x - n) + sin * (axis ^ x) + n;

            Vector3 y = Vector3.UnitY;
            n = axis.y * axis;
            y = cos * (y - n) + sin * (axis ^ y) + n;

            Vector3 z = Vector3.UnitZ;
            n = axis.z * axis;
            z = cos * (z - n) + sin * (axis ^ z) + n;

            return new Matrix(x, y, z);
        }

        public static Matrix LookAt(Vector3 forward, Vector3 upward)
        {
            var side = +(upward ^ forward);
            upward = +(forward ^ side);

            return Transpose(new Matrix(side, upward, forward));
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

        public static Matrix Inverse(Matrix m1)
        {
            Matrix m2 = Identity;

            double[,] mat = (double[,])m1;
            double[,] inv = (double[,])Identity;
            double buf = 0;

            for (int i = 0; i < 4; i++)
            {
                buf = 1 / mat[i, i];
                for (int j = 0; j < 4; j++)
                {
                    mat[i, j] *= buf;
                    inv[i, j] *= buf;
                }
                for (int j = 0; j < 4; j++)
                {
                    if (i != j)
                    {
                        buf = mat[j, i];
                        for (int k = 0; k < 4; k++)
                        {
                            mat[j, k] -= mat[i, k] * buf;
                            inv[j, k] -= inv[i, k] * buf;
                        }
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                buf = 1 / mat[i, i];
                for (int j = 0; j < 4; j++)
                {
                    mat[i, j] *= buf; inv[i, j] *= buf;
                }
                for (int j = 0; j < 4; j++)
                {
                    if (i != j)
                    {
                        buf = mat[j, i];
                        for (int k = 0; k < 4; k++)
                        {
                            mat[j, k] -= mat[i, k] * buf;
                            inv[j, k] -= inv[i, k] * buf;
                        }
                    }
                }
            }
            return inv;
        }

        public static Matrix Transpose(Matrix m1)
        {
            var m2 = new Matrix();

            m2.m00 = m1.m00;
            m2.m01 = m1.m10;
            m2.m02 = m1.m20;
            m2.m03 = m1.m30;

            m2.m10 = m1.m01;
            m2.m11 = m1.m11;
            m2.m12 = m1.m21;
            m2.m13 = m1.m31;

            m2.m20 = m1.m02;
            m2.m21 = m1.m12;
            m2.m22 = m1.m22;
            m2.m23 = m1.m32;

            m2.m30 = m1.m03;
            m2.m31 = m1.m13;
            m2.m32 = m1.m23;
            m2.m33 = m1.m33;

            return m2;
        }

        public static Vector4 Transform(Vector4 v1, Matrix m1)
        {
            var v2 = new Vector4();

            v2.x = v1.x * m1.m00 + v1.y * m1.m01 + v1.z * m1.m02 + v1.w * m1.m03;
            v2.y = v1.x * m1.m10 + v1.y * m1.m11 + v1.z * m1.m12 + v1.w * m1.m13;
            v2.z = v1.x * m1.m20 + v1.y * m1.m21 + v1.z * m1.m22 + v1.w * m1.m23;
            v2.w = v1.x * m1.m30 + v1.y * m1.m31 + v1.z * m1.m32 + v1.w * m1.m33;

            return v2;
        }

        public static Vector3 Transform(Vector3 v1, Matrix m1)
        {
            var v2 = new Vector3();

            v2.x = v1.x * m1.m00 + v1.y * m1.m01 + v1.z * m1.m02;
            v2.y = v1.x * m1.m10 + v1.y * m1.m11 + v1.z * m1.m12;
            v2.z = v1.x * m1.m20 + v1.y * m1.m21 + v1.z * m1.m22;

            return v2;
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

        public static Vector4 operator *(Vector4 v1, Matrix m1) => Transform(v1, m1);

        public static Vector3 operator *(Vector3 v1, Matrix m1) => Transform(v1, m1);

        public static implicit operator Matrix(Quaternion q1) => RotationQuaternion(q1);

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

        public static implicit operator Matrix(double[,] d)
        {
            var m = new Matrix();

            int index1 = 0;
            int index2 = 0;
            m.m00 = d[index2, index1++];
            m.m01 = d[index2, index1++];
            m.m02 = d[index2, index1++];
            m.m03 = d[index2++, index1++];

            index1 = 0;
            m.m10 = d[index2, index1++];
            m.m11 = d[index2, index1++];
            m.m12 = d[index2, index1++];
            m.m13 = d[index2++, index1++];

            index1 = 0;
            m.m20 = d[index2, index1++];
            m.m21 = d[index2, index1++];
            m.m22 = d[index2, index1++];
            m.m23 = d[index2++, index1++];

            index1 = 0;
            m.m30 = d[index2, index1++];
            m.m31 = d[index2, index1++];
            m.m32 = d[index2, index1++];
            m.m33 = d[index2++, index1++];

            return m;
        }

        public static explicit operator double[,] (Matrix m)
        {
            double[,] result = new double[4, 4];

            int index1 = 0;
            int index2 = 0;
            result[index2, index1++] = m.m00;
            result[index2, index1++] = m.m01;
            result[index2, index1++] = m.m02;
            result[index2++, index1++] = m.m03;

            index1 = 0;
            result[index2, index1++] = m.m10;
            result[index2, index1++] = m.m11;
            result[index2, index1++] = m.m12;
            result[index2++, index1++] = m.m13;

            index1 = 0;
            result[index2, index1++] = m.m20;
            result[index2, index1++] = m.m21;
            result[index2, index1++] = m.m22;
            result[index2++, index1++] = m.m23;

            index1 = 0;
            result[index2, index1++] = m.m30;
            result[index2, index1++] = m.m31;
            result[index2, index1++] = m.m32;
            result[index2++, index1++] = m.m33;

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
