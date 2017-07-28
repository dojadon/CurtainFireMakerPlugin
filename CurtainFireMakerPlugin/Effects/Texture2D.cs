using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Effects
{
    internal class Texture2D : IFxElement
    {
        private const string TextureNameDummy = "{TEXTURE_NAME}";
        private const string TexturePathDummy = "{TEXTURE_PATH}";

        private const string Script =
        "texture2D " + TextureNameDummy + " <\n" +
        "	string ResourceName = \"" + TexturePathDummy + "\";\n" +
        "	int MipLevels = 0;\n" +
        ">;\n";

        public string TextureName { get; }
        public string TexturePath { get; }

        public Texture2D(string textureName, string texturePath)
        {
            TextureName = textureName;
            TexturePath = texturePath;
        }

        public string Build()
        {
            return Script.Replace(TextureNameDummy, TextureName).Replace(TexturePathDummy, TexturePath);
        }
    }
}
