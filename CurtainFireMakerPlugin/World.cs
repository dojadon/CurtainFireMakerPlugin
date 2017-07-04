using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; }
    }
}
