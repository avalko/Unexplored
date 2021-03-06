﻿using System;
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
#if ANDROID
            using (StreamReader sr = new StreamReader(Android.Activity1.AssetManager.Open("map.json")))
            {
                Level1 = MapReader.ParseMap(sr.ReadToEnd());
            }
#else
            Level1 = MapReader.ParseMap(File.ReadAllText("Content/map/map.json"));
#endif
        }
    }
}
