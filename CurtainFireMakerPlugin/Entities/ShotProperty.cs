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

        public short ShotGroup { get; }

        public ShotProperty(ShotType type, int color) : this(type, color, short.MaxValue) { }

        public ShotProperty(ShotType type, int color, short group)
        {
            Color = color;
            ShotGroup = group;

            Type = type;
        }

        public override bool Equals(object obj) => obj is ShotProperty prop && Equals(prop);

        public bool Equals(ShotProperty p) => p.Color == Color && Type == p.Type && ShotGroup == p.ShotGroup;

        public bool IsGroupable(ShotProperty p) => p.Color == Color && Type == p.Type && (ShotGroup & p.ShotGroup) > 0;

        public override int GetHashCode()
        {
            int result = 17;
            result = result * 23 + Type.Name.GetHashCode();
            result = result * 23 + Color;
            result = result * 23 + ShotGroup;
            return result;
        }
    }
}
