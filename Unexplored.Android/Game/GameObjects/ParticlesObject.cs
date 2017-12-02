using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Unexplored.Game.Components;

namespace Unexplored.Game.GameObjects
{
    public class ParticlesObject : GameObject
    {
        public ParticlesObject() : base()
        {
            SetComponents(
                new ParticleRendererComponent()
                );
        }
    }
}
