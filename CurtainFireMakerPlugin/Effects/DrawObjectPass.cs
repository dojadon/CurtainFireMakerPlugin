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

        private const string Script =
        "	pass DrawObject" + BoneNameDummy + "{\n" +
        "		ZENABLE = false;\n" +
        "		VertexShader = compile vs_1_1 Billboard_VS(" + BoneNameDummy + " * 0.1);\n" +
        "		PixelShader  = compile ps_2_0 Billboard_PS(" + SamplerNameDummy + ", " + ColorDummy + ");\n" +
        "	}\n";

        public string BoneName { get; }
        public string SamplerName { get; }

        public int Color { get; }
        private float Alpha => (Color >> 24 & 0x000000FF) / 255.0F;
        private float Red => (Color >> 16 & 0x000000FF) / 255.0F;
        private float Green => (Color >> 8 & 0x000000FF) / 255.0F;
        private float Blue => (Color >> 0 & 0x000000FF) / 255.0F;

        public DrawObjectPass(string boneName, string samplerName, int color)
        {
            BoneName = boneName;
            SamplerName = samplerName;
        }

        public string Build()
        {
            string ColorStr = "float4(" + Red + ", " + Green + ", " + Blue + ", 1.0)";

            return Script.Replace(BoneNameDummy, BoneName).Replace(SamplerNameDummy, SamplerName)
            .Replace(ColorDummy, ColorStr);
        }
    }
}
