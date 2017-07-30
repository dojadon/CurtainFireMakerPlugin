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

        public ShotTypePmxL(string name, string path, float size) : this(name, path, new Vector3(size, size, size)) { }

        public ShotTypePmxL(string name, string path, Vector3 size) : base(name, path, size)
        {
            InitModelData = data =>
           {
               var prop = data.Property;

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

               if (!data.World.PmxModel.TextureList.Contains(texPath))
               {
                   data.World.PmxModel.TextureList.Add(texPath);
               }
               data.Textures[0] = texPath;
               data.Materials[0].SphereId = data.World.PmxModel.TextureList.IndexOf(texPath);
               data.Materials[0].Mode = 1;
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

            float boder1 = 0.7F;
            float boder2 = 0.9F;
            float boderMult = 1 / (boder2 - boder1);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color src = image.GetPixel(x, y);
                    float subScale = 1;

                    var pos = new Vector2((float)x / image.Width - 0.5F, (float)y / image.Width - 0.5F) * 2;

                    float dis = pos.Length();

                    if (dis < boder1)
                    {
                        subScale = 0;
                    }
                    else if (boder1 <= dis && dis < boder2)
                    {
                        subScale = (dis - boder1) * boderMult;
                    }

                    image.SetPixel(x, y, ColorScale(image.GetPixel(x, y), color + sub * subScale));
                }
            }
        }

        private Color ColorScale(Color src, Vector3 scale) => Color.FromArgb(src.A, (byte)(src.R * scale.x), (byte)(src.G * scale.y), (byte)(src.R * scale.z));
    }
}
