using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core;
using Unexplored.Core.Attributes;
using Unexplored.Game.Attributes;

namespace Unexplored.Game
{
    public struct Light
    {
        public Vector2 Position;
        public Vector2 RealPosition;
        [CustomProperty]
        public Color Color;
        [CustomProperty]
        public float Opacity;
    }

    class Ligtings
    {
        private RenderTarget2D target;
        private Effect effect;

        private bool direction = false;
        private double timeout;
        private float scale = 2.5f;

        private Light[] lights;
        private int lightsCount = 0;

        private readonly Vector2 centered;

        public Ligtings()
        {
            lights = new Light[100];
            lights[0] = new Light { Color = Color.White, Opacity = 0.25f };

            centered = new Vector2(StaticResources.LightingMask.Width, StaticResources.LightingMask.Height) / 2;
        }

        public void Initialize()
        {
            target = MainGame.Instance.CreateSmallRenderTarget();
            effect = StaticResources.LightingEffect;

        }

        public void SetFirstLight(Vector2 pos, GameTime gameTime)
        {
            lightsCount = 1;
            lights[0].RealPosition = pos;

            timeout += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeout >= 200)
            {
                timeout = 0;
                direction = !direction;
            }

            float value = (float)gameTime.ElapsedGameTime.TotalSeconds * 0.2f;
            if (direction)
                scale += value;
            else
                scale -= value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Light light)
        {
            lights[lightsCount++] = light;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MainGame.Instance.GraphicsDevice.SetRenderTarget(target);
            MainGame.Instance.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            for (int i = 0; i < lightsCount; ++i)
            {
                spriteBatch.Draw(StaticResources.LightingMask, lights[i].RealPosition * 0.5f, null, lights[i].Color * lights[i].Opacity, 0, centered, scale, SpriteEffects.None, 0);
            }
            spriteBatch.End();

             MainGame.Instance.GraphicsDevice.SetRenderTarget(MainGame.Instance.GameSceneTarget);
            effect.Parameters["MapTexture"].SetValue(target);
            spriteBatch.Begin(effect: effect);
            spriteBatch.Draw(MainGame.Instance.GameSceneTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
