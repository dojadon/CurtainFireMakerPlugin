using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Effects
{
    internal class DrawObjectPass : IFxElement
    {
        private const string BoneNameDummy = "{BONE_NAME}";
        private const string SamplerNameDummy = "{SAMPLER_NAME}";
        private const string ColorDummy = "{COLOR}";
        private const string MorphNameDummy = "{MORPH_NAME}";

        private const string Script =
        "	pass DrawObject" + BoneNameDummy + "{\n" +
        "		ZENABLE = false;\n" +
        "		VertexShader = compile vs_1_1 Billboard_VS(" + BoneNameDummy + " * 0.1);\n" +
        "		PixelShader  = compile ps_2_0 Billboard_PS(" + SamplerNameDummy + ", " + ColorDummy + ", " + MorphNameDummy + ");\n" +
        "	}\n";

        public string BoneName { get; set; }
        public string MorphName { get; set; }
        public string SamplerName { get; set; }

        public int Color { get; }
        private float Red => (Color >> 16 & 0x000000FF) / 255.0F;
        private float Green => (Color >> 8 & 0x000000FF) / 255.0F;
        private float Blue => (Color >> 0 & 0x000000FF) / 255.0F;

        public DrawObjectPass(string boneName, string morphName, string samplerName, int color)
        {
            BoneName = boneName;
            MorphName = morphName;
            SamplerName = samplerName;
            Color = color;
        }

        public string Build()
        {
            string ColorStr = "float4(" + Red + ", " + Green + ", " + Blue + ", 1.0)";

            return Script.Replace(BoneNameDummy, BoneName).Replace(MorphNameDummy, MorphName)
            .Replace(SamplerNameDummy, SamplerName).Replace(ColorDummy, ColorStr);
        }
    }
}
