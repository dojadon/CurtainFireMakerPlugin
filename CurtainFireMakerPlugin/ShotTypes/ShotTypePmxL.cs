using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using VecMath;
using CurtainFireMakerPlugin.Entities;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public class ShotTypePmxL : ShotTypePmx
    {
        private Dictionary<int, Image> ImageDict { get; } = new Dictionary<int, Image>();

        public ShotTypePmxL(string name, string path, float size) : this(name, path, new Vector3(size, size, size)) { }

        public ShotTypePmxL(string name, string path, Vector3 size) : base(name, path, size)
        {
            InitModelData = data => { };
        }

        public override string[] CreateTextures(World wolrd, ShotProperty prop)
        {
            var texture = base.CreateTextures(wolrd, prop)[0];
            var colorTexture  = AppendFileName(texture, Convert.ToString(prop.Color, 16));

            if (!ImageDict.ContainsKey(prop.Color))
            {
                var image = new Bitmap(Plugin.Instance.Config.ResourceDirPath + "\\" + texture.Replace('/', '\\'));
                SetPxcelColor(image, prop.Red, prop.Green, prop.Blue);
                ImageDict.Add(prop.Color, image);

                wolrd.Export += (w, e) =>
                 {
                     var config = Plugin.Instance.Config;

                     string exportPath = config.ExportDirPath + "\\" + colorTexture.Replace('/', '\\');
                     File.Delete(exportPath);

                     image.Save(exportPath);
                     image.Dispose();
                 };
            }

            return new string[] { colorTexture };
        }

        private string AppendFileName(string path, string str)
        {
            string ext = Path.GetExtension(path);
            return path.Substring(0, path.Length - ext.Length) + str + ext;
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
