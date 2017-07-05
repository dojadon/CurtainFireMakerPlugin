using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.ShotTypes;

namespace CurtainFireMakerPlugin.Entities
{
    public class ShotProperty
    {
        private readonly int color;
        public int Color => color;
        public float Red => (color >> 16 & 0x000000FF) / 255.0F;
        public float Green => (color >> 8 & 0x000000FF) / 255.0F;
        public float Blue => (color >> 0 & 0x000000FF) / 255.0F;

        private readonly ShotType type;
        public ShotType Type => type;

        public ShotProperty(string typeName, int color) : this(ShotTypeList.GetShotType(typeName), color)
        {
        }

        public ShotProperty(ShotType type, int color)
        {
            this.color = color;
            this.type = type;
        }
    }
}
