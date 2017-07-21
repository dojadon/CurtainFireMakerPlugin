using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsVmd.Data
{
    internal interface IVmdData
    {
        void Export(VmdExporter exporter);
    }
}
