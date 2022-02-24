using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Spasskaya_tower
{
    class RenderCommand : Command
    {
        private PictureBox canvas;
        private bool drawAxes;

        unsafe public RenderCommand(ref PictureBox canvas, bool drawOXYZ)
        {
            this.canvas = canvas;
            this.drawAxes = drawOXYZ;
        }
        public override void execute(Controller controller)
        {
            controller.render(ref canvas, this.drawAxes);
        }
    }

    class DrawAxesCommand : Command
    {
        private PictureBox canvas;
        private bool drawAxes;

        unsafe public DrawAxesCommand(ref PictureBox canvas, bool draw)
        {
            this.canvas = canvas;
            this.drawAxes = draw;
        }
        public override void execute(Controller controller)
        {
            controller.drawingAxes(ref canvas, this.drawAxes);
        }
    }
}
