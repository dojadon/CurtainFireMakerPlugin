using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using VecMath;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public class ShotTypePmxL : ShotTypePmx
    {
        private static Dictionary<int, Image> ImageDict { get; } = new Dictionary<int, Image>();

        public ShotTypePmxL(string name, string path, Vector3 size) : base(name, path, size)
        {
            InitModelData = data =>
           {
               var prop = data.Property;

               foreach (var material in data.Materials)
               {
                   material.Diffuse = new Vector4(prop.Red, prop.Green, prop.Blue, 1);
                   material.Ambient = new Vector3(prop.Red, prop.Green, prop.Blue);
               }

               var texPath = AppendFileName(data.Textures[0], Convert.ToString(prop.Color, 16));

               if (!ImageDict.ContainsKey(prop.Color))
               {
                   var image = new Bitmap(Plugin.Instance.Config.ResourceDirPath + "\\" + data.Textures[0]);
                   this.SetPxcelColor(image, prop.Red, prop.Green, prop.Blue);
                   ImageDict.Add(prop.Color, image);

                   data.World.Export += (w, e) =>
                   {
                       var config = Plugin.Instance.Config;

                       string exportPath = config.ExportDirPath + "\\" + texPath;
                       File.Delete(exportPath);

                       image.Save(exportPath);
                       image.Dispose();
                   };
               }

               data.Textures[0] = AppendFileName(data.Textures[0], Convert.ToString(prop.Color, 16));
           };
        }

        private string AppendFileName(string path, string str)
        {
            string ext = Path.GetExtension(path);
            return path.Substring(0, path.Length - ext.Length - 1) + str + ext;
        }

        private void SetPxcelColor(Bitmap image, float r, float g, float b)
        {
            var color = new Vector3(r, g, b);
            var sub = new Vector3(1, 1, 1) - color;

            for (int x = 0; x < image.Width; x += 2)
            {
                for (int y = 0; y < image.Height; y += 2)
                {
                    Color src = image.GetPixel(x, y);
                    float subScale = 1;

                    float dis = (float)Math.Sqrt(x * x + y * y);

                    if (dis < 0.8)
                    {
                        subScale = 0;
                    }
                    else if (0.8 < dis && dis < 0.9)
                    {
                        subScale = (dis - 0.8F) * 10;
                    }

                    image.SetPixel(x, y, ColorScale(image.GetPixel(x, y), color + sub * subScale));
                }
            }
        }

        private Color ColorScale(Color src, Vector3 scale) => Color.FromArgb((byte)(src.R * scale.x), (byte)(src.G * scale.y), (byte)(src.R * scale.z), src.A);
    }
}
