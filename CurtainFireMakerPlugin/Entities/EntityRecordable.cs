using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public abstract class EntityRecordable : EntityShootable
    {
        public Vector3 PrevPos { get; private set; }
        public Quaternion PrevRot { get; private set; }
        public Vector3 PrevVelocity { get; private set; }
        public static float Epsilon = 1E-4F;

        public EntityRecordable(World world, Entity parentEntity = null) : base(world, parentEntity) { }

        protected override void UpdatePos()
        {
            if (ShouldRecord())
            {
                Record();
            }

            base.UpdatePos();

            PrevPos = Pos;
            PrevRot = Rot;
            PrevVelocity = Velocity;
        }

        protected virtual bool ShouldRecord() => !(Vector3.EpsilonEquals(Pos, PrevPos, Epsilon) &&
            Quaternion.EpsilonEquals(Rot, PrevRot, Epsilon) &&
            Vector3.EpsilonEquals(Velocity, PrevVelocity, Epsilon));

        protected abstract void Record();
    }
}
