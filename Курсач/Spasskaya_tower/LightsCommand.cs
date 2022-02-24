using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class GetLightCommand : Command
    {
        private string name;
        Light light;

        public GetLightCommand(string name)
        {
            this.name = name;
        }
        public override void execute(Controller controller)
        {
            controller.getLight(this.name, ref this.light);
        }

        public Light getResult()
        {
            return this.light;
        }
    }

    class ChangeLightCommand : Command
    {
        private string name;
        Vec3d position;
        double intensity;

        public ChangeLightCommand(string name, Vec3d position, double intensity)
        {
            this.name = name;
            this.position = position;
            this.intensity = intensity;
        }

        public ChangeLightCommand(string name, double intensity)
        {
            this.name = name;
            this.position = null;
            this.intensity = intensity;
        }

        public override void execute(Controller controller)
        {
            if (this.position == null)
                controller.changeLight(this.name, this.intensity);
            else
                controller.changeLight(this.name, this.position, this.intensity);

        }
    }

    class GetLightsNameCommand : Command
    {
        
        List<string> lights;
        
        public override void execute(Controller controller)
        {
            this.lights = controller.getLightsName();
        }

        public List<string> getResult()
        {
            return this.lights;
        }
    }

    class AddLightCommand : Command
    {

        Vec3d position;
        double intensity;

        public AddLightCommand(Vec3d position, double intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }
        public override void execute(Controller controller)
        {
            controller.addLight(this.position, this.intensity);
        }
    }

    class DeleteLightCommand : Command
    {
        private string name;
        public DeleteLightCommand(string name)
        {
            this.name = name;
        }
        public override void execute(Controller controller)
        {
            controller.deleteLight(this.name);
        }
    }
}
