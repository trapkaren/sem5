using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class Vec3d
    {
        public double x, y, z;
        
        public Vec3d()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        public Vec3d(Vec3d vec)
        {
            this.x = vec.x;
            this.y = vec.y;
            this.z = vec.z;
        }
        public Vec3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vec3d operator +(Vec3d v1, Vec3d v2)
        {
            return new Vec3d { x = v1.x + v2.x, y = v1.y + v2.y, z = v1.z + v2.z };
        }

        public static Vec3d operator / (Vec3d v1, double k)
        {
            return new Vec3d { x = v1.x / k, y = v1.y / k, z = v1.z / k };
        }

        public static Vec3d operator -(Vec3d v1, Vec3d v2)
        {
            return new Vec3d { x = v1.x - v2.x, y = v1.y - v2.y, z = v1.z - v2.z };
        }
        public static Vec3d operator -(Vec3d v)
        {
            return new Vec3d { x = -v.x, y = -v.y, z = -v.z };
        }

        public static Vec3d operator *(Vec3d v, double k)
        {
            return new Vec3d { x = v.x * k, y = v.y * k, z = v.z * k };
        }

        public static Vec3d operator *(Vec3d v, double[,] mtr)
        {
            double[] tmp = new double[4] { v.x, v.y, v.z, 1 };
            double[] result = new double[4] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i] += tmp[j] * mtr[i, j];
                }
            }

            return new Vec3d { x = result[0], y = result[1], z = result[2] };
        }
        public static Vec3d operator *(double k, Vec3d v)
        {
            return new Vec3d { x = v.x * k, y = v.y * k, z = v.z * k };
        }

        public static double operator *(Vec3d a, Vec3d b)
        {
            return a.x* b.x + a.y * b.y + a.z * b.z;
        }

        public static double ScalarMultiplication(Vec3d a, Vec3d b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static double Length(Vec3d vec)
        {
            return Math.Sqrt(vec.x * vec.x + vec.y*vec.y + vec.z * vec.z);
        }

        public Vec3d Normalize()
        {
            double length_vec = Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            return new Vec3d(this.x / length_vec, this.y / length_vec, this.z / length_vec);
        }
    }
}
