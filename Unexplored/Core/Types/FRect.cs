using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core.Types
{
    public struct FRect
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public Vector2 Location => new Vector2(X, Y);
        public Vector2 Size => new Vector2(Width, Height);

        public FRect(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
