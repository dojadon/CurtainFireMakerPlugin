using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public interface IRecordType
    {
        bool ShouldRecord(EntityShot entity);
        Vector3 GetRecordedPos(EntityShot entity);
        Quaternion GetRecordedRot(EntityShot entity);
    }

    internal class RecordTypeNone : IRecordType
    {
        public Vector3 GetRecordedPos(EntityShot entity) => entity.Pos;
        public Quaternion GetRecordedRot(EntityShot entity) => entity.Rot;
        public bool ShouldRecord(EntityShot entity) => false;
    }

    internal class RecordTypeVelocity : IRecordType
    {
        public Vector3 GetRecordedPos(EntityShot entity) => entity.Pos;
        public Quaternion GetRecordedRot(EntityShot entity) => Matrix3.LookAt(+entity.LookAtVec, entity.Upward);
        public bool ShouldRecord(EntityShot entity) => entity.IsUpdatedVelocity;
    }

    internal class RecordTypeLocalMat : IRecordType
    {
        public Vector3 GetRecordedPos(EntityShot entity) => entity.Pos;
        public Quaternion GetRecordedRot(EntityShot entity) => entity.Rot;
        public bool ShouldRecord(EntityShot entity) => entity.IsUpdatedLocalMat;
    }

    public static class RecordTypes
    {
        public static readonly IRecordType Velocity = new RecordTypeVelocity();
        public static readonly IRecordType LocalMat = new RecordTypeLocalMat();
        public static readonly IRecordType None = new RecordTypeNone();
    }
}
