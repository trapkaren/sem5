using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class LoadCommand : Command
    {
        private string name;

        public LoadCommand(string name)
        {
            this.name = name;
        }
        public override void execute(Controller controller)
        {
            controller.loadScene(this.name);
        }
    }
}
