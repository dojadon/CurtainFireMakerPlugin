using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin;
using MikuMikuPlugin;

namespace CurtainFireMaker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var plugin = new Plugin();

            plugin.Run(null);

            plugin.Dispose();
        }
    }
}
