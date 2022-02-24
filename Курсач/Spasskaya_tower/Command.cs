using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spasskaya_tower
{
    abstract class Command
    {
        abstract public void execute(Controller controller);
    }
}
