using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Spasskaya_tower
{
    class SaveScene
    {
        public void savingScene(string filename, Scene scene)
        {
            Converter convert = new Converter();
            convert.SceneObjectToJson(scene);
            string output = JsonConvert.SerializeObject(convert);
            System.IO.File.WriteAllText(filename, output);
        }
    }
}
