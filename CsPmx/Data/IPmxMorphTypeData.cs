using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsPmx.Data
{
    public interface IPmxMorphTypeData : IPmxData
    {
        byte GetMorphType();
        int Index { get; set; }
    }
}
