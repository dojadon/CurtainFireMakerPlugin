using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Effects
{
    internal class BoneControlObject : IFxElement
    {
        private const string BoneNameDummy = "{BONE_NAME}";
        private const string Script
        = "float4 " + BoneNameDummy + " : CONTROLOBJECT < string name = \"" + FxEffect.ModelNameDummy + "\"; string item = \"" + BoneNameDummy + "\"; >;\n";

        public string BoneName { get; }

        public BoneControlObject(string boneName)
        {
            BoneName = boneName;
        }

        public string Build()
        {
            return Script.Replace(BoneNameDummy, BoneName);
        }
    }

    internal class MorphControlObject : IFxElement
    {
        private const string MorphNameDummy = "{BONE_NAME}";
        private const string Script
        = "float " + MorphNameDummy + " : CONTROLOBJECT < string name = \"" + FxEffect.ModelNameDummy + "\"; string item = \"" + MorphNameDummy + "\"; >;\n";

        public string MorphName { get; }

        public MorphControlObject(string morphName)
        {
            MorphName = morphName;
        }

        public string Build()
        {
            return Script.Replace(MorphNameDummy, MorphName);
        }
    }
}
