using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public class ShotTypeList
    {
        private static readonly List<ShotType> TYPE_LIST = new List<ShotType>();

        public static void Init(Action<List<ShotType>> action)
        {
            TYPE_LIST.Clear();

            TYPE_LIST.Add(new ShotTypeNone("NULL", false, false));
            TYPE_LIST.Add(new ShotTypeNone("BONE", false, true));

            action(TYPE_LIST);
        }

        public static ShotType GetShotType(string name)
        {
            return TYPE_LIST.Find(t => t.Name.Equals(name));
        }
    }
}
