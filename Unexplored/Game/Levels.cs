using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game
{
    public static class Levels
    {
        public static Map Level1;

        public static void Load()
        {
            Level1 = MapReader.ParseMap(File.ReadAllText("Content/map/map.json"));
        }
    }
}
