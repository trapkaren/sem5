using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class Triangle : Primitive
    {
        public Vec3d A;
        public Vec3d B;
        public Triangle(string name, Vec3d C, Vec3d A, Vec3d B,
            Vec3d color, double specular, double reflective) : base(name, C, color, specular, reflective)
        {
            this.A = A;
            this.B = B;
        }
    }
}