using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core.Base
{
    public interface IGameScene
    {
        IGameCamera GetCamera();
        void Initialize(SpriteBatch spriteBatch);
        void LoadContent(ContentManager content);
        void Update(GameTime gameTime);
        void Draw();
    }
}
