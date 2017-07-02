using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entity;
using CPmx.Data;

namespace CurtainFireMakerPlugin.ShotType
{
    public abstract class ShotType
    {
        private String Name { get; }

        private Action<EntityShot> frameFunc = entity =>
        {

        };

        public ShotType(String name)
        {
            this.Name = name;
        }

        public bool HasMmdData()
        {
            return true;
        }

        public bool RecordMotion()
        {
            return true;
        }

        public void Frame(EntityShot entity)
        {
            this.frameFunc.Invoke(entity);
        }

        public abstract PmxVertexData[] GetVertices(EntityShot entity);

        public abstract int[] GetVertexIndices(EntityShot entity);

        public abstract PmxMaterialData[] GetMaterials(EntityShot entity);

        public abstract String[] GetTextures(EntityShot entity);

        public abstract PmxBoneData[] GetBones(EntityShot entity);
    }
}
