using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Spasskaya_tower
{
    class LoadScene
    {
        public void loadingScene(string filename, ref Scene scene)
        {
            string input = System.IO.File.ReadAllText(filename);
            Converter convert = JsonConvert.DeserializeObject<Converter>(input);
            convert.JsonToSceneObject(ref scene);
        }
    }
}
