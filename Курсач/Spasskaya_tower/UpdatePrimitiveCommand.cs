using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class UpdateSphereCommand : Command
    {
        private string oldname;
        private string newname;
        private Vec3d C;
        private double r;
        private double specular;
        private double reflective;
        private Vec3d color;

        public UpdateSphereCommand(string oldname, string newname, Vec3d C, double r, Vec3d color, double specular, double reflective)
        {
            this.oldname = oldname;
            this.newname = newname;
            this.C = C;
            this.r = r;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.updateSphere(this.oldname, this.newname, this.C, this.r, this.color, this.specular, this.reflective);
        }
    }

    class UpdateParallelepipedCommand : Command
    {
        private string oldname;
        private string newname;
        private Vec3d C;
        private Vec3d E;
        private double specular;
        private double reflective;
        private Vec3d color;

        public UpdateParallelepipedCommand(string oldname, string newname, Vec3d C, Vec3d E, Vec3d color, double specular, double reflective)
        {
            this.oldname = oldname;
            this.newname = newname;
            this.C = C;
            this.E = E;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.updateParallelepiped(this.oldname, this.newname, this.C, this.E, this.color, this.specular, this.reflective);
        }
    }

    class UpdateTrianglePyramidCommand : Command
    {
        private string oldname;
        private string newname;
        private Vec3d P;
        private Vec3d A;
        private Vec3d B;
        private Vec3d C;
        private double specular;
        private double reflective;
        private Vec3d color;

        public UpdateTrianglePyramidCommand(string oldname, string newname, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d color, double specular, double reflective)
        {
            this.oldname = oldname;
            this.newname = newname;
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
            controller.updateTrianglePyramid(this.oldname, this.newname, this.P, this.A, this.B, this.C, this.color, this.specular, this.reflective);
        }
    }

    class UpdateQuadPyramidCommand : Command
    {
        private string oldname;
        private string newname;
        private Vec3d P;
        private Vec3d A;
        private Vec3d B;
        private Vec3d C;
        private Vec3d D;
        private double specular;
        private double reflective;
        private Vec3d color;

        public UpdateQuadPyramidCommand(string oldname, string newname, Vec3d P, Vec3d A, Vec3d B, Vec3d C, Vec3d D, Vec3d color, double specular, double reflective)
        {
            this.oldname = oldname;
            this.newname = newname;
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
            controller.updateQuadPyramid(this.oldname, this.newname, this.P, this.A, this.B, this.C, this.D, this.color, this.specular, this.reflective);
        }
    }

    class UpdateCylinderCommand : Command
    {
        private string oldname;
        private string newname;
        private Vec3d C;
        private Vec3d V;
        private double r;
        private double maxm;
        private double specular;
        private double reflective;
        private Vec3d color;

        public UpdateCylinderCommand(string oldname, string newname, Vec3d C, Vec3d V, double r, double maxm, Vec3d color, double specular, double reflective)
        {
            this.oldname = oldname;
            this.newname = newname;
            this.C = C;
            this.V = V;
            this.r = r;
            this.maxm = maxm;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.updateCylinder(this.oldname, this.newname, this.C, this.V, this.r, this.maxm, this.color, this.specular, this.reflective);
        }
    }

    class UpdateBasePlaneCommand : Command
    {
        private double specular;
        private double reflective;
        private Vec3d color;

        public UpdateBasePlaneCommand(Vec3d color, double specular, double reflective)
        {
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.updateBasePlane(this.color, this.specular, this.reflective);
        }
    }

    class UpdateConeCommand : Command
    {
        private string oldname;
        private string newname;
        private Vec3d C;
        private Vec3d V;
        private double alpha;
        private double minm;
        private double maxm;
        private double specular;
        private double reflective;
        private Vec3d color;

        public UpdateConeCommand(string oldname, string newname, Vec3d C, Vec3d V, double alpha, double minm, double maxm, Vec3d color, double specular, double reflective)
        {
            this.oldname = oldname;
            this.newname = newname;
            this.C = C;
            this.V = V;
            this.alpha = alpha;
            this.minm = minm;
            this.maxm = maxm;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }
        public override void execute(Controller controller)
        {
            controller.updateCone(this.oldname, this.newname, this.C, this.V, this.alpha, this.minm, this.maxm, this.color, this.specular, this.reflective);
        }
    }
}
