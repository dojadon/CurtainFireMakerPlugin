using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Entities
{
    public struct ShotProperty
    {
        public int Color { get; }
        public float Red => (Color >> 16 & 0x000000FF) / 255.0F;
        public float Green => (Color >> 8 & 0x000000FF) / 255.0F;
        public float Blue => (Color >> 0 & 0x000000FF) / 255.0F;

        public ShotType Type { get; }

        public ShotProperty(ShotType type, int color)
        {
            Color = color;
            Type = type;
        }

        public override bool Equals(object obj) => obj is ShotProperty prop && Equals(prop);

        public bool Equals(ShotProperty p) => p.Color == Color && Type == p.Type;

        public override int GetHashCode()
        {
            int result = 17;
            result = result * 23 + Type.Name.GetHashCode();
            result = result * 23 + Color;
            return result;
        }

        public static bool operator ==(ShotProperty p1, ShotProperty p2) => p1.Color == p2.Color && p1.Type == p2.Type;
        public static bool operator !=(ShotProperty p1, ShotProperty p2) => p1.Color != p2.Color && p1.Type != p2.Type;
    }
}
