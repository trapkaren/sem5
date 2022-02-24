using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class Parallelepiped : Primitive
    {
        public Vec3d E;
        public Parallelepiped(string name, Vec3d C, Vec3d E,
            Vec3d color, double specular, double reflective) : base(name, C, color, specular, reflective)
        {
            this.E = E;
        }
    }
}