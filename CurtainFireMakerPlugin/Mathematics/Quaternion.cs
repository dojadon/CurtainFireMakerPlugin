using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Mathematics
{
    public struct Quaternion
    {
        private const double EPS = 1.0e-12;
        private const double EPS2 = 1.0e-30;
        private const double PIO2 = 1.57079632679;

        public double x;
        public double y;
        public double z;
        public double w;

        public Quaternion(double x, double y, double z, double w)
        {
            double mag;
            mag = 1.0 / Math.Sqrt(x * x + y * y + z * z + w * w);
            this.x = x * mag;
            this.y = y * mag;
            this.z = z * mag;
            this.w = w * mag;
        }

        public Quaternion(Quaternion q1) : this(q1.x, q1.y, q1.z, q1.w)
        {

        }

        public static Quaternion Conjugate(Quaternion q1)
        {
            var q2 = new Quaternion(q1);
            q2.x = -q2.x;
            q2.y = -q2.y;
            q2.z = -q2.z;

            return q2;
        }

        public static Quaternion Mul(Quaternion q1, Quaternion q2)
        {
            var q3 = new Quaternion();

            q3.w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
            q3.x = q1.w * q2.x + q2.w * q1.x + q1.y * q2.z - q1.z * q2.y;
            q3.y = q1.w * q2.y + q2.w * q1.y - q1.x * q2.z + q1.z * q2.x;
            q3.z = q1.w * q2.z + q2.w * q1.z + q1.x * q2.y - q1.y * q2.x;

            return q3;
        }

        public static Quaternion Inverse(Quaternion q1)
        {
            var q2 = new Quaternion();
            double norm = 1.0 / (q1.w * q1.w + q1.x * q1.x + q1.y * q1.y + q1.z * q1.z);

            q2.w = norm * q1.w;
            q2.x = -norm * q1.x;
            q2.y = -norm * q1.y;
            q2.z = -norm * q1.z;

            return q2;
        }

        public static Quaternion Normalize(Quaternion q1)
        {
            var q2 = new Quaternion();
            double norm = q1.x * q1.x + q1.y * q1.y + q1.z * q1.z + q1.w * q1.w;
            norm = 1.0 / Math.Sqrt(norm);

            q2.x = norm * q1.x;
            q2.y = norm * q1.y;
            q2.z = norm * q1.z;
            q2.w = norm * q1.w;

            return q2;
        }

        public static Quaternion RotationAxisAngle(Vector3 a, double angle)
        {
            var q1 = new Quaternion();

            double amag = Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);

            if (amag < EPS)
            {
                q1.w = 0.0;
                q1.x = 0.0;
                q1.y = 0.0;
                q1.z = 0.0;
            }
            else
            {
                amag = 1.0 / amag;
                double mag = Math.Sin(angle / 2.0);
                q1.w = Math.Cos(angle / 2.0);
                q1.x = a.x * amag * mag;
                q1.y = a.y * amag * mag;
                q1.z = a.z * amag * mag;
            }
            return q1;
        }

        public static Quaternion RotationMatrix(Matrix m1)
        {
            var q1 = new Quaternion();

            q1.w = Math.Sqrt(m1.m00 + m1.m11 + m1.m22 + 1) * 0.5;
            double ww = 0.25 / q1.w;

            q1.x = (m1.m12 + m1.m21) * ww;
            q1.y = (m1.m20 + m1.m02) * ww;
            q1.z = (m1.m01 + m1.m10) * ww;

            return q1;
        }

        public static Quaternion Interpolate(Quaternion q1, Quaternion q2, double alpha)
        {
            var q3 = new Quaternion();

            double dot, s1, s2, om, sinom;

            dot = q2.x * q1.x + q2.y * q1.y + q2.z * q1.z + q2.w * q1.w;

            if (dot < 0)
            {
                q1.x = -q1.x;
                q1.y = -q1.y;
                q1.z = -q1.z;
                q1.w = -q1.w;
                dot = -dot;
            }

            if (1.0 - dot > EPS)
            {
                om = Math.Acos(dot);
                sinom = Math.Sin(om);
                s1 = Math.Sin((1.0 - alpha) * om) / sinom;
                s2 = Math.Sin(alpha * om) / sinom;
            }
            else
            {
                s1 = 1.0 - alpha;
                s2 = alpha;
            }
            q3.w = s1 * q1.w + s2 * q2.w;
            q3.x = s1 * q1.x + s2 * q2.x;
            q3.y = s1 * q1.y + s2 * q2.y;
            q3.z = s1 * q1.z + s2 * q2.z;

            return q3;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as Quaternion?;
            if (p == null)
            {
                return false;
            }

            return this.Equals((Quaternion)obj);
        }

        public bool Equals(Quaternion p1) => p1.x == this.x && p1.y == this.y && p1.z == this.z && p1.w == this.w;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                hashCode = (hashCode * 397) ^ w.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Quaternion q1, Quaternion q2) => q1.Equals(q2);

        public static bool operator !=(Quaternion q1, Quaternion q2) => !q1.Equals(q2);

        public static Quaternion operator +(Quaternion q1) => Normalize(q1);

        public static Quaternion operator -(Quaternion q1) => Conjugate(q1);

        public static Quaternion operator ~(Quaternion q1) => Inverse(q1);

        public static Quaternion operator *(Quaternion q1, Quaternion q2) => Mul(q1, q2);

        public static implicit operator Quaternion(Matrix m1) => RotationMatrix(m1);

        public static implicit operator Quaternion(DxMath.Quaternion q1) => new Quaternion(q1.X, q1.Y, q1.Z, q1.W);

        public static explicit operator DxMath.Quaternion(Quaternion q1) => new DxMath.Quaternion((float)q1.x, (float)q1.y, (float)q1.z, (float)q1.w);
    }
}
