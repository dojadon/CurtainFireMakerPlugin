using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Mathematics
{
    public struct Quaternion
    {
        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);
        private const float EPS = 1.0e-12F;
        private const float EPS2 = 1.0e-30F;
        private const float PIO2 = 1.57079632679F;

        public float x;
        public float y;
        public float z;
        public float w;

        public float Length => (float)Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w);

        public Vector3 Vec
        {
            get { return new Vector3(x, y, z); }
            set
            {
                this.x = value.x;
                this.y = value.y;
                this.z = value.z;
            }
        }

        public Quaternion(float x, float y, float z, float w)
        {
            float mag;
            mag = 1.0F / (float)Math.Sqrt(x * x + y * y + z * z + w * w);
            this.x = x * mag;
            this.y = y * mag;
            this.z = z * mag;
            this.w = w * mag;
        }

        public Quaternion(Quaternion q1) : this(q1.x, q1.y, q1.z, q1.w)
        {

        }

        public Quaternion(Vector3 v1, float w) : this(v1.x, v1.y, v1.z, w)
        {

        }

        public static Quaternion Conjugate(Quaternion q1)
        {
            return new Quaternion()
            {
                w = q1.w,
                x = -q1.x,
                y = -q1.y,
                z = -q1.z
            };
        }

        public static Quaternion Inverse(Quaternion q1)
        {
            float norm = 1.0F / (q1.w * q1.w + q1.x * q1.x + q1.y * q1.y + q1.z * q1.z);
            return new Quaternion()
            {
                w = norm * q1.w,
                x = -norm * q1.x,
                y = -norm * q1.y,
                z = -norm * q1.z
            };
        }

        public static Quaternion Mul(Quaternion q1, Quaternion q2)
        {
            return new Quaternion()
            {
                w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z,
                x = q1.w * q2.x + q2.w * q1.x + q1.y * q2.z - q1.z * q2.y,
                y = q1.w * q2.y + q2.w * q1.y - q1.x * q2.z + q1.z * q2.x,
                z = q1.w * q2.z + q2.w * q1.z + q1.x * q2.y - q1.y * q2.x
            };
        }

        public static Quaternion Normalize(Quaternion q1)
        {
            float norm = q1.x * q1.x + q1.y * q1.y + q1.z * q1.z + q1.w * q1.w;
            norm = 1.0F / (float)Math.Sqrt(norm);

            return new Quaternion()
            {
                x = norm * q1.x,
                y = norm * q1.y,
                z = norm * q1.z,
                w = norm * q1.w
            };
        }

        public static Quaternion RotationAxisAngle(Vector3 a, float angle)
        {
            float amag = (float)Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);

            if (amag < EPS)
            {
                return Identity;
            }
            else
            {
                amag = 1.0F / amag;
                float mag = (float)Math.Sin(angle / 2.0);

                return new Quaternion()
                {
                    w = (float)Math.Cos(angle / 2.0),
                    x = a.x * amag * mag,
                    y = a.y * amag * mag,
                    z = a.z * amag * mag
                };
            }
        }

        public static Quaternion RotationMatrix(Matrix m1)
        {
            float m00 = m1.m00;
            float m01 = m1.m01;
            float m02 = m1.m02;

            float m10 = m1.m10;
            float m11 = m1.m11;
            float m12 = m1.m12;

            float m20 = m1.m20;
            float m21 = m1.m21;
            float m22 = m1.m22;

            float[] values = new float[4];
            values[0] = m00 - m11 - m22;
            values[1] = -m00 + m11 - m22;
            values[2] = -m00 - m11 + m22;
            values[3] = m00 + m11 + m22;

            int biggestIndex = 0;
            for (int i = 1; i < 4; i++)
            {
                if (values[i] > values[biggestIndex]) biggestIndex = i;
            }

            float val = (float)Math.Sqrt(values[biggestIndex] + 1.0F) * 0.5F;
            float mult = 0.25F / val;

            switch (biggestIndex)
            {
                case 0:
                    return new Quaternion()
                    {
                        x = val,
                        y = (m01 + m10) * mult,
                        z = (m20 + m02) * mult,
                        w = (m12 - m21) * mult
                    };

                case 1:
                    return new Quaternion()
                    {
                        x = (m01 + m10) * mult,
                        y = val,
                        z = (m12 + m21) * mult,
                        w = (m20 - m02) * mult
                    };

                case 2:
                    return new Quaternion()
                    {
                        x = (m20 + m02) * mult,
                        y = (m12 + m21) * mult,
                        z = val,
                        w = (m01 - m10) * mult
                    };

                case 3:
                    return new Quaternion()
                    {
                        x = (m12 - m21) * mult,
                        y = (m20 - m02) * mult,
                        z = (m01 - m10) * mult,
                        w = val
                    };
            }
            return Identity;
        }

        public static Quaternion Pow(Quaternion q1, float exponent)
        {
            if (Math.Abs(q1.w) < 0.999999) { return Identity; }

            float angle1 = (float)Math.Acos(q1.w);
            float angle2 = angle1 * exponent;

            float mult = (float)(Math.Sin(angle2) / Math.Sin(angle1));

            return new Quaternion()
            {
                w = (float)Math.Cos(angle2),
                x = q1.x * mult,
                y = q1.y * mult,
                z = q1.z * mult
            };
        }

        public static Quaternion Interpolate(Quaternion q1, Quaternion q2, float alpha)
        {
            float dot, s1, s2, om, sinom;

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
                om = (float)Math.Acos(dot);
                sinom = (float)Math.Sin(om);
                s1 = (float)Math.Sin((1.0 - alpha) * om) / sinom;
                s2 = (float)Math.Sin(alpha * om) / sinom;
            }
            else
            {
                s1 = 1.0F - alpha;
                s2 = alpha;
            }
            return new Quaternion()
            {
                w = s1 * q1.w + s2 * q2.w,
                x = s1 * q1.x + s2 * q2.x,
                y = s1 * q1.y + s2 * q2.y,
                z = s1 * q1.z + s2 * q2.z
            };
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

        public bool Equals(Quaternion q1) => this == q1;

        public static bool EpsilonEquals(Quaternion q1, Quaternion q2, float epsilon)
        {
            float diff;
            diff = q1.x - q2.x;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = q1.y - q2.y;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = q1.z - q2.z;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = q1.w - q2.w;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            return true;
        }

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

        public override string ToString()
        {
            return "[" + x + ", " + y + ", " + z + ", " + w + "]";
        }

        public static bool operator ==(Quaternion q1, Quaternion q2) => q1.x == q2.x && q1.y == q2.y && q1.z == q2.z && q1.w == q2.w;

        public static bool operator !=(Quaternion q1, Quaternion q2) => (q1.x == q2.x && q1.y == q2.y && q1.z == q2.z && q1.w == q2.w);

        public static Quaternion operator +(Quaternion q1) => Normalize(q1);

        public static Quaternion operator -(Quaternion q1) => Conjugate(q1);

        public static Quaternion operator ~(Quaternion q1) => Inverse(q1);

        public static Quaternion operator *(Quaternion q1, Quaternion q2) => Mul(q1, q2);

        public static Quaternion operator ^(Quaternion q1, double d1) => Pow(q1, (float)d1);

        public static implicit operator Quaternion(Matrix m1) => RotationMatrix(m1);

        public static implicit operator Quaternion(DxMath.Quaternion q1) => new Quaternion(q1.X, q1.Y, q1.Z, q1.W);

        public static explicit operator DxMath.Quaternion(Quaternion q1) => new DxMath.Quaternion(q1.x, q1.y, q1.z, q1.w);
    }
}
