using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public abstract class Recording
    {
        public static readonly Recording Velocity = new RecordingVelocity();
        public static readonly Recording LocalMat = new RecordingLocalMat();
        public static readonly Recording None = new RecordingNone();

        public abstract bool ShouldRecord(EntityShot entity);
        public abstract Vector3 GetRecordedPos(EntityShot entity);
        public abstract Quaternion GetRecordedRot(EntityShot entity);
    }

    public class RecordingNone : Recording
    {
        public override Vector3 GetRecordedPos(EntityShot entity) => entity.Pos;
        public override Quaternion GetRecordedRot(EntityShot entity) => entity.Rot;
        public override  bool ShouldRecord(EntityShot entity) => false;
    }

    public class RecordingVelocity : Recording
    {
        public override Vector3 GetRecordedPos(EntityShot entity) => entity.Pos;
        public override Quaternion GetRecordedRot(EntityShot entity) => Matrix3.LookAt(+entity.LookAtVec, entity.Upward);
        public override bool ShouldRecord(EntityShot entity) => entity.IsUpdatedVelocity;
    }

    public class RecordingLocalMat : Recording
    {
        public override Vector3 GetRecordedPos(EntityShot entity) => entity.Pos;
        public override Quaternion GetRecordedRot(EntityShot entity) => entity.Rot;
        public override bool ShouldRecord(EntityShot entity) => entity.IsUpdatedLocalMat;
    }
}
