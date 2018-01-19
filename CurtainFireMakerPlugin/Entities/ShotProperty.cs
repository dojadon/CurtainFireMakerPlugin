using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public struct ShotProperty
    {
        public int Color { get; }
        public float Red => (Color >> 16 & 0x000000FF) / 255.0F;
        public float Green => (Color >> 8 & 0x000000FF) / 255.0F;
        public float Blue => (Color >> 0 & 0x000000FF) / 255.0F;

        public ShotType Type { get; }

        public Matrix4 Scale { get; set; }

        public ShotProperty(ShotType type, int color, Matrix4 scale)
        {
            Color = color;
            Type = type;
            Scale = scale;
        }

        public override bool Equals(object obj) => obj is ShotProperty prop && Equals(prop);

        public bool Equals(ShotProperty p) => p.Color == Color && Type == p.Type && Scale == p.Scale;

        public override int GetHashCode()
        {
            int result = 17;
            result = result * 23 + Type.Name.GetHashCode();
            result = result * 23 + Color;
            result = result * 23 + Scale.GetHashCode();

            return result;
        }

        public static bool EplisionEquals(ShotProperty p1, ShotProperty p2, float epsilon)
        {
            return p1.Color == p2.Color && p1.Type == p2.Type && Matrix4.EpsilonEquals(p1.Scale, p2.Scale, epsilon);
        }

        public static bool operator ==(ShotProperty p1, ShotProperty p2) => p1.Color == p2.Color && p1.Type == p2.Type && p1.Scale == p2.Scale;
        public static bool operator !=(ShotProperty p1, ShotProperty p2) => p1.Color != p2.Color && p1.Type != p2.Type && p1.Scale == p2.Scale;
    }
}
