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
            action(TYPE_LIST);
        }
    }
}
