using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class Camera
    {
        public Vec3d position;
        public Vec3d angle;
        public double[,] rotation;

        public Camera()
        {
            this.position = new Vec3d(0, 0, 0);
            this.angle = new Vec3d(0, 0, 0);
            this.rotation = new double[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        }

    }
}
