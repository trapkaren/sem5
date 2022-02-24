using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Spasskaya_tower
{
    class Controller
    {
        private Scene scene;
        private Scene sceneOXYZ;
        private RayTracer rayTracer;
        private RayTracer rayTracerOXYZ;
        private LoadScene loaderScene;
        private SaveScene saverScene;
        private CameraManager cameraManager;

        private Bitmap tmp;
        private Bitmap tmp_axes;
        public Controller(int canvasWidth, int canvasHeight)
        {
            this.scene = new Scene(canvasWidth, canvasHeight);
            this.rayTracer = new RayTracer(this.scene);
            this.loaderScene = new LoadScene();
            this.saverScene = new SaveScene();
            this.cameraManager = new CameraManager();


            this.sceneOXYZ = new Scene(canvasWidth, canvasHeight);
            this.sceneOXYZ.lights.RemoveRange(1, 1);
            this.sceneOXYZ.lights[0].intensity = 1;
            this.sceneOXYZ.sceneObjects.Clear();
            this.sceneOXYZ.AddCylinder("OX", new Vec3d(0, 0, 0), new Vec3d(1, 0, 0), 0.05, 500, new Vec3d(255, 0, 0), 0, 0);
            this.sceneOXYZ.AddCylinder("OY", new Vec3d(0, 0, 0), new Vec3d(0, 1, 0), 0.05, 500, new Vec3d(0, 255, 0), 0, 0);
            this.sceneOXYZ.AddCylinder("OZ", new Vec3d(0, 0, 0), new Vec3d(0, 0, 1), 0.05, 500, new Vec3d(0, 0, 255), 0, 0);
            this.rayTracerOXYZ = new RayTracer(this.sceneOXYZ);
        }

        public void yawCamera(double angle)
        {
            Camera tmp_camera = this.scene.camera;
            this.cameraManager.yawCamera(ref tmp_camera, angle);
            this.scene.camera = tmp_camera;
        }

        public void pitchCamera(double angle)
        {
            Camera tmp_camera = this.scene.camera;
            this.cameraManager.pitchCamera(ref tmp_camera, angle);
            this.scene.camera = tmp_camera;
        }

        public void rollCamera(double angle)
        {
            Camera tmp_camera = this.scene.camera;
            this.cameraManager.rollCamera(ref tmp_camera, angle);
            this.scene.camera = tmp_camera;
        }

        public void moveCamera(double dx, double dy, double dz)
        {
            Camera tmp_camera = this.scene.camera;
            this.cameraManager.moveCamera(ref tmp_camera, dx, dy, dz);
            this.scene.camera = tmp_camera;
        }

        public void loadScene(string filename)
        {
            this.loaderScene.loadingScene(filename, ref this.scene);
        }

        public void saveScene(string filename)
        {
            this.saverScene.savingScene(filename, this.scene);
        }

        public void AddSphereToScene(string filename)
        {
            this.saverScene.savingScene(filename, this.scene);
        }

        public void AddSphereToScene(string name, Vec3d C, double radius, Vec3d color, double specular, double reflective)
        {
            scene.AddSphere(name, C, radius, color, specular, reflective);
        }
        public void AddCylinderToScene(string name, Vec3d C, Vec3d V, double radius, double maxm, Vec3d color, double specular, double reflective)
        {
            scene.AddCylinder(name, C, V, radius, maxm, color, specular, reflective);
        }

        public void AddConeToScene(string name, Vec3d C, double k, Vec3d V, double minm, double maxm, Vec3d color, double specular, double reflective)
        {
            scene.AddCone(name, C, V, k, minm, maxm, color, specular, reflective);
        }

        public void AddQuadPyramidToScene(string name, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d D, Vec3d color, double specular, double reflective)
        {
            scene.AddQuadPyramid(name, P, A, B, C, D, color, specular, reflective);
        }

        public void AddTrianglePyramidToScene(string name, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d color, double specular, double reflective)
        {
            scene.AddTrianglePyramid(name, P, A, B, C, color, specular, reflective);
        }

        public void AddParallelepipedToScene(string name, Vec3d C, Vec3d E, Vec3d color, double specular, double reflective)
        {
            scene.AddParallelepiped(name, C, E, color, specular, reflective);
        }
        public void render(ref PictureBox canvas, bool drawAxes)
        {
            rayTracer.scene = this.scene;
            tmp = rayTracer.render();        


            this.sceneOXYZ.convertBackgroundReverse(this.scene.canvasWidth, this.scene.canvasHeight);
            this.sceneOXYZ.camera = this.scene.camera;
            tmp_axes = rayTracerOXYZ.render();

            drawingAxes(ref canvas, drawAxes);

        }
        public void drawingAxes(ref PictureBox canvas, bool draw)
        {
            if (draw == true)
                canvas.Image = tmp_axes;
            else
                canvas.Image = tmp;
        }

        public bool CheckFreeNamePrimitive(string name)
        {
            for (int i = 0; i < scene.sceneObjects.Count; i++)
                if (scene.sceneObjects[i].name == name)
                    return false;
            return true;
        }

        public List<Primitive> getSceneObjects()
        {
            return this.scene.sceneObjects;
        }

        public void getLight(string name, ref Light light)
        {
            for (int i = 0; i < this.scene.lights.Count; i++)
            {
                if (this.scene.lights[i].name == name)
                {
                    light = this.scene.lights[i];
                    break;
                }
            }
        }

        public void changeLight(string name, Vec3d position, double intensity)
        {
            for (int i = 0; i < this.scene.lights.Count; i++)
            {
                if (this.scene.lights[i].name == name)
                {
                    this.scene.lights[i].position = position;
                    this.scene.lights[i].intensity = intensity;
                    break;
                }
            }
        }
        public void changeLight(string name, double intensity)
        {
            for (int i = 0; i < this.scene.lights.Count; i++)
            {
                if (this.scene.lights[i].name == name)
                {
                    this.scene.lights[i].intensity = intensity;
                    break;
                }
            }
        }

        public List<string> getLightsName()
        {

            List<string> lightsName = new List<string>();
            for (int i = 0; i < this.scene.lights.Count; i++)
            {
                lightsName.Add(this.scene.lights[i].name);
            }
            return lightsName;
        }

        public void addLight(Vec3d position, double intensity)
        {
            this.scene.AddLightPoint(position, intensity);
        }

        public void deleteLight(string name)
        {
            this.scene.RemoveLightPoint(name);
        }

        public void updateSphere(string oldname, string newname, Vec3d C, double r, Vec3d color, double specular, double reflective)
        {
            int index = -1;
            
            for (int i = 0; i < this.scene.sceneObjects.Count; i++)
            {
                if (this.scene.sceneObjects[i].name == oldname)
                {
                    index = i;
                    break;
                }  
            }
            if (index != -1)
            {
                Sphere tmp = (Sphere)this.scene.sceneObjects[index];
                tmp.name = newname;
                tmp.C = C;
                tmp.radius = r;
                tmp.color = color;
                tmp.specular = specular;
                tmp.reflective = reflective;
                this.scene.sceneObjects[index] = tmp;
            }
        }

        public void updateBasePlane(Vec3d color, double specular, double reflective)
        {

            for (int i = 0; i < this.scene.sceneObjects.Count; i++)
            {
                if (this.scene.sceneObjects[i].name == "плоскость основания")
                {
                    this.scene.sceneObjects[i].color = color;
                    this.scene.sceneObjects[i].specular = specular;
                    this.scene.sceneObjects[i].reflective = reflective;
                    break;
                }
            }
        }

        public void updateParallelepiped(string oldname, string newname, Vec3d C, Vec3d E, Vec3d color, double specular, double reflective)
        {
            int index = -1;

            for (int i = 0; i < this.scene.sceneObjects.Count; i++)
            {
                if (this.scene.sceneObjects[i].name == oldname)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                Parallelepiped tmp = (Parallelepiped)this.scene.sceneObjects[index];
                tmp.name = newname;
                tmp.C = C;
                tmp.E = E;
                tmp.color = color;
                tmp.specular = specular;
                tmp.reflective = reflective;
                this.scene.sceneObjects[index] = tmp;
            }
        }

        public void updateTrianglePyramid(string oldname, string newname, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d color, double specular, double reflective)
        {

            for (int i = 0; i < this.scene.sceneObjects.Count; i++)
            {
                if (this.scene.sceneObjects[i].name == oldname)
                {
                    this.scene.sceneObjects.RemoveRange(i, 1);
                    i--;
                }
            }
            this.scene.AddTrianglePyramid(newname, P, A, B, C, color, specular, reflective);
        }

        public void updateQuadPyramid(string oldname, string newname, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d D, Vec3d color, double specular, double reflective)
        {

            for (int i = 0; i < this.scene.sceneObjects.Count; i++)
            {
                if (this.scene.sceneObjects[i].name == oldname)
                {
                    this.scene.sceneObjects.RemoveRange(i, 1);
                    i--;
                }
            }
            this.scene.AddQuadPyramid(newname, P, A, B, C, D, color, specular, reflective);
        }

        public void updateCylinder(string oldname, string newname, Vec3d C, Vec3d V, double r, double maxm, Vec3d color, double specular, double reflective)
        {

            for (int i = 0; i < this.scene.sceneObjects.Count; i++)
            {
                if (this.scene.sceneObjects[i].name == oldname)
                {
                    this.scene.sceneObjects.RemoveRange(i, 1);
                    i--;
                }
            }
            this.scene.AddCylinder(newname, C, V, r, maxm, color, specular, reflective);
        }

        public void updateCone(string oldname, string newname, Vec3d C, Vec3d V, double alpha, double minm, double maxm, Vec3d color, double specular, double reflective)
        {

            for (int i = 0; i < this.scene.sceneObjects.Count; i++)
            {
                if (this.scene.sceneObjects[i].name == oldname)
                {
                    this.scene.sceneObjects.RemoveRange(i, 1);
                    i--;
                }
            }
            this.scene.AddCone(newname, C, V, alpha, minm, maxm, color, specular, reflective);
        }


        public void deletePrimitive(string name)
        {

            for (int i = 0; i < this.scene.sceneObjects.Count; i++)
            {
                if (this.scene.sceneObjects[i].name == name)
                {
                    this.scene.sceneObjects.RemoveRange(i, 1);
                    i--;
                }
            }
        }

        public Camera getCamera()
        {
            return this.scene.camera;
        }

        public void clearScene()
        {
            for (int i = 0; i < this.scene.sceneObjects.Count; i++)
            {
                if (this.scene.sceneObjects[i] is Plane)
                {
                    continue;
                }
                this.scene.sceneObjects.RemoveRange(i, 1);
                i--;
            }
        }
    }
}