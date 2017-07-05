using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsVmd.Data
{
    interface IVmdData
    {
        void Export(VmdExporter exporter);
    }
}
