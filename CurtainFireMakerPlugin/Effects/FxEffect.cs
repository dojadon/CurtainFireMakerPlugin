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

        public List<ControlObject> ControlObjectList { get; } = new List<ControlObject>();
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

            result = result.Replace("{BONE_CONTROL_LIST}", BuildElementList(ControlObjectList));
            result = result.Replace("{TEXTURE_LIST}", BuildElementList(TextureList));
            result = result.Replace("{SAMPLER_LIST}", BuildElementList(SamplerList));
            result = result.Replace("{DRAW_OBJECT_PASS_LIST}", BuildElementList(DrawObjectPassList));

            string materialIndices = string.Join(", ", MaterialIndices);
            result = result.Replace("{MATERIAL_INDICES}", materialIndices);

            result.Replace(ModelNameDummy, modelName);
            result.Replace("\\", "\\\\");

            return result;
        }

        private string BuildElementList<T>(List<T> elementList) where T : IFxElement
        {
            string result = "";
            foreach (var element in elementList)
            {
                result += element.Build();
            }
            return result;
        }
    }
}
