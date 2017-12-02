using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Structures
{
    public class WarpPoint
    {
        public readonly int MaxState;
        public int State;
        public Vector2 Position;
        public bool Avaliable => State < MaxState;

        private Action onNotify;

        public WarpPoint(int max, Action onNotify = null)
        {
            MaxState = max;
            this.onNotify = onNotify;
        }

        public void Notify() => onNotify?.Invoke();            
    }
}
