using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class DeletePrimitiveCommand : Command
    {
        private string name;
        
        public DeletePrimitiveCommand(string name)
        {
            this.name = name;
        }
        public override void execute(Controller controller)
        {
            controller.deletePrimitive(this.name);
        }
    }

    class ClearSceneCommand : Command
    {

        public override void execute(Controller controller)
        {
            controller.clearScene();
        }
    }
}
