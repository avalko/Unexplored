using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;

namespace Unexplored.Core.Components
{
    public class Camera2DComponent : BehaviorComponent, IGameCamera
    {
        public Vector2 Offset
        {
            get => offset;
            set
            {
                offset = value;
                skip = false;
                observer.Notify();
            }
        }

        public Vector2 Location
        {
            get => location;
            set
            {
                location = value;
                skip = false;
                observer.Notify();
            }
        }
        public float Scale
        {
            get => scale;
            set
            {
                scale = value;
                skip = false;
                observer.Notify();
            }
        }

        public Rectangle Bounds { get; set; }

        public Vector2 ToWorld(Vector2 position) => Vector2.Transform(position, invertTransforMatrix);
        public Vector2 ToScreen(Vector2 position) => Vector2.Transform(position, transforMatrix);

        public Matrix TransformMatrix => transforMatrix;
        public Matrix ScaleMatrix => scaleMatrix;

        private Matrix transforMatrix, invertTransforMatrix, scaleMatrix;
        private Vector2 location = Vector2.Zero;
        private Vector2 offset = Vector2.Zero;
        private float scale = 1.0f;
        private bool skip = false;
        private Observer observer;

        public void SetViewport(Viewport viewport)
        {
            Bounds = viewport.Bounds;
            observer.Notify("Viewport");
        }

        public Camera2DComponent()
        {
            observer = new Observer("Camera2D");
        }

        public bool InBounds(Vector2 point, float limit = 0)
        {
            if (point.X >= -limit && point.Y >= -limit &&
                point.X <= Bounds.Width + limit && point.Y <= Bounds.Height + limit)
                return true;
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            if (skip)
                return;

            transforMatrix = Matrix.CreateTranslation(new Vector3(Location, 0))
            * (scaleMatrix = Matrix.CreateScale(Scale))
            * Matrix.CreateTranslation(new Vector3(offset, 0));
            invertTransforMatrix = Matrix.Invert(transforMatrix);

            observer.Notify("Update");
            skip = true;
        }
    }
}
