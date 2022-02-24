using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class Cylinder : Primitive
    {
        public Vec3d V;
        public double radius;
        public double maxm;
        public Cylinder(string name, Vec3d C, Vec3d V, double radius, double maxm,
            Vec3d color, double specular, double reflective) : base (name, C, color, specular, reflective)
        {
            this.V = V;
            this.radius = radius;
            this.maxm = maxm;
        }
    }
}
