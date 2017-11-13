using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core
{
    class GameComponentAttribute : Attribute
    {
        public int OrderLevel { get; private set; }

        public GameComponentAttribute(int orderLevel)
        {
            OrderLevel = orderLevel;
        }
    }
}
