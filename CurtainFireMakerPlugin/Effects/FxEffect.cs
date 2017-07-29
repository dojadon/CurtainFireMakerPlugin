using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CurtainFireMakerPlugin.Collections;

namespace CurtainFireMakerPlugin.Effects
{
    internal class FxEffect
    {
        public const string ModelNameDummy = "{MODEL_NAME}";

        public List<BoneControlObject> BoneControlObjectList { get; } = new List<BoneControlObject>();
        public List<MorphControlObject> MorphControlObjectList { get; } = new List<MorphControlObject>();
        public List<Texture2D> TextureList { get; } = new List<Texture2D>();
        public List<Sampler> SamplerList { get; } = new List<Sampler>();
        public List<DrawObjectPass> DrawObjectPassList { get; } = new List<DrawObjectPass>();

        private string Script { get; }

        public HashSet<int> MaterialIndices { get; } = new HashSet<int>();

        public FxEffect(string filePath)
        {
            Script = File.ReadAllText(filePath, Encoding.UTF8);
        }

        public string Build(string modelName)
        {
            string result = Script;

            result = result.Replace("{BONE_CONTROL_LIST}", BuildElementList(BoneControlObjectList));
            result = result.Replace("{MORPH_CONTROL_LIST}", BuildElementList(MorphControlObjectList));
            result = result.Replace("{TEXTURE_LIST}", BuildElementList(TextureList));
            result = result.Replace("{SAMPLER_LIST}", BuildElementList(SamplerList));
            result = result.Replace("{DRAW_OBJECT_PASS_LIST}", BuildElementList(DrawObjectPassList));

            string materialIndices = string.Join(", ", MaterialIndices);
            result = result.Replace("{MATERIAL_INDICES}", materialIndices);

            result = result.Replace(ModelNameDummy, modelName);

            return result;
        }

        private string BuildElementList<T>(List<T> elementList) where T : IFxElement
        {
            var sb = new StringBuilder();
            foreach (var element in elementList)
            {
                sb.Append(element.Build());
            }
            return sb.ToString();
        }
    }
}
