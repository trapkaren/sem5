using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    class FacadeViewer
    {
        private Controller controller;
        public FacadeViewer(int canvasWidth, int canvasHeight)
        {
            this.controller = new Controller(canvasWidth, canvasHeight);
        }

        public void executeCommand(Command command)
        {
            command.execute(controller);
        }
    }
}
