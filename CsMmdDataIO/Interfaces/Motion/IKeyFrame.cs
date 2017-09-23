using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Interfaces.Motion
{
    public interface IKeyFrame
    {
        long FrameTime { get; set; }
    }
}
