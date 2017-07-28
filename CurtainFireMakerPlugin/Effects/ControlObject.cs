using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Effects
{
    internal class ControlObject : IFxElement
    {
        private const string BoneNameDummy = "{BONE_NAME}";
        private const string Script
        = "float4 " + BoneNameDummy + " : CONTROLOBJECT < string name = \"" + FxEffect.ModelNameDummy + "\"; string item = \"" + BoneNameDummy + "\"; >;\n";

        public string BoneName { get; }

        public ControlObject(string boneName)
        {
            BoneName = boneName;
        }

        public string Build()
        {
            return Script.Replace(BoneNameDummy, BoneName);
        }
    }
}
