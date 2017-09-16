using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.ShotTypes;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class ShotProperty
    {
        public int Color { get; }
        public float Red => (Color >> 16 & 0x000000FF) / 255.0F;
        public float Green => (Color >> 8 & 0x000000FF) / 255.0F;
        public float Blue => (Color >> 0 & 0x000000FF) / 255.0F;

        public ShotType Type { get; }

        public ShotProperty(string typeName, int color)
        {
            Color = color;
            Type = ShotTypeList.GetShotType(typeName);
        }

        public override bool Equals(object obj) => obj is ShotProperty prop && Equals(prop);

        public bool Equals(ShotProperty p) => p.Color == Color && Type.Name == p.Type.Name;

        public override int GetHashCode()
        {
            return Type.Name.GetHashCode() << 24 | Color;
        }
    }
}
