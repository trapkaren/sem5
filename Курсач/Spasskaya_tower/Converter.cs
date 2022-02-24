using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Spasskaya_tower
{
    class Converter
    {
        [JsonProperty("camera")]
        private Camera camera;

        [JsonProperty("spheres")]
        private Sphere[] spheres;

        [JsonProperty("cylinders")]
        private Cylinder[] cylinders;

        [JsonProperty("cones")]
        private Cone[] cones;

        [JsonProperty("quadPyramids")]
        private QuadPyramid[] quadPyramids;

        [JsonProperty("trianglePyramids")]
        private TrianglePyramid[] trianglePyramids;

        [JsonProperty("parallelepipeds")]
        private Parallelepiped[] parallelepipeds;

        [JsonProperty("plane")]
        private Plane[] planes;

        [JsonProperty("lights")]
        private Light[] lights;


        
        public void SceneObjectToJson(Scene scene)
        {
            this.camera = scene.camera;

            List<Sphere> spheres = new List<Sphere>();
            List<Cylinder> cylinders = new List<Cylinder>();
            List<Cone> cones = new List<Cone>();
            List<QuadPyramid> quadPyramids = new List<QuadPyramid>();
            List<TrianglePyramid> trianglePyramids = new List<TrianglePyramid>();
            List<Parallelepiped> parallelepipeds = new List<Parallelepiped>();
            List<Plane> planes = new List<Plane>();

            for (int i = 0; i < scene.sceneObjects.Count; i++)
            {
                if (scene.sceneObjects[i] is Sphere)
                    spheres.Add((Sphere)scene.sceneObjects[i]);

                else if (scene.sceneObjects[i] is Cylinder)
                    cylinders.Add((Cylinder)scene.sceneObjects[i]);

                else if (scene.sceneObjects[i] is Cone)
                    cones.Add((Cone)scene.sceneObjects[i]);

                else if (scene.sceneObjects[i] is QuadPyramid)
                    quadPyramids.Add((QuadPyramid)scene.sceneObjects[i]);

                else if (scene.sceneObjects[i] is TrianglePyramid)
                    trianglePyramids.Add((TrianglePyramid)scene.sceneObjects[i]);

                else if (scene.sceneObjects[i] is Parallelepiped)
                    parallelepipeds.Add((Parallelepiped)scene.sceneObjects[i]);

                else if (scene.sceneObjects[i] is Plane)
                    planes.Add((Plane)scene.sceneObjects[i]);
            }

            this.spheres = spheres.ToArray();
            this.cylinders = cylinders.ToArray();
            this.cones = cones.ToArray();
            this.quadPyramids = quadPyramids.ToArray();
            this.trianglePyramids = trianglePyramids.ToArray();
            this.parallelepipeds = parallelepipeds.ToArray();
            this.planes = planes.ToArray();

            this.lights = scene.lights.ToArray();

        }
        public void JsonToSceneObject(ref Scene scene)
        {
            scene.camera = this.camera;

            scene.sceneObjects.Clear();

            for (int i = 0; i < this.spheres.Length; i++)
                scene.AddSphere(this.spheres[i].name, this.spheres[i].C, this.spheres[i].radius, this.spheres[i].color, this.spheres[i].specular, this.spheres[i].reflective);

            for (int i = 0; i < this.cylinders.Length; i++)
                scene.AddCylinder(this.cylinders[i].name, this.cylinders[i].C, this.cylinders[i].V, this.cylinders[i].radius, this.cylinders[i].maxm, this.cylinders[i].color, this.cylinders[i].specular, this.cylinders[i].reflective);

            for (int i = 0; i < this.cones.Length; i++)
                scene.AddCone(this.cones[i].name, this.cones[i].C, this.cones[i].V, this.cones[i].angle, this.cones[i].minm, this.cones[i].maxm, this.cones[i].color, this.cones[i].specular, this.cones[i].reflective);

            for (int i = 0; i < this.quadPyramids.Length; i++)
                scene.AddQuadPyramid(this.quadPyramids[i].name, this.quadPyramids[i].P, this.quadPyramids[i].A, this.quadPyramids[i].B, this.quadPyramids[i].C, this.quadPyramids[i].D, this.quadPyramids[i].color, this.quadPyramids[i].specular, this.quadPyramids[i].reflective);

            for (int i = 0; i < this.trianglePyramids.Length; i++)
                scene.AddTrianglePyramid(this.trianglePyramids[i].name, this.trianglePyramids[i].P, this.trianglePyramids[i].A, this.trianglePyramids[i].B, this.trianglePyramids[i].C, this.trianglePyramids[i].color, this.trianglePyramids[i].specular, this.trianglePyramids[i].reflective);

            for (int i = 0; i < this.parallelepipeds.Length; i++)
                scene.AddParallelepiped(this.parallelepipeds[i].name, this.parallelepipeds[i].C, this.parallelepipeds[i].E, this.parallelepipeds[i].color, this.parallelepipeds[i].specular, this.parallelepipeds[i].reflective);

            for (int i = 0; i < this.planes.Length; i++)
                scene.AddPlane(this.planes[i].name, this.planes[i].C, this.planes[i].V, this.planes[i].color, this.planes[i].specular, this.planes[i].reflective);

            scene.lights.Clear();

            for (int i = 0; i < this.lights.Length; i++)
                scene.lights.Add(this.lights[i]);

        }
    }
}
