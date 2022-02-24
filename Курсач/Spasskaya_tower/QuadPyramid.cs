using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class QuadPyramid : Primitive
    {
        public Vec3d A;
        public Vec3d B;
        public Vec3d D;
        public Vec3d P;
        public QuadPyramid(string name, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d D,
            Vec3d color, double specular, double reflective) : base(name, C, color, specular, reflective)
        {
            this.A = A;
            this.B = B;
            this.D = D;
            this.P = P;
        }
    }
}