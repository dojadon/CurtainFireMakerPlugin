using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
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

        public static Vector2 Add(Vector2 v1, Vector2 v2)
        {
            var v3 = new Vector2();

            v3.x = v1.x + v2.x;
            v3.y = v1.y + v2.y;

            return v3;
        }

        public static Vector2 Sub(Vector2 v1, Vector2 v2)
        {
            var v3 = new Vector2();

            v3.x = v1.x - v2.x;
            v3.y = v1.y - v2.y;

            return v3;
        }

        public static Vector2 Scale(Vector2 v1, float d1)
        {
            var v3 = new Vector2();

            v3.x = v1.x * d1;
            v3.y = v1.y * d1;

            return v3;
        }

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return v2.x * v1.x + v2.y * v1.y;
        }

        public static Vector2 Normalize(Vector2 v1)
        {
            var v2 = new Vector2();

            float len = Length(v1);

            if (len != 1.0 && len != 0.0)
            {
                v2.x = v1.x / len;
                v2.y = v1.y / len;
            }

            return v2;
        }

        public static float Length(Vector2 v1)
        {
            return (float)Math.Sqrt(v1.x * v1.x + v1.y * v1.y);
        }

        public float Length()
        {
            return Length(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as Vector2?;
            if ((System.Object)p == null)
            {
                return false;
            }

            return this.Equals((Vector2)obj);
        }

        public bool Equals(Vector2 v1) => v1.x == this.x && v1.y == this.y;

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2;
        }

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
