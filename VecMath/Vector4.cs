using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    [Serializable]
    public struct Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(Vector4 v1) : this(v1.x, v1.y, v1.z, v1.w)
        {

        }

        public static Vector4 Add(Vector4 v1, Vector4 v2) => new Vector4()
        {
            x = v1.x + v2.x,
            y = v1.y + v2.y,
            z = v1.z + v2.z,
            w = v1.w + v2.w
        };

        public static Vector4 Sub(Vector4 v1, Vector4 v2) => new Vector4()
        {
            x = v1.x - v2.x,
            y = v1.y - v2.y,
            z = v1.z - v2.z,
            w = v1.w - v2.w
        };

        public static Vector4 Scale(Vector4 v1, float d1) => new Vector4()
        {
            x = v1.x * d1,
            y = v1.y * d1,
            z = v1.z * d1,
            w = v1.w * d1
        };

        public static float Dot(Vector4 v1, Vector4 v2)
        {
            return v2.x * v1.x + v2.y * v1.y + v2.z * v1.z + v1.w * v2.w;
        }

        public static Vector4 Normalize(Vector4 v1)
        {
            float len = v1.Length();
            float mult = len != 1.0 && len != 0.0 ? 1.0F / len : 1.0F;

            return new Vector4(v1.x * mult, v1.y * mult, v1.z * mult, v1.w * mult);
        }

        public float Length() => (float)Math.Sqrt(x * x + y * y + z * z + w * w);

        public override bool Equals(object obj)
        {
            if (obj is Quaternion q)
            {
                return Equals(q);
            }
            return false;
        }

        public bool Equals(Vector4 v1) => v1.x == x && v1.y == y && v1.z == z && v1.w == w;

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

        public static Vector4 operator *(Vector4 v1, float d1) => Scale(v1, d1);

        public static Vector4 operator *(float d1, Vector4 v1) => Scale(v1, d1);

        public static float operator *(Vector4 v1, Vector4 v2) => Dot(v1, v2);

        public static Vector4 operator /(Vector4 v1, float d1) => Scale(v1, 1.0F / d1);

        public static explicit operator DxMath.Vector4(Vector4 v1) => new DxMath.Vector4(v1.x, v1.y, v1.z, v1.w);

        public static implicit operator Vector4(DxMath.Vector4 v1) => new Vector4(v1.X, v1.Y, v1.Z, v1.W);
    }
}
