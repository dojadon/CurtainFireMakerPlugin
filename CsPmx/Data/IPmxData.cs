using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsPmx.Data
{
    public interface IPmxData
    {
        void Export(PmxExporter exporter);
        void Parse(PmxParser parser);
    }
}
