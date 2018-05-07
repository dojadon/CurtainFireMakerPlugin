using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using VecMath;
using VecMath.Geometry;

namespace CurtainFireMakerPlugin.Entities
{
    public class Entity
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

        public float Range { get; set; } = 0;
        public Sphere Sphere => new Sphere(Pos, Range);

        public virtual Func<Entity, bool> ShouldRemove { get; set; } = e => e.LivingLimit != 0 && e.FrameCount >= e.LivingLimit;

        public delegate void RemoveEventHandler(object sender, RemoveEventArgs args);
        public event RemoveEventHandler RemoveEvent;

        public bool IsSpawned { get; private set; }
        public bool IsRemoved { get; private set; }

        public virtual bool IsNeededUpdate => true;

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

        public virtual bool Spawn()
        {
            if (!IsSpawned)
            {
                SpawnFrameNo = World.AddEntity(this);
                return IsSpawned = true;
            }
            return false;
        }

        public virtual bool Remove(bool isFinalize = false)
        {
            if (!IsRemoved)
            {
                DeathFrameNo = World.RemoveEntity(this);
                RemoveEvent?.Invoke(this, new RemoveEventArgs(isFinalize));

                return IsRemoved = true;
            }
            return false;
        }

        public override bool Equals(object obj) => obj is Entity e && EntityId == e.EntityId;

        public override int GetHashCode() => EntityId;
    }

    public class RemoveEventArgs : EventArgs
    {
        public bool IsFinalize { get; set; }

        public RemoveEventArgs(bool isFinalize)
        {
            IsFinalize = isFinalize;
        }
    }
}
