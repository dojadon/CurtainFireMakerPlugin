using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public class ShotTypeList
    {
        private readonly List<ShotType> TYPE_LIST = new List<ShotType>();

        public static readonly ShotTypeList Instance = new ShotTypeList();

        private ShotTypeList()
        {
            TYPE_LIST.Add(new ShotTypeNone("NULL", false, false));
            TYPE_LIST.Add(new ShotTypeNone("BONE", false, true));
        }

        public static void AddShotType(ShotType type)
        {
            Instance.TYPE_LIST.Add(type);
        }

        public static ShotType GetShotType(string name)
        {
            return Instance.TYPE_LIST.Find(t => t.Name.Equals(name));
        }
    }
}
