using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Effects
{
    internal class Sampler : IFxElement
    {
        private const string SamplerNameDummy = "{SAMPLER_NAME}";
        private const string TextureNameDummy = "{TEXTURE_NAME}";

        private const string Script =
        "sampler " + SamplerNameDummy + " = sampler_state {\n" +
        "	texture = <" + TextureNameDummy + ">;\n" +
        "	MINFILTER = LINEAR;\n" +
        "	MAGFILTER = LINEAR;\n" +
        "	MIPFILTER = LINEAR;\n" +
        "	AddressU  = CLAMP;\n" +
        "	AddressV  = CLAMP;\n" +
        "};";

        public string SamplerName { get; }
        public string TextureName { get; }

        public Sampler(string samplerName, string textureName)
        {
            SamplerName = samplerName;
            TextureName = textureName;
        }

        public string Build()
        {
            return Script.Replace(SamplerNameDummy, SamplerName).Replace(TextureNameDummy, TextureName);
        }
    }
}
