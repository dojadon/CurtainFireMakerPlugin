using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    [Serializable]
    public struct Matrix3
    {
        public static readonly Matrix3 Identity = new Matrix3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        public float m00;
        public float m01;
        public float m02;
        public float m10;
        public float m11;
        public float m12;
        public float m20;
        public float m21;
        public float m22;

        public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;

            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;

            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
        }

        public Matrix3(Vector3 x, Vector3 y, Vector3 z)
        {
            this.m00 = x.x;
            this.m10 = x.y;
            this.m20 = x.z;

            this.m01 = y.x;
            this.m11 = y.y;
            this.m21 = y.z;

            this.m02 = z.x;
            this.m12 = z.y;
            this.m22 = z.z;
        }

        public static Matrix3 RotationQuaternion(Quaternion q)
        {
            if (q == Quaternion.Identity)
            {
                return Identity;
            }
            float xx = q.x * q.x;
            float yy = q.y * q.y;
            float zz = q.z * q.z;

            return new Matrix3()
            {
                m00 = 1 - 2 * (yy + zz),
                m01 = 2 * (q.x * q.y + q.w * q.z),
                m02 = 2 * (q.x * q.z - q.w * q.y),

                m10 = 2 * (q.x * q.y - q.w * q.z),
                m11 = 1 - 2 * (zz + xx),
                m12 = 2 * (q.y * q.z + q.w * q.x),

                m20 = 2 * (q.x * q.z + q.w * q.y),
                m21 = 2 * (q.y * q.z - q.w * q.x),
                m22 = 1 - 2 * (xx + yy),
            };
        }

        public static Matrix3 RotationAxis(Vector3 axis, float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            var x = Vector3.UnitX;
            var n = axis.x * axis;
            x = cos * (x - n) + sin * (axis ^ x) + n;

            var y = Vector3.UnitY;
            n = axis.y * axis;
            y = cos * (y - n) + sin * (axis ^ y) + n;

            var z = Vector3.UnitZ;
            n = axis.z * axis;
            z = cos * (z - n) + sin * (axis ^ z) + n;

            return new Matrix3(x, y, z);
        }

        public static Matrix3 LookAt(Vector3 forward, Vector3 upward)
        {
            var z = -forward;
            var x = +(upward ^ z);
            var y = +(z ^ x);

            return new Matrix3(x, y, z);
        }

        public static Matrix3 Mul(Matrix3 m1, Matrix3 m2) => new Matrix3()
        {
            m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20,
            m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21,
            m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22,

            m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20,
            m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21,
            m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22,

            m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20,
            m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21,
            m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22,
        };

        public static Matrix3 Mul(Matrix3 m, float f) => new Matrix3()
        {
            m00 = m.m00 * f,
            m01 = m.m01 * f,
            m02 = m.m02 * f,
            m10 = m.m10 * f,
            m11 = m.m11 * f,
            m12 = m.m12 * f,
            m20 = m.m20 * f,
            m21 = m.m21 * f,
            m22 = m.m22 * f,
        };

        public static Matrix3 Pow(Matrix3 m, float exponent)
        {
            return ((Quaternion)m) ^ exponent;
        }

        public static Matrix3 Inverse(Matrix3 m)
        {
            float det = m.Det();

            if (det == 0)
            {
                throw new ArithmeticException("Determinant is 0");
            }

            return new Matrix3()
            {
                m00 = m.m11 * m.m22 - m.m12 * m.m21,
                m01 = -m.m01 * m.m22 + m.m02 * m.m21,
                m02 = m.m01 * m.m12 - m.m02 * m.m11,
                m10 = -m.m10 * m.m22 + m.m12 * m.m20,
                m11 = m.m00 * m.m22 - m.m02 * m.m20,
                m12 = -m.m00 * m.m12 + -m.m02 * m.m10,
                m20 = m.m10 * m.m21 - m.m11 * m.m20,
                m21 = -m.m00 * m.m21 + m.m01 * m.m20,
                m22 = m.m00 * m.m11 - m.m01 * m.m10,
            } * (1.0F / det);
        }

        public static Matrix3 Transpose(Matrix3 m1) => new Matrix4()
        {
            m00 = m1.m00,
            m01 = m1.m10,
            m02 = m1.m20,
            m10 = m1.m01,
            m11 = m1.m11,
            m12 = m1.m21,
            m20 = m1.m02,
            m21 = m1.m12,
            m22 = m1.m22,
        };

        public static Vector3 Transform(Vector3 v1, Matrix3 m1) => new Vector3()
        {
            x = v1.x * m1.m00 + v1.y * m1.m10 + v1.z * m1.m20,
            y = v1.x * m1.m01 + v1.y * m1.m11 + v1.z * m1.m21,
            z = v1.x * m1.m02 + v1.y * m1.m12 + v1.z * m1.m22
        };

        public static bool EpsilonEquals(Matrix3 m1, Matrix3 m2, float epsilon)
        {
            float diff;
            float[] f1 = (float[])m1;
            float[] f2 = (float[])m2;

            for (int i = 0; i < 16; i++)
            {
                diff = f1[i] - f2[i];
                if ((diff < 0 ? -diff : diff) > epsilon) return false;
            }
            return true;
        }

        public float Det() => m00 * m11 * m22 + m01 * m12 * m21 + m02 * m10 * m21 - m02 * m11 * m21 - m01 * m10 * m22 - m00 * m12 * m21;

        public override string ToString() => $"[{m00}, {m01}, {m02}]\n[{m10}, {m11}, {m12}]\n[{m20}, {m21}, {m22}]";

        public static Matrix3 operator ~(Matrix3 m1) => Inverse(m1);

        public static Matrix3 operator *(Matrix3 m1, Matrix3 m2) => Mul(m1, m2);

        public static Matrix3 operator *(Matrix3 m1, float f1) => Mul(m1, f1);

        public static Vector3 operator *(Vector3 v1, Matrix3 m1) => Transform(v1, m1);

        public static Matrix3 operator ^(Matrix3 m1, float d1) => Pow(m1, d1);

        public static implicit operator Matrix3(Quaternion q1) => RotationQuaternion(q1);

        public static implicit operator Matrix3(Matrix4 m) => new Matrix3(m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22);

        public static explicit operator float[] (Matrix3 m)
        {
            var result = new float[16];

            int index = 0;
            result[index++] = m.m00;
            result[index++] = m.m01;
            result[index++] = m.m02;

            result[index++] = m.m10;
            result[index++] = m.m11;
            result[index++] = m.m12;

            result[index++] = m.m20;
            result[index++] = m.m21;
            result[index++] = m.m22;

            return result;
        }

        public static implicit operator Matrix3(float[] src)
        {
            int index = 0;
            return new Matrix3()
            {
                m00 = src[index++],
                m01 = src[index++],
                m02 = src[index++],

                m10 = src[index++],
                m11 = src[index++],
                m12 = src[index++],

                m20 = src[index++],
                m21 = src[index++],
                m22 = src[index++],
            };
        }
    }
}
