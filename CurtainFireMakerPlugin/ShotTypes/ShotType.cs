using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;
using VecMath;
using CsPmx.Data;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public class ShotType
    {
        public String Name { get; }

        public ShotType(String name)
        {
            Name = name;
        }

        public virtual bool HasMmdData { get; } = false;

        public virtual Action<EntityShot> Init { get; set; } = e => { };
        public Action<ShotModelData> InitModelData { get; set; } = data =>
         {
             if (data.Materials != null)
             {
                 var prop = data.Property;
                 foreach (var material in data.Materials)
                 {
                     material.Diffuse = new Vector4(prop.Red, prop.Green, prop.Blue, 1);
                     material.Ambient = new Vector3(prop.Red, prop.Green, prop.Blue);
                 }
             }
         };

        public virtual PmxBoneData[] GetCreateBones(World wolrd, ShotProperty prop) => new PmxBoneData[] { new PmxBoneData() };

        public virtual PmxMaterialData[] CreateMaterials(World wolrd, ShotProperty prop) => null;

        public virtual string[] CreateTextures(World wolrd, ShotProperty prop) => null;

        public virtual int[] CreateVertexIndices(World wolrd, ShotProperty prop) => null;

        public virtual PmxVertexData[] CreateVertices(World wolrd, ShotProperty prop) => null;
    }
}
