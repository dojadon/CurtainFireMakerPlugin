using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPmx.Data
{
   public interface IPmxMorphTypeData : IPmxData
    {
        void SetIndices(int[] indices);
        byte GetMorphType();
    }
}
