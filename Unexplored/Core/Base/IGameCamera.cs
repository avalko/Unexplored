using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core.Base
{
    public interface IGameCamera : IBehaviorComponent
    {
        Matrix TransformMatrix { get; }
        Matrix ScaleMatrix { get; }
        Vector2 Offset { get; set; }
        Vector2 Location { get; set; }
        float Scale { get; set; }
        Vector2 ToWorld(Vector2 position);
        Vector2 ToScreen(Vector2 position);
        void SetViewport(Viewport viewport);
        bool InBounds(Vector2 point, float limit = 0);
        void Update(GameTime gameTime);
    }
}
