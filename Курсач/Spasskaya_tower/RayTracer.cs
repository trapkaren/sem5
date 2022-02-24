using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;


namespace Spasskaya_tower
{
    class RayTracer
    {
        public Bitmap bmp;
        public Scene scene;

        private int viewport_width = 1;
        private int viewport_height = 1;
        private int projection_plane_d = 1;

        private Color[,] buffer;

        public RayTracer(Scene scene)
        {
            this.scene = scene;
            this.bmp = new Bitmap(scene.canvasWidth, scene.canvasHeight);
            this.buffer = new Color[scene.canvasWidth, scene.canvasHeight];
        }

        private void IntersectRaySphere(Vec3d O, Vec3d D, Sphere sphere, ref double t1, ref double t2)
        {
            Vec3d CO = O - sphere.C;

            double a = Vec3d.ScalarMultiplication(D, D);
            double b = 2 * Vec3d.ScalarMultiplication(CO, D);
            double c = Vec3d.ScalarMultiplication(CO, CO) - sphere.radius * sphere.radius;


            double discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                t1 = Double.PositiveInfinity;
                t2 = Double.PositiveInfinity;
                return;
            }

            t1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            t2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
        }

        private void IntersectRayPlane(Vec3d O, Vec3d D, Vec3d C, Vec3d V, ref double t)
        {
            Vec3d CO = O - C;

            double d_v = Vec3d.ScalarMultiplication(D, V);
            double co_v = Vec3d.ScalarMultiplication(CO, V);

            if (d_v < 0 || d_v > 0)
            {
                t = -co_v / d_v;
                if (t < 0)
                    t = Double.PositiveInfinity;

            }
            else
            {
                t = Double.PositiveInfinity;
            }
        }
        private void IntersectRayDiskPlane(Vec3d O, Vec3d D, Vec3d C, Vec3d V, double r, ref double t)
        {
            IntersectRayPlane(O, D, C, V, ref t);

            if (t != Double.PositiveInfinity)
            {
                Vec3d P = O + D * t;
                Vec3d v = P - C;
                double d2 = Vec3d.ScalarMultiplication(v, v);
                if (Math.Sqrt(d2) > r)
                    t = Double.PositiveInfinity;
            }
            else
            {
                t = Double.PositiveInfinity;
            }
        }

        private void IntersectRayCylinder(Vec3d O, Vec3d D, Cylinder cylinder, ref double t1, ref double t2)
        {
            Vec3d CO = O - cylinder.C;

            double d_d = Vec3d.ScalarMultiplication(D, D);
            double d_co = Vec3d.ScalarMultiplication(D, CO);
            double co_co = Vec3d.ScalarMultiplication(CO, CO);
            double d_v = Vec3d.ScalarMultiplication(D, cylinder.V);
            double co_v = Vec3d.ScalarMultiplication(CO, cylinder.V);

            double a = d_d - d_v * d_v;
            double b = 2 * (d_co - d_v * co_v);
            double c = co_co - co_v * co_v - cylinder.radius * cylinder.radius;


            double discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                t1 = Double.PositiveInfinity;
                t2 = Double.PositiveInfinity;
                return;
            }

            t1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            t2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

            
            double m1 = d_v * t1 + co_v;
            double m2 = d_v * t2 + co_v;

