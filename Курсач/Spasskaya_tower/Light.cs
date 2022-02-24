using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{

    enum LightType { Ambient, Point, Directional};
    class Light
    {
        public string name;
        public LightType ltype;
        public Vec3d position;
        public double intensity;
        

        public Light(string name, LightType ltype, Vec3d position, double intensity)
        {
            this.name = name;
            this.ltype = ltype;
            this.position = position;
            this.intensity = intensity;
        }
    }
}
