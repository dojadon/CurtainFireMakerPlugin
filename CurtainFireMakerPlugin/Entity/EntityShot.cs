using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Entity
{
    class EntityShot : Entity
    {
        public ShotProperty Property { get; }
        public Entity ParentEntity { get => this.parentEntity; set { this.parentEntity = value; } }

        public EntityShot(ShotProperty property)
        {
            this.Property = property;
        }


    }
}
