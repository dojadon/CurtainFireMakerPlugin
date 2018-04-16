using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using VecMath;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin.Entities
{
    public class Entity : DynamicObject
    {
        public virtual Matrix4 WorldMat => ParentEntity != null ? LocalMat * ParentEntity.WorldMat : LocalMat;
        public Vector3 WorldPos => WorldMat.Translation;
        public Matrix3 WorldRot => WorldMat;

        public virtual Matrix4 LocalMat
        {
            get => new Matrix4(Rot, Pos);
            protected set
            {
                Rot = value.Rotation;
                Pos = value.Translation;
            }
        }

        public virtual Vector3 Pos { get; set; }
        public virtual Quaternion Rot { get; set; } = Quaternion.Identity;

        public virtual Entity ParentEntity { get; protected set; }

        public int FrameCount => World.FrameCount - SpawnFrameNo;
        public int LivingLimit { get; set; }
        public int SpawnFrameNo { get; private set; }
        public int DeathFrameNo { get; private set; }

        public virtual Func<Entity, bool> ShouldRemove { get; set; } = e => e.LivingLimit != 0 && e.FrameCount >= e.LivingLimit;

        public event EventHandler DeathEvent;

        public bool IsRemoved { get; private set; }

        public virtual bool IsNeededUpdate => true;

        private Dictionary<string, object> AttributeDict { get; } = new Dictionary<string, object>();

        public World World { get; }

        public int EntityId { get; }
        private static int nextEntityId;

        public Entity(World world, Entity parentEntity = null)
        {
            World = world;
            EntityId = nextEntityId++;

            ParentEntity = parentEntity;
        }

        public virtual void Frame()
        {
            if (ShouldRemove(this))
            {
                Remove();
            }
        }

        public void __call__()
        {
            Spawn();
        }

        public virtual void Spawn()
        {
            SpawnFrameNo = World.AddEntity(this);
        }

        public virtual void Remove()
        {
            DeathFrameNo = World.RemoveEntity(this);
            IsRemoved = true;

            DeathEvent?.Invoke(this, EventArgs.Empty);
        }

        public override bool Equals(object obj) => obj is Entity e && EntityId == e.EntityId;

        public override int GetHashCode() => EntityId;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!AttributeDict.ContainsKey(binder.Name))
            {
                throw new KeyNotFoundException("Not found key : " + binder.Name);
            }
            result = AttributeDict[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            AttributeDict[binder.Name] = value;
            return true;
        }
    }
}
