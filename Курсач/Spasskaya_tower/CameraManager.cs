using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class CameraManager
    {
        private double[,] multMatrix(double[,] A, double [,] B)
        {
            double[,] result = new double[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                        result[i, j] += A[i, k] * B[k, j];
                }
                
            }
            return result;
        }

        public void yawCamera(ref Camera camera, double angle)
        {
            double[,] RMtr = new double[4, 4] { { 1, 0, 0, 0 },
                                                { 0, Math.Cos(angle * Math.PI / 180), -Math.Sin(angle * Math.PI / 180), 0 },
                                                { 0, Math.Sin(angle * Math.PI / 180), Math.Cos(angle * Math.PI / 180), 0 },
                                                { 0, 0, 0, 1 } };
            double[,] tmp = multMatrix(camera.rotation, RMtr);
            camera.rotation = tmp;
            camera.angle.x += angle;
        }
        public void pitchCamera(ref Camera camera, double angle)
        {
            double[,] RMtr = new double[4, 4] { { Math.Cos(angle * Math.PI / 180), 0, -Math.Sin(angle * Math.PI / 180), 0 },
                                                { 0, 1, 0, 0 },
                                                { Math.Sin(angle * Math.PI / 180), 0, Math.Cos(angle * Math.PI / 180), 0 },
                                                { 0, 0, 0, 1 } };
            double[,] tmp = multMatrix(camera.rotation, RMtr);
            camera.rotation = tmp;
            camera.angle.y += angle;
        }
        public void rollCamera(ref Camera camera, double angle)
        {
            double[,] RMtr = new double[4, 4] { { Math.Cos(angle * Math.PI / 180), -Math.Sin(angle * Math.PI / 180), 0, 0 },
                                                { Math.Sin(angle * Math.PI / 180), Math.Cos(angle * Math.PI / 180), 0, 0 },
                                                { 0, 0, 1, 0 },
                                                { 0, 0, 0, 1 } };
            
            double[,] tmp = multMatrix(camera.rotation, RMtr);
            camera.rotation = tmp;
            camera.angle.z += angle;
        }

        public void moveCamera(ref Camera camera, double dx, double dy, double dz)
        {
            camera.position.x += dx;
            camera.position.y += dy;
            camera.position.z += dz;
        }
    }
}