            if (m1 < 0 || m1 > cylinder.maxm)
            {
                t1 = Double.PositiveInfinity;
            }
            if (m2 < 0 || m2 > cylinder.maxm)
            {
                t2 = Double.PositiveInfinity;
            }
        }

        private void IntersectRayCone(Vec3d O, Vec3d D, Cone cone, ref double t1, ref double t2)
        {
            Vec3d CO = O - cone.C;

            double d_d = Vec3d.ScalarMultiplication(D, D);
            double d_co = Vec3d.ScalarMultiplication(D, CO);
            double co_co = Vec3d.ScalarMultiplication(CO, CO);
            double d_v = Vec3d.ScalarMultiplication(D, cone.V);
            double co_v = Vec3d.ScalarMultiplication(CO, cone.V);

            double a = d_d - (1 + cone.k * cone.k) * d_v * d_v;
            double b = 2 * (d_co - (1 + cone.k * cone.k) * d_v * co_v);
            double c = co_co - (1 + cone.k * cone.k) * co_v * co_v;


            double discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                t1 = Double.PositiveInfinity;
                t2 = Double.PositiveInfinity;
                return;
            }

            t1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            t2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

            double m1 = d_v * t1 + co_v;
            double m2 = d_v * t2 + co_v;

            
            if (m1 < cone.minm || m1 > cone.maxm)
            {
                t1 = Double.PositiveInfinity;
            }
            if (m2 < cone.minm || m2 > cone.maxm)
            {
                t2 = Double.PositiveInfinity;
            }

        }
        private void IntersectRayTriangle(Vec3d O, Vec3d D, Vec3d v0, Vec3d v1, Vec3d v2, ref double t1)
        {
            Vec3d e1 = v1 - v0;
            Vec3d e2 = v2 - v0;

            Vec3d pvec = new Vec3d(D.y * e2.z - D.z * e2.y, D.z * e2.x - D.x * e2.z, D.x * e2.y - D.y * e2.x);
            double det = Vec3d.ScalarMultiplication(e1, pvec);

            // Луч параллелен плоскости
            if (det < 1e-8 && det > -1e-8)
            {
                t1 = Double.PositiveInfinity;
                return;
            }

            double inv_det = 1 / det;
            Vec3d tvec = O - v0;
            double u = Vec3d.ScalarMultiplication(tvec, pvec) * inv_det;
            if (u < 0 || u > 1)
            {
                t1 = Double.PositiveInfinity;
                return;
            }
            Vec3d qvec = new Vec3d(tvec.y * e1.z - tvec.z * e1.y, tvec.z * e1.x - tvec.x * e1.z, tvec.x * e1.y - tvec.y * e1.x);

            double v = Vec3d.ScalarMultiplication(D, qvec) * inv_det;

            if (v < 0 || u + v > 1)
            {
                t1 = Double.PositiveInfinity;
                return;
            }

            t1 = Vec3d.ScalarMultiplication(e2, qvec) * inv_det;
        }

        private void IntersectRayParallelepiped(Vec3d O, Vec3d D, Parallelepiped parallelepiped, ref double t1ret, ref double t2ret)
          {
              double t1, t2;
              double tnear = Double.NegativeInfinity;
              double tfar = Double.PositiveInfinity;

              if (Math.Abs(D.x) < 0.001)
              {
                  if (O.x < parallelepiped.C.x || O.x > parallelepiped.E.x)
                {
                    t1ret = Double.PositiveInfinity;
                    t2ret = Double.NegativeInfinity;
                    return;
                }
               
              }
              else
              {

                  t1 = (parallelepiped.C.x - O.x) / D.x;
                  t2 = (parallelepiped.E.x - O.x) / D.x;
                  if (t1 > t2)
                  {
                      double tmp = t1;
                      t1 = t2;
                      t2 = tmp;
                  }
                  if (t1 > tnear) tnear = t1;
                  if (t2 < tfar) tfar = t2;
                  if (tnear > tfar)
                {
                    t1ret = Double.PositiveInfinity;
                    t2ret = Double.NegativeInfinity;
                    return;
                }
                if (tfar < 0)
                {
                    t1ret = Double.PositiveInfinity;
                    t2ret = Double.NegativeInfinity;
                    return;
                }
            }

              if (Math.Abs(D.y) < 0.001)
              {
                  if (O.y < parallelepiped.C.y || O.y > parallelepiped.E.y)
                {
                    t1ret = Double.PositiveInfinity;
                    t2ret = Double.NegativeInfinity;
                    return;
                }
            }
              else
              {

                  t1 = (parallelepiped.C.y - O.y) / D.y;
                  t2 = (parallelepiped.E.y - O.y) / D.y;
                  if (t1 > t2)
                  {
                      double tmp = t1;
                      t1 = t2;
                      t2 = tmp;
                  }
                  if (t1 > tnear) tnear = t1;
                  if (t2 < tfar) tfar = t2;
                  if (tnear > tfar)
                {
                    t1ret = Double.PositiveInfinity;
                    t2ret = Double.NegativeInfinity;
                    return;
                }
                if (tfar < 0)
                {
                    t1ret = Double.PositiveInfinity;
                    t2ret = Double.NegativeInfinity;
                    return;
                }
            }
              if (Math.Abs(D.z) < 0.001)
              {
                  if (O.z < parallelepiped.C.z || O.z > parallelepiped.E.z)
                {
                    t1ret = Double.PositiveInfinity;
                    t2ret = Double.NegativeInfinity;
                    return;
                }
            }
              else
              {

                  t1 = (parallelepiped.C.z - O.z) / D.z;
                  t2 = (parallelepiped.E.z - O.z) / D.z;
                  if (t1 > t2)
                  {
                      double tmp = t1;
                      t1 = t2;
                      t2 = tmp;
                  }
                  if (t1 > tnear) tnear = t1;
                  if (t2 < tfar) tfar = t2;
                  if (tnear > tfar)
                {
                    t1ret = Double.PositiveInfinity;
                    t2ret = Double.NegativeInfinity;
                    return;
                }
                if (tfar < 0)
                {
                    t1ret = Double.PositiveInfinity;
                    t2ret = Double.NegativeInfinity;
                    return;
                }
            }
              t1ret = tnear;
              t2ret = tfar;
          }

        private void ClosestIntersection(ref Primitive closest_object, ref double closest_t, Vec3d O, Vec3d D, double t_min, double t_max)
        {

            double t1 = 0;
            double t2 = 0;

            for (int i = 0; i < this.scene.sceneObjects.Count; i++)
            {
                if (this.scene.sceneObjects[i] is Sphere)
                {
                    IntersectRaySphere(O, D, (Sphere)this.scene.sceneObjects[i], ref t1, ref t2);
                }
                else if (this.scene.sceneObjects[i] is Plane)
                {
                    Plane tmp = (Plane)this.scene.sceneObjects[i];
                    IntersectRayPlane(O, D, tmp.C, tmp.V, ref t1);
                    t2 = t1;
                }
                else if (this.scene.sceneObjects[i] is DiskPlane)
                {
                    DiskPlane tmp = (DiskPlane)this.scene.sceneObjects[i];
                    IntersectRayDiskPlane(O, D, tmp.C, tmp.V, tmp.r, ref t1);
                    t2 = t1;
                }
                else if (this.scene.sceneObjects[i] is Triangle)
                {
                    Triangle tmp = (Triangle)this.scene.sceneObjects[i];
                    IntersectRayTriangle(O, D, tmp.C, tmp.A, tmp.B, ref t1);
                    t2 = t1;
                }
                else if (this.scene.sceneObjects[i] is Cylinder)
                {
                    IntersectRayCylinder(O, D, (Cylinder)this.scene.sceneObjects[i], ref t1, ref t2);
                }
                else if (this.scene.sceneObjects[i] is Cone)
                {
                    IntersectRayCone(O, D, (Cone)this.scene.sceneObjects[i], ref t1, ref t2);
                }
                else if (this.scene.sceneObjects[i] is Parallelepiped)
                {
                    IntersectRayParallelepiped(O, D, (Parallelepiped)this.scene.sceneObjects[i], ref t1, ref t2);
                    
                }

                if (t1 < closest_t && t_min < t1 && t1 < t_max)
                {
                    closest_t = t1;
                    closest_object = this.scene.sceneObjects[i];
                }
                if (t2 < closest_t && t_min < t2 && t2 < t_max)
                {
                    closest_t = t2;
                    closest_object = this.scene.sceneObjects[i];
                }
            }
        }
        private Vec3d ReflectRay(Vec3d R, Vec3d N)
        {
            double n_dot_r = Vec3d.ScalarMultiplication(N, R);
            Vec3d res = 2 * N * n_dot_r - R;
            return res;
        }

        private Vec3d Vec3dNormalParallelepiped(Vec3d P, Parallelepiped parallelepiped)
        {
            Vec3d size = parallelepiped.E - parallelepiped.C;
            Vec3d C = parallelepiped.E + parallelepiped.C;
            C = C * 0.5;


            Vec3d localPoint = P - C;
            Vec3d normal = new Vec3d(1, 0, 0);

            normal.x = normal.x * Math.Sign(localPoint.x);
            double distance = Math.Abs(size.x - Math.Abs(localPoint.x));
            double min = distance;
            
            distance = Math.Abs(size.y - Math.Abs(localPoint.y));

            if (distance < min)
            {
                min = distance;

                normal = new Vec3d(0, 1, 0);

                normal.y = normal.y * Math.Sign(localPoint.y);
                
            }
            distance = Math.Abs(size.z - Math.Abs(localPoint.z));
            if (distance < min)
            {
                min = distance;
                normal = new Vec3d(0, 0, 1);

                normal.z = normal.z * Math.Sign(localPoint.z);
            }
            return normal;

        }

        private Vec3d Vec3dNormalCylinder(Vec3d P, double closest_t, Cylinder cylinder, Vec3d O, Vec3d D)
        {

            Vec3d CO = O - cylinder.C;


            double d_v = Vec3d.ScalarMultiplication(D, cylinder.V);
            double co_v = Vec3d.ScalarMultiplication(CO, cylinder.V);

            double m = d_v * closest_t + co_v;
            Vec3d normal = P;
            normal = normal - cylinder.C;
            normal = normal - cylinder.V * m;
            return normal;

        }

        private Vec3d Vec3dNormalCone(Vec3d P, double closest_t, Cone cone, Vec3d O, Vec3d D)
        {

            Vec3d CO = O - cone.C;

            double d_v = Vec3d.ScalarMultiplication(D, cone.V);
            double co_v = Vec3d.ScalarMultiplication(CO, cone.V);

            double m = d_v * closest_t + co_v;
            
            Vec3d normal = P - cone.C - (1 + cone.k * cone.k) * cone.V * m;
            
            return normal;

        }

        private Vec3d Vec3dNormalTriangle(Triangle closest_object)
        {
            Vec3d e1 = closest_object.A - closest_object.C;
            Vec3d e2 = closest_object.B - closest_object.C;
            Vec3d normal = new Vec3d(e1.y * e2.z - e1.z * e2.y, e1.z * e2.x - e1.x * e2.z, e1.x * e2.y - e1.y * e2.x);
            double len_n = Vec3d.Length(normal);
            normal = normal / len_n;
            return normal;
        }

        private Vec3d TraceRay(Vec3d O, Vec3d D, double t_min, double t_max, int depth, int x, int y)
        {
            double closest_t = Double.PositiveInfinity;
            Primitive closest_object = null;

            ClosestIntersection(ref closest_object, ref closest_t, O, D, t_min, t_max);

            if (closest_object == null)
            {
                x += 330;
                y += 330;
                return scene.background[x, y];
            }


            Vec3d P = O + closest_t * D;
            Vec3d N;
            if (closest_object is Parallelepiped)
                N = Vec3dNormalParallelepiped(P, (Parallelepiped)closest_object);
            else if (closest_object is Cylinder)
                N = Vec3dNormalCylinder(P, closest_t, (Cylinder)closest_object, O, D);
            else if (closest_object is Cone)
                N = Vec3dNormalCone(P, closest_t, (Cone)closest_object, O, D);
            else if (closest_object is Triangle)
            {
                N = Vec3dNormalTriangle((Triangle)closest_object);
            }
            else if (closest_object is Plane)
            {
                Plane tmp = (Plane)closest_object;
                N = tmp.V;
            }
            else if (closest_object is DiskPlane)
            {
                DiskPlane tmp = (DiskPlane)closest_object;
                N = tmp.V;
            }
            else
                N = P - closest_object.C;


            N = N / Vec3d.Length(N); 

            double intensity = ComputeLighting(P, N, -D, closest_object.specular);

            Vec3d localColor = intensity * closest_object.color; 

            double r = closest_object.reflective;

            if (depth <= 0 || r <= 0)
                return localColor;

            Vec3d R = ReflectRay(-D, N);
            Vec3d reflectedColor = TraceRay(P, R, 0.001, Double.PositiveInfinity, depth - 1, x, y);

            Vec3d kLocalColor = (1 - r) * localColor;
            Vec3d rReflectedColor = r * reflectedColor;

            return kLocalColor+rReflectedColor;
        }

        private double ComputeLighting(Vec3d P, Vec3d N, Vec3d V, double specular)
        {
            double intensity = 0;
            List<Light> sceneLight = scene.lights;
            
            for (int i = 0; i < sceneLight.Count; i++)
            {
                if (sceneLight[i].ltype == LightType.Ambient)
                {
                    intensity += sceneLight[i].intensity;
                }
                else
                {
                    Vec3d L;
                    double t_max;
                    if (sceneLight[i].ltype == LightType.Point)
                    {
                        L = sceneLight[i].position - P;
                        t_max = 1;                    
                    }
                    else
                    {
                        L = sceneLight[i].position;
                        t_max = Double.MaxValue;
                    }

                    double shadow_t = Double.PositiveInfinity;
                    Primitive shadow_object = null;
                    ClosestIntersection(ref shadow_object, ref shadow_t, P, L, 0.001, t_max);
                    if (shadow_object != null)
                        continue;

                    double n_dot_l = Vec3d.ScalarMultiplication(N, L);

                    if (n_dot_l > 0)
                    {
                        intensity += sceneLight[i].intensity * n_dot_l / (Vec3d.Length(N) * Vec3d.Length(L));
                    }

                    if (specular != -1) 
                    {

                        Vec3d R = ReflectRay(L, N);
                        double r_dot_v = Vec3d.ScalarMultiplication(R, V);

                        if (r_dot_v > 0)
                        {
                            intensity += sceneLight[i].intensity * Math.Pow(r_dot_v / (Vec3d.Length(R) * Vec3d.Length(V)), specular);
                        }
                    }
                }
            }
            return intensity;
        }
        private void PutPixel(int x, int y, Color color)
        {
            int x_ = scene.canvasWidth / 2 + x;
            int y_ = scene.canvasHeight / 2 - y - 1;

            if (x_ < 0 || x_ >= scene.canvasWidth || y_ < 0 || y_ >= scene.canvasHeight)
            {
                return;
            }

            this.buffer[x_,y_] = color;    
        }

        private Color Clamp(Vec3d color)
        {
            int color_x = Math.Min(255, Math.Max(0, (int)color.x));
            int color_y = Math.Min(255, Math.Max(0, (int)color.y));
            int color_z = Math.Min(255, Math.Max(0, (int)color.z));
            return Color.FromArgb(color_x, color_y, color_z);
        }


        private void rendering(object obj)
        {
            Params p = (Params)obj;
            Camera camera = scene.camera;
            Vec3d D = null;
            Vec3d color = null;
            int recursion_depth = 3;
            for (int x = p.start_x; x < p.start_x + p.width; x++)
            {
                for (int y = p.start_y; y < p.start_y + p.height; y++)
                {
                    D = CanvasToViewport(x, y) * camera.rotation;
                    color = TraceRay(camera.position, D, 1, Double.PositiveInfinity, recursion_depth, x, y);
                    PutPixel(x, y, Clamp(color));

                }
            }
            
        }
        public Bitmap render()
        {
            Thread[] threads = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                Params p = new Params(scene.canvasWidth / 4, scene.canvasHeight, -scene.canvasWidth / 2 + scene.canvasWidth / 4 * i, -scene.canvasHeight / 2);
                threads[i] = new Thread(rendering);
                threads[i].Start(p);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

          
            for (int i = 0; i < scene.canvasWidth; i++)
                for (int j = 0; j < scene.canvasHeight; j++)
                    this.bmp.SetPixel(i, j, buffer[i,j]);

            
      
            return this.bmp;
        }


        private Vec3d CanvasToViewport(int x, int y)
        {
            return new Vec3d(x * (double)viewport_width / scene.canvasWidth, y * (double)viewport_height / scene.canvasHeight, projection_plane_d);
        }
    }

    class Params
    {
        public int width;
        public int height;
        public int start_x;
        public int start_y;
        public Params(int width, int height, int start_x, int start_y)
        {
            this.width = width;
            this.height = height;
            this.start_x = start_x;
            this.start_y = start_y;
        }
    }
}
