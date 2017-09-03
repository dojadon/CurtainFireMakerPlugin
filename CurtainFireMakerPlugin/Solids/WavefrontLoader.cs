using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VecMath;

namespace CurtainFireMakerPlugin.Solids
{
    public class WavefrontLoader
    {
        public static void GetVertices(string filename, Action<Vector3> action)
        {
            string text = File.ReadAllText(Plugin.Instance.Config.ResourceDirPath + "\\Wavefront\\" + filename);
            Parse(new StringReader(text), action);
        }

        private static void Parse(StringReader reader, Action<Vector3> action)
        {
            var set = new HashSet<Vector3>();

            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                string[] tokens = line.Split(' ');

                if (tokens[0] == "v")
                {
                    set.Add(new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                }
            }

            foreach(var vec in set)
            {
                action(vec);
            }
        }
    }
}
