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
            set
            {
                Rot = value.Rotation;
                Pos = value.Translation;
            }
        }

        public virtual Vector3 Pos { get; set; }
        public virtual Quaternion Rot { get; set; } = Quaternion.Identity;

        public virtual Entity ParentEntity { get; protected set; }

        public int FrameCount => World.FrameCount - SpawnFrameNo;
        public int LifeSpan { get; set; }
        public int SpawnFrameNo { get; private set; }
        public int DeathFrameNo { get; private set; }

        public float Range { get; set; } = 0;
        public Sphere Sphere => new Sphere(Pos, Range);
        public Sphere GetExpandSphere(float r) => new Sphere(Pos, Range + r);

        public virtual Func<Entity, bool> ShouldRemove { get; set; } = e => e.LifeSpan != 0 && e.FrameCount >= e.LifeSpan;

        public delegate void RemovedEventHandler(object sender, RemoveEventArgs args);
        public event RemovedEventHandler RemovedEvent;

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
                RemovedEvent?.Invoke(this, new RemoveEventArgs(isFinalize));

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
