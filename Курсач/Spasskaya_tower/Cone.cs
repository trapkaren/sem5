using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
  /*  class Cone : Primitive
    {
        public Vec3d V;
        public double k;
        public double maxm;
        public double minm;
        public Cone(Vec3d C, Vec3d V, double k, double maxm, double minm,
            Vec3d color, double specular, double reflective) : base(C, color, specular, reflective)
        {
            this.V = V;
            this.k = k;
            this.maxm = maxm;
            this.minm = minm;
        }
    }*/
   class Cone : Primitive
    {
        public Vec3d V;
        public double angle;
        public double k;
        public double minm;
        public double maxm;

        public Cone(string name, Vec3d C, Vec3d V, double angle, double k, double minm, double maxm,
            Vec3d color, double specular, double reflective) : base(name, C, color, specular, reflective)
        {
            this.V = V;
            this.angle = angle;
            this.k = k;
            this.minm = minm;
            this.maxm = maxm;
        }
    }
}

