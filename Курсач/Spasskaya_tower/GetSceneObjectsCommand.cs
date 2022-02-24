using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spasskaya_tower
{
    class GetSceneObjectsCommand : Command
    {
        private List<Primitive> sceneObjects;

        public override void execute(Controller controller)
        {
            sceneObjects = controller.getSceneObjects();
        }

        public List<Primitive> getResult()
        {
            return this.sceneObjects;
        }
    }
}
