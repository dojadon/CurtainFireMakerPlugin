using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.ShotTypes;
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

        public short ShotGroup { get; }

        public ShotProperty(string typeName, int color) : this(typeName, color, short.MaxValue) { }

        public ShotProperty(string typeName, int color, short group)
        {
            Color = color;
            ShotGroup = group;

            if (ShotType.TypeDict.ContainsKey(typeName))
            {
                Type = ShotType.TypeDict[typeName];
            }
            else
            {
                throw new ArgumentException($"Not found shot type name : {typeName}");
            }
        }

        public override bool Equals(object obj) => obj is ShotProperty prop && Equals(prop);

        public bool Equals(ShotProperty p) => p.Color == Color && Type.Name == p.Type.Name && ShotGroup == p.ShotGroup;

        public bool GroupEquals(ShotProperty p) => p.Color == Color && Type.Name == p.Type.Name && (ShotGroup & p.ShotGroup) > 0;

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
