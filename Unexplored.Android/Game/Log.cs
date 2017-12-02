using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game
{
    public static class Log
    {
        public static void Assert(bool condition, string message, params object[] args)
        {
            if (!condition)
            {
                string format = string.Format(message, args);
            #if !DEBUG
                File.AppendAllText("exceptions.log", format);
            #endif
                throw new Exception(format);
            }
        }
    }
}
