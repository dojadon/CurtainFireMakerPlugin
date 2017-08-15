using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    [Serializable]
    public struct Matrix4
    {
        public static readonly Matrix4 Identity = new Matrix4(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, Vector3.Zero);

        public float m00;
        public float m01;
        public float m02;
        public float m03;
        public float m10;
        public float m11;
        public float m12;
        public float m13;
        public float m20;
        public float m21;
        public float m22;
        public float m23;
        public float m30;
        public float m31;
        public float m32;
        public float m33;

        public Vector3 TransformVec
        {
            get => new Vector3(m30, m31, m32);
            set
            {
                m30 = value.x;
                m31 = value.y;
                m32 = value.z;
            }
        }

        public Matrix4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21,
            float m22, float m23, float m30, float m31, float m32, float m33)
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

        public Matrix4(Vector3 x, Vector3 y, Vector3 z, Vector3 trans)
        {
            this.m00 = x.x;
            this.m10 = x.y;
            this.m20 = x.z;
            this.m30 = trans.x;

            this.m01 = y.x;
            this.m11 = y.y;
            this.m21 = y.z;
            this.m31 = trans.y;

            this.m02 = z.x;
            this.m12 = z.y;
            this.m22 = z.z;
            this.m32 = trans.z;

            this.m03 = 0;
            this.m13 = 0;
            this.m23 = 0;
            this.m33 = 1;
        }

        public Matrix4(Vector3 x, Vector3 y, Vector3 z) : this(x, y, z, Vector3.Zero)
        {
        }

        public static Matrix4 Translation(Matrix3 m1, Vector3 trans)
        {
            Matrix4 m2 = m1;
            m2.TransformVec += trans;
            return m2;
        }

        public static Matrix4 RotationQuaternion(Quaternion q)
        {
            if (q == Quaternion.Identity)
            {
                return Identity;
            }
            float xx = q.x * q.x;
            float yy = q.y * q.y;
            float zz = q.z * q.z;

            return new Matrix4()
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

                m33 = 1
            };
        }

        public static Matrix4 RotationAxis(Vector3 axis, float angle)
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

            return new Matrix4(x, y, z);
        }

        public static Matrix4 LookAt(Vector3 forward, Vector3 upward)
        {
            var z = -forward;
            var x = +(upward ^ z);
            var y = +(z ^ x);

            return new Matrix4(x, y, z);
        }

        public static Matrix4 Mul(Matrix4 m1, Matrix4 m2) => new Matrix4()
        {
            m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20 + m1.m03 * m2.m30,
            m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21 + m1.m03 * m2.m31,
            m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22 + m1.m03 * m2.m32,
            m03 = m1.m00 * m2.m03 + m1.m01 * m2.m13 + m1.m02 * m2.m23 + m1.m03 * m2.m33,

            m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20 + m1.m13 * m2.m30,
            m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21 + m1.m13 * m2.m31,
            m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22 + m1.m13 * m2.m32,
            m13 = m1.m10 * m2.m03 + m1.m11 * m2.m13 + m1.m12 * m2.m23 + m1.m13 * m2.m33,

            m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20 + m1.m23 * m2.m30,
            m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21 + m1.m23 * m2.m31,
            m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22 + m1.m23 * m2.m32,
            m23 = m1.m20 * m2.m03 + m1.m21 * m2.m13 + m1.m22 * m2.m23 + m1.m23 * m2.m33,

            m30 = m1.m30 * m2.m00 + m1.m31 * m2.m10 + m1.m32 * m2.m20 + m1.m33 * m2.m30,
            m31 = m1.m30 * m2.m01 + m1.m31 * m2.m11 + m1.m32 * m2.m21 + m1.m33 * m2.m31,
            m32 = m1.m30 * m2.m02 + m1.m31 * m2.m12 + m1.m32 * m2.m22 + m1.m33 * m2.m32,
            m33 = m1.m30 * m2.m03 + m1.m31 * m2.m13 + m1.m32 * m2.m23 + m1.m33 * m2.m33
        };

        public static Matrix4 Pow(Matrix4 m1, float exponent)
        {
            return ((Quaternion)m1) ^ exponent;
        }

        public static Matrix4 Inverse(Matrix4 m1) => Translation(~(Matrix3)m1, m1.TransformVec);

        public static Matrix4 Transpose(Matrix4 m1) => new Matrix4()
        {
            m00 = m1.m00,
            m01 = m1.m10,
            m02 = m1.m20,
            m03 = m1.m30,

            m10 = m1.m01,
            m11 = m1.m11,
            m12 = m1.m21,
            m13 = m1.m31,

            m20 = m1.m02,
            m21 = m1.m12,
            m22 = m1.m22,
            m23 = m1.m32,

            m30 = m1.m03,
            m31 = m1.m13,
            m32 = m1.m23,
            m33 = m1.m33
        };

        public static Vector4 Transform(Vector4 v1, Matrix4 m1) => new Vector4()
        {
            x = v1.x * m1.m00 + v1.y * m1.m10 + v1.z * m1.m20 + v1.w * m1.m30,
            y = v1.x * m1.m01 + v1.y * m1.m11 + v1.z * m1.m21 + v1.w * m1.m31,
            z = v1.x * m1.m02 + v1.y * m1.m12 + v1.z * m1.m22 + v1.w * m1.m32,
            w = v1.x * m1.m03 + v1.y * m1.m13 + v1.z * m1.m23 + v1.w * m1.m33
        };

        public static Vector3 Transform(Vector3 v1, Matrix4 m1) => new Vector3()
        {
            x = v1.x * m1.m00 + v1.y * m1.m10 + v1.z * m1.m20,
            y = v1.x * m1.m01 + v1.y * m1.m11 + v1.z * m1.m21,
            z = v1.x * m1.m02 + v1.y * m1.m12 + v1.z * m1.m22
        };

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("[");
            sb.Append(this.m00).Append(", ").Append(this.m01).Append(", ").Append(this.m02).Append(", ").Append(this.m03);
            sb.Append("]\n[");
            sb.Append(this.m10).Append(", ").Append(this.m11).Append(", ").Append(this.m12).Append(", ").Append(this.m13);
            sb.Append("]\n[");
            sb.Append(this.m20).Append(", ").Append(this.m21).Append(", ").Append(this.m22).Append(", ").Append(this.m23);
            sb.Append("]\n[");
            sb.Append(this.m30).Append(", ").Append(this.m31).Append(", ").Append(this.m32).Append(", ").Append(this.m33);
            sb.Append("]");

            return sb.ToString();
        }

        public static bool EpsilonEquals(Matrix4 m1, Matrix4 m2, float epsilon)
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

        public static Matrix4 operator ~(Matrix4 m1) => Inverse(m1);

        public static Matrix4 operator *(Matrix4 m1, Matrix4 m2) => Mul(m1, m2);

        public static Vector4 operator *(Vector4 v1, Matrix4 m1) => Transform(v1, m1);

        public static Vector3 operator *(Vector3 v1, Matrix4 m1) => Transform(v1, m1);

        public static Matrix4 operator ^(Matrix4 m1, float d1) => Pow(m1, d1);

        public static implicit operator Matrix4(Matrix3 m) => new Matrix4()
        {
            m00 = m.m00,
            m01 = m.m01,
            m02 = m.m02,
            m03 = 0,
            m10 = m.m10,
            m11 = m.m11,
            m12 = m.m12,
            m13 = 0,
            m20 = m.m20,
            m21 = m.m21,
            m22 = m.m22,
            m23 = 0,
            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,
        };

        public static implicit operator Matrix4(Quaternion q1) => RotationQuaternion(q1);

        public static explicit operator float[] (Matrix4 m)
        {
            var result = new float[16];

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

        public static implicit operator Matrix4(float[] src)
        {
            int index = 0;
            return new Matrix4()
            {
                m00 = src[index++],
                m01 = src[index++],
                m02 = src[index++],
                m03 = src[index++],

                m10 = src[index++],
                m11 = src[index++],
                m12 = src[index++],
                m13 = src[index++],

                m20 = src[index++],
                m21 = src[index++],
                m22 = src[index++],
                m23 = src[index++],

                m30 = src[index++],
                m31 = src[index++],
                m32 = src[index++],
                m33 = src[index++]
            };
        }

        public static implicit operator Matrix4(float[,] d)
        {
            var m = new Matrix4();

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

        public static explicit operator float[,] (Matrix4 m)
        {
            float[,] result = new float[4, 4];

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
    }
}
