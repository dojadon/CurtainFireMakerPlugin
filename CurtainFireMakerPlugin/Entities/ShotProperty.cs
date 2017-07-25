﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.ShotTypes;
using CurtainFireMakerPlugin.Mathematics;

namespace CurtainFireMakerPlugin.Entities
{
    public class ShotProperty
    {
        public int Color { get; }
        public float Red => (Color >> 16 & 0x000000FF) / 255.0F;
        public float Green => (Color >> 8 & 0x000000FF) / 255.0F;
        public float Blue => (Color >> 0 & 0x000000FF) / 255.0F;

        public ShotType Type { get; }
        public Vector3 VertexScale { get; set; }
        public Vector3 VertexOffset { get; set; }

        public ShotProperty(string typeName, int color)
        {
            this.Color = color;
            this.Type = ShotTypeList.GetShotType(typeName);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as ShotProperty;
            if (p == null)
            {
                return false;
            }

            return this.Equals((ShotProperty)obj);
        }

        public bool Equals(ShotProperty p)
        {
            return p.Color == Color && Type.Name == p.Type.Name && VertexScale == p.VertexScale;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
