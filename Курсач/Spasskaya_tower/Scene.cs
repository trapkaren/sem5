using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Spasskaya_tower
{
    class Scene
    {
        public int canvasWidth { get; private set; }
        public int canvasHeight { get; private set; }
        public Vec3d[,] background;

        public Camera camera { get; set; }
        public List<Primitive> sceneObjects { get; set; }
        public List<Light> lights { get; set; }
        public int countPointLights = 0;


        public void convertBackgroundReverse(int canvasWidth, int canvasHeight)
        {
            for (int i = 0; i < canvasWidth; i++)
            {
                for (int j = 0; j < canvasHeight; j++)
                {
                    this.background[i, canvasHeight - j - 1] = new Vec3d(117, 187, 253);
                }
            }
        }

        public void convertBackground(int canvasWidth, int canvasHeight)
        {
            for (int i = 0; i < canvasWidth; i++)
            {
                for (int j = 0; j < canvasHeight; j++)
                {
                    this.background[i, j] = new Vec3d(117, 187, 253);
                }
            }
        }
        public Scene(int canvasWidth, int canvasHeight)
        {
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
            background = new Vec3d[canvasWidth, canvasHeight];
            convertBackground(canvasWidth, canvasHeight);

            camera = new Camera();
            sceneObjects = new List<Primitive>();
            lights = new List<Light>();

            
            sceneObjects.Add(new Plane("плоскость основания", new Vec3d(0, 0, 0), new Vec3d(0, 1, 0), new Vec3d(234, 237, 230), 1, 0));

            lights.Add(new Light("фоновое освещение", LightType.Ambient, new Vec3d(0, 0, 0), 0.2));
            lights.Add(new Light("направленный (Солнце)", LightType.Directional, new Vec3d(1, 0, 0), 0.2));
        }

        public void AddSphere(string name, Vec3d C, double r, Vec3d color, double specular, double reflective)
        {
            sceneObjects.Add(new Sphere(name, C, r, color, specular, reflective));
        }
        public void AddCylinder(string name, Vec3d C, Vec3d V, double r, double maxm, Vec3d color, double specular, double reflective)
        {
            Cylinder cylinder = new Cylinder(name, C, V, r, maxm, color, specular, reflective);
            sceneObjects.Add(cylinder);
            AddDiskPlane(cylinder.name, cylinder.C, -cylinder.V, cylinder.radius, cylinder.color, cylinder.specular, cylinder.reflective);
            AddDiskPlane(cylinder.name, cylinder.C + cylinder.V * maxm, cylinder.V, cylinder.radius, cylinder.color, cylinder.specular, cylinder.reflective);
        }

        public void AddParallelepiped(string name, Vec3d C, Vec3d E, Vec3d color, double specular, double reflective)
        {
            sceneObjects.Add(new Parallelepiped(name, C, E, color, specular, reflective));
        }

        public void AddCone(string name, Vec3d C, Vec3d V, double alpha, double minm, double maxm, Vec3d color, double specular, double reflective)
        {
            double k = Math.Tan(alpha * Math.PI / 180);
            sceneObjects.Add(new Cone(name, C, V, alpha, k, minm, maxm, color, specular, reflective));            
            double r = maxm * k;
            AddDiskPlane(name,C + maxm * V, V, r, color, specular, reflective);
            if (minm > 0)
            {
                r = minm * k;
                AddDiskPlane(name, C + minm * V, V, r, color, specular, reflective);
            }

        }


        public void AddQuadPyramid(string name, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d D, Vec3d color, double specular, double reflective)
        {
            AddTriangle(name, P, A, B, color, specular, reflective);
            AddTriangle(name, P, B, C, color, specular, reflective);
            AddTriangle(name, P, C, D, color, specular, reflective);
            AddTriangle(name, P, D, A, color, specular, reflective);
            AddTriangle(name, A, B, D, color, specular, reflective);
            AddTriangle(name, B, C, D, color, specular, reflective);
            sceneObjects.Add(new QuadPyramid(name, P, A, B, C, D, color, specular, reflective));
        }

        public void AddTrianglePyramid(string name, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d color, double specular, double reflective)
        {
            AddTriangle(name, P, A, B, color, specular, reflective);
            AddTriangle(name, P, B, C, color, specular, reflective);
            AddTriangle(name, P, C, A, color, specular, reflective);
            AddTriangle(name, A, B, C, color, specular, reflective);
            sceneObjects.Add(new TrianglePyramid(name, P, A, B, C, color, specular, reflective));
        }

        public void AddTriangle(string name, Vec3d P, Vec3d A, Vec3d B, Vec3d color, double specular, double reflective)
        {
            sceneObjects.Add(new Triangle(name, P, A, B, color, specular, reflective));
        }

        public void AddPlane(string name, Vec3d C, Vec3d V, Vec3d color, double specular, double reflective)
        {
            sceneObjects.Add(new Plane(name, C, V, color, specular, reflective));
        }

        public void AddDiskPlane(string name, Vec3d C, Vec3d V, double r, Vec3d color, double specular, double reflective)
        {
            sceneObjects.Add(new DiskPlane(name, C, V, r, color, specular, reflective));
        }

        public void AddLightPoint(Vec3d position, double intensity)
        {
            countPointLights += 1;
            lights.Add(new Light("точечный_" + countPointLights, LightType.Point, position, intensity));
        }

        public void UpdateLightPointName()
        {
            int j = 1;
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].ltype == LightType.Point)
                {
                    lights[i].name = "точечный_" + j;
                    j++;
                }
            }
        }

        public void RemoveLightPoint(string name)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].name == name)
                {
                    lights.RemoveRange(i, 1);
                    break;
                }
            }
            countPointLights -= 1;
            UpdateLightPointName();
        }

        public void ChangeLightIntensity(string name, double intensity)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].name == name)
                {
                    lights[i].intensity = intensity;
                    break;
                }
            }
        }

        public void ChangeLightPosition(string name, Vec3d position)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].name == name)
                {
                    lights[i].position = position;
                    break;
                }
            }
        }
    }
}