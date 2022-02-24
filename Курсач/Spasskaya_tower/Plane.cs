using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class Plane : Primitive
    {
        
        public Vec3d V;
        public Plane(string name, Vec3d C, Vec3d V, Vec3d color, double specular, double reflective) : base (name, C, color, specular, reflective)
        {
            this.V = V;
        }
    }

    class DiskPlane : Primitive
    {

        public Vec3d V;
        public double r;
        public DiskPlane(string name, Vec3d C, Vec3d V, double r, Vec3d color, double specular, double reflective) : base(name, C, color, specular, reflective)
        {
            this.V = V;
            this.r = r;
        }
    }
}
