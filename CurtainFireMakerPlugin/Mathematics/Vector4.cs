using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Mathematics
{
    public struct Vector4
    {
        public double x;
        public double y;
        public double z;
        public double w;

        public Vector4(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(Vector4 v1) : this(v1.x, v1.y, v1.z, v1.w)
        {

        }

        public static Vector4 Add(Vector4 v1, Vector4 v2)
        {
            var v3 = new Vector4();

            v3.x = v1.x + v2.x;
            v3.y = v1.y + v2.y;
            v3.z = v1.z + v2.z;
            v3.w = v1.w + v2.w;

            return v3;
        }

        public static Vector4 Sub(Vector4 v1, Vector4 v2)
        {
            var v3 = new Vector4();

            v3.x = v1.x - v2.x;
            v3.y = v1.y - v2.y;
            v3.z = v1.z - v2.z;
            v3.w = v1.w - v2.w;

            return v3;
        }

        public static Vector4 Scale(Vector4 v1, double d1)
        {
            var v3 = new Vector4();

            v3.x = v1.x * d1;
            v3.y = v1.y * d1;
            v3.z = v1.z * d1;
            v3.w = v1.w * d1;

            return v3;
        }

        public static double Dot(Vector4 v1, Vector4 v2)
        {
            return v2.x * v1.x + v2.y * v1.y + v2.z * v1.z + v1.w * v2.w;
        }

        public static Vector4 Transform(Matrix m1, Vector4 v1)
        {
            var v2 = new Vector4();

            v2.x = m1.m00 * v1.x + m1.m01 * v1.y + m1.m02 * v1.z + m1.m03 * v1.w;
            v2.y = m1.m10 * v1.x + m1.m11 * v1.y + m1.m12 * v1.z + m1.m13 * v1.w;
            v2.z = m1.m20 * v1.x + m1.m21 * v1.y + m1.m22 * v1.z + m1.m23 * v1.w;

            return v2;
        }

        public static Vector4 Normalize(Vector4 v1)
        {
            var v2 = new Vector4();

            double len = Length(v1);

            if (len != 1.0 && len != 0.0)
            {
                v2.x = v1.x / len;
                v2.y = v1.y / len;
                v2.z = v1.z / len;
                v2.w = v1.w / len;
            }

            return v2;
        }

        public static double Length(Vector4 v1)
        {
            return Math.Sqrt(v1.x * v1.x + v1.y * v1.y + v1.z * v1.z + v1.w * v1.w);
        }

        public double Length()
        {
            return Length(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as Vector4?;
            if ((System.Object)p == null)
            {
                return false;
            }

            return this.Equals((Vector4)obj);
        }

        public bool Equals(Vector4 v1) => v1.x == this.x && v1.y == this.y && v1.z == this.z && v1.w == this.w;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                hashCode = (hashCode * 397) ^ w.GetHashCode();
                return hashCode;
            }
        }

        public static Vector4 operator -(Vector4 v1) => v1 * -1;

        public static Vector4 operator +(Vector4 v1) => Normalize(v1);

        public static bool operator ==(Vector4 v1, Vector4 v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w;

        public static bool operator !=(Vector4 v1, Vector4 v2) => !(v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);

        public static Vector4 operator +(Vector4 v1, Vector4 v2) => Add(v1, v2);

        public static Vector4 operator -(Vector4 v1, Vector4 v2) => Sub(v1, v2);

        public static Vector4 operator *(Vector4 v1, double d1) => Scale(v1, d1);

        public static double operator *(Vector4 v1, Vector4 v2) => Dot(v1, v2);

        public static Vector4 operator *(Matrix m1, Vector4 v1) => Transform(m1, v1);

        public static Vector4 operator /(Vector4 v1, double d1) => Scale(v1, 1.0 / d1);

        public static explicit operator DxMath.Vector4(Vector4 v1) => new DxMath.Vector4((float)v1.x, (float)v1.y, (float)v1.z, (float)v1.w);

        public static implicit operator Vector4(DxMath.Vector4 v1) => new Vector4(v1.X, v1.Y, v1.Z, v1.W);
    }
}
