using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core.Components.Logic
{
    public interface ICoreComponent
    {
        bool Enabled { get; set; }
        bool DrawEnabled { get; set; }

        void Initialize();
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
