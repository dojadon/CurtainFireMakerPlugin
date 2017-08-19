using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    [Serializable]
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(Vector2 v1) : this(v1.x, v1.y)
        {

        }

        public static Vector2 Add(Vector2 v1, Vector2 v2) => new Vector2()
        {
            x = v1.x + v2.x,
            y = v1.y + v2.y
        };

        public static Vector2 Sub(Vector2 v1, Vector2 v2) => new Vector2()
        {
            x = v1.x - v2.x,
            y = v1.y - v2.y
        };

        public static Vector2 Scale(Vector2 v1, float d1) => new Vector2()
        {
            x = v1.x * d1,
            y = v1.y * d1
        };

        public static float Dot(Vector2 v1, Vector2 v2) => v2.x * v1.x + v2.y * v1.y;

        public static Vector2 Normalize(Vector2 v1)
        {
            var v2 = new Vector2();

            float len = v1.Length();

            if (len != 1.0 && len != 0.0)
            {
                v2.x = v1.x / len;
                v2.y = v1.y / len;
            }

            return v2;
        }

        public float Length() => (float)Math.Sqrt(x * x + y * y);

        public static bool EpsilonEquals(Vector2 v1, Vector2 v2, float epsilon)
        {
            float diff;
            diff = v1.x - v2.x;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = v1.y - v2.y;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2 v)
            {
                return Equals(v);
            }
            return false;
        }

        public bool Equals(Vector2 v1) => v1.x == x && v1.y == y;

        public override string ToString() => $"[{x}, {y}]";

        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() << 2;

        public static Vector2 operator -(Vector2 v1) => v1 * -1;

        public static Vector2 operator +(Vector2 v1) => Normalize(v1);

        public static bool operator ==(Vector2 v1, Vector2 v2) => v1.x == v2.x && v1.y == v2.y;

        public static bool operator !=(Vector2 v1, Vector2 v2) => !(v1.x == v2.x && v1.y == v2.y);

        public static Vector2 operator +(Vector2 v1, Vector2 v2) => Add(v1, v2);

        public static Vector2 operator -(Vector2 v1, Vector2 v2) => Sub(v1, v2);

        public static Vector2 operator *(Vector2 v1, float d1) => Scale(v1, d1);

        public static float operator *(Vector2 v1, Vector2 v2) => Dot(v1, v2);

        public static explicit operator DxMath.Vector2(Vector2 v1) => new DxMath.Vector2((float)v1.x, (float)v1.y);

        public static implicit operator Vector2(DxMath.Vector2 v1) => new Vector2(v1.X, v1.Y);
    }
}
