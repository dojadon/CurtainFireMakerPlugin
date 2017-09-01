using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace SolidMath
{
    public struct Face
    {
        public Vector3 Pos0 { get; set; }
        public Vector3 Pos1 { get; set; }
        public Vector3 Pos2 { get; set; }

        public Vector3 this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Pos0;
                    case 1: return Pos1;
                    case 2: return Pos2;
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                switch (index)
                {
                    case 0: Pos0 = value; return;
                    case 1: Pos1 = value; return;
                    case 2: Pos2 = value; return;
                }
                throw new IndexOutOfRangeException();
            }
        }

        public Face(Vector3 pos0, Vector3 pos1, Vector3 pos2)
        {
            Pos0 = pos0;
            Pos1 = pos1;
            Pos2 = pos2;
        }

        public override bool Equals(object obj) => (obj is Face f) ? Equals(f) : false;

        public bool Equals(Face f)
        {
            for (int i = 0; i < 3; i++)
            {
                if (this[i] != f[i]) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int result = 17;
            for (int i = 0; i < 3; i++)
            {
                result = 31 * result + this[i].GetHashCode();
            }
            return result;
        }
    }
}
