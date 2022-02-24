using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class RollCameraCommand : Command
    {
        private double angle;

        public RollCameraCommand(double angle)
        {
            this.angle = angle;
        }
        public override void execute(Controller controller)
        {
            controller.rollCamera(angle);
        }
    }

    class YawCameraCommand : Command
    {
        private double angle;

        public YawCameraCommand(double angle)
        {
            this.angle = angle;
        }
        public override void execute(Controller controller)
        {
            controller.yawCamera(angle);
        }
    }

    class PitchCameraCommand : Command
    {
        private double angle;

        public PitchCameraCommand(double angle)
        {
            this.angle = angle;
        }
        public override void execute(Controller controller)
        {
            controller.pitchCamera(angle);
        }
    }

    class MoveCameraCommand : Command
    {
        private double dx;
        private double dy;
        private double dz;

        public MoveCameraCommand(double dx, double dy, double dz)
        {
            this.dx = dx;
            this.dy = dy;
            this.dz = dz;
        }
        public override void execute(Controller controller)
        {
            controller.moveCamera(this.dx, this.dy, this.dz);
        }
    }

    class GetCameraCommand : Command
    {
        private Camera camera;
  
        public override void execute(Controller controller)
        {
            camera = controller.getCamera();
        }
        public Camera getResult()
        {
            return this.camera;
        }
    }
}
