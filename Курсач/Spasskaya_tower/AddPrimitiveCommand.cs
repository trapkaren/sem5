using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class AddSphereCommand : Command
    {
        private string name;
        private Vec3d C;
        private double radius;
        private Vec3d color;
        private double specular;
        private double reflective;



        public AddSphereCommand(string name, Vec3d C, double radius, Vec3d color, double specular, double reflective)
        {
            this.name = name;
            this.C = C;
            this.radius = radius;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.AddSphereToScene(this.name, this.C, this.radius, this.color, this.specular, this.reflective);
        }
    }

    class AddCylinderCommand : Command
    {
        private string name;
        private Vec3d C;
        private double radius;
        private Vec3d V;
        private double maxm;
        private Vec3d color;
        private double specular;
        private double reflective;



        public AddCylinderCommand(string name, Vec3d C, Vec3d V, double radius, double maxm, Vec3d color, double specular, double reflective)
        {
            this.name = name;
            this.C = C;
            this.radius = radius;
            this.V = V;
            this.maxm = maxm;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.AddCylinderToScene(this.name, this.C, this.V, this.radius, this.maxm, this.color, this.specular, this.reflective);
        }
    }

    class AddConeCommand : Command
    {
        private string name;
        private Vec3d C;
        private double k;
        private Vec3d V;
        private double maxm;
        private double minm;
        private Vec3d color;
        private double specular;
        private double reflective;



        public AddConeCommand(string name, Vec3d C, Vec3d V, double k, double minm, double maxm, Vec3d color, double specular, double reflective)
        {
            this.name = name;
            this.C = C;
            this.k = k;
            this.V = V;
            this.minm = minm;
            this.maxm = maxm;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.AddConeToScene(this.name, this.C, this.k, this.V, this.minm, this.maxm,  this.color, this.specular, this.reflective);
        }
    }

    class AddQuadPyramidCommand : Command
    {
        private string name;
        private Vec3d P;
        private Vec3d A;
        private Vec3d B;
        private Vec3d C;
        private Vec3d D;
        private Vec3d color;
        private double specular;
        private double reflective;



        public AddQuadPyramidCommand(string name, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d D, Vec3d color, double specular, double reflective)
        {
            this.name = name;
            this.P = P;
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.AddQuadPyramidToScene(this.name, this.P, this.A, this.B, this.C, this.D, this.color, this.specular, this.reflective);
        }
    }

    class AddTrianglePyramidCommand : Command
    {
        private string name;
        private Vec3d P;
        private Vec3d A;
        private Vec3d B;
        private Vec3d C;
        private Vec3d color;
        private double specular;
        private double reflective;



        public AddTrianglePyramidCommand(string name, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d color, double specular, double reflective)
        {
            this.name = name;
            this.P = P;
            this.A = A;
            this.B = B;
            this.C = C;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.AddTrianglePyramidToScene(this.name, this.P, this.A, this.B, this.C, this.color, this.specular, this.reflective);
        }
    }

    class AddParallelepipedCommand : Command
    {
        private string name;
        private Vec3d C;
        private Vec3d E;
        private Vec3d color;
        private double specular;
        private double reflective;

        public AddParallelepipedCommand(string name, Vec3d C, Vec3d E, Vec3d color, double specular, double reflective)
        {
            this.name = name;
            this.C = C;
            this.E = E;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.AddParallelepipedToScene(this.name, this.C, this.E, this.color, this.specular, this.reflective);
        }
    }

    class CheckFreeNamePrimitiveCommand : Command
    {
        private string name;
      
        public CheckFreeNamePrimitiveCommand(string name)
        {
            this.name = name;
        }
        public override void execute(Controller controller)
        {
            bool check = controller.CheckFreeNamePrimitive(this.name);
            if (!check)
                throw new Exception("Примитив с таким именем уже есть!");
        }
    }

}
