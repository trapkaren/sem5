using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class Sphere : Primitive
    {
        public double radius;
        public Sphere(string name, Vec3d C, double r, Vec3d color, double specular, double reflective) : base (name, C, color, specular, reflective)
        {
            this.radius = r;   
        }
    }
}
