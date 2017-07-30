using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CurtainFireMakerPlugin.Effects;


namespace CurtainFireMakerPlugin.Entities
{
    internal class CurtainFireEffect
    {
        public FxEffect Effect { get; }
        public string XFilePath => Plugin.Instance.PluginRootPath + "\\Resource\\Template.x";

        public CurtainFireEffect()
        {
            Effect = new FxEffect(Plugin.Instance.PluginRootPath + "\\Resource\\Template.fx");
        }

        public void InitEntityShot(EntityShot entity, string texturePath)
        {
            string textureName = Path.GetFileNameWithoutExtension(texturePath) + "Tex";
            string samplerName = textureName + "Samp";

            if (!Effect.TextureList.Exists(t => t.TexturePath == texturePath))
            {
                Effect.TextureList.Add(new Texture2D(textureName, texturePath));
                Effect.SamplerList.Add(new Sampler(samplerName, textureName));
            }

            if (!Effect.DrawObjectPassList.Exists(p => p.BoneName == entity.RootBone.BoneName))
            {
                Effect.MorphControlObjectList.Add(new MorphControlObject(entity.MaterialMorph.MorphName));
                Effect.BoneControlObjectList.Add(new BoneControlObject(entity.RootBone.BoneName));
                Effect.DrawObjectPassList.Add(new DrawObjectPass(entity.RootBone.BoneName, entity.MaterialMorph.MorphName, samplerName, entity.Property.Color));
            }
        }

        public bool ShouldBuild()
        {
            return Effect.DrawObjectPassList.Count > 0;
        }

        public string Build(string modelName)
        {
            return Effect.Build(modelName);
        }
    }
}
