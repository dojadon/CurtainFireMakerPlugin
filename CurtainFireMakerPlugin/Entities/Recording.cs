using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public interface IRecording
    {
        bool ShouldRecord(EntityShot entity);
        Vector3 GetRecordedPos(EntityShot entity);
        Quaternion GetRecordedRot(EntityShot entity);
    }

    internal class RecordingNone : IRecording
    {
        public Vector3 GetRecordedPos(EntityShot entity) => entity.Pos;
        public Quaternion GetRecordedRot(EntityShot entity) => entity.Rot;
        public bool ShouldRecord(EntityShot entity) => false;
    }

    internal class RecordingVelocity : IRecording
    {
        public Vector3 GetRecordedPos(EntityShot entity) => entity.Pos;
        public Quaternion GetRecordedRot(EntityShot entity) => Matrix3.LookAt(+entity.LookAtVec, entity.Upward);
        public bool ShouldRecord(EntityShot entity) => entity.IsUpdatedVelocity;
    }

    internal class RecordingLocalMat : IRecording
    {
        public Vector3 GetRecordedPos(EntityShot entity) => entity.Pos;
        public Quaternion GetRecordedRot(EntityShot entity) => entity.Rot;
        public bool ShouldRecord(EntityShot entity) => entity.IsUpdatedLocalMat;
    }

    public static class Recording
    {
        public static readonly IRecording Velocity = new RecordingVelocity();
        public static readonly IRecording LocalMat = new RecordingLocalMat();
        public static readonly IRecording None = new RecordingNone();
    }
}
