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

    class Lightings
    {
#if ANDROID
        private RenderTarget2D target2;
#endif
        private RenderTarget2D target;
        private Effect effect;

        private bool direction = false;
        private double timeout;
        private float scale = 2.5f;

        private Light[] lights;
        private int lightsCount = 0;

        private readonly Vector2 centered;
        private SceneManager sceneManager;

        public Lightings(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;

            lights = new Light[100];
            lights[0] = new Light { Color = Color.White, Opacity = 0.25f };

            centered = new Vector2(StaticResources.LightingMask.Width, StaticResources.LightingMask.Height) / 2;
        }

        public void Initialize()
        {
            target = sceneManager.Game.CreateSmallRenderTarget();
#if ANDROID
            target2 = sceneManager.Game.CreateRenderTarget();
#endif
            effect = StaticResources.LightingEffect;
            lightsCount = 1;
        }

        public void SetFirstLight(Vector2 pos, GameTime gameTime)
        {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            lightsCount = 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
#if ANDROID
            sceneManager.Game.GraphicsDevice.SetRenderTarget(target);
            sceneManager.Game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            for (int i = 0; i < lightsCount; ++i)
            {
                spriteBatch.Draw(StaticResources.LightingMask, lights[i].RealPosition * 0.5f, null, lights[i].Color * lights[i].Opacity, 0, centered, scale, SpriteEffects.None, 0);
            }
            spriteBatch.End();

            sceneManager.Game.GraphicsDevice.SetRenderTarget(target2);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            effect.Parameters["MapTexture"].SetValue(target);
            effect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(sceneManager.Game.GameSceneTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            sceneManager.Game.GraphicsDevice.SetRenderTarget(sceneManager.Game.GameSceneTarget);
            spriteBatch.Begin();
            spriteBatch.Draw(target2, Vector2.Zero, Color.White);
            spriteBatch.End();
#else
            sceneManager.Game.GraphicsDevice.SetRenderTarget(target);
            sceneManager.Game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            for (int i = 0; i < lightsCount; ++i)
            {
                spriteBatch.Draw(StaticResources.LightingMask, lights[i].RealPosition * 0.5f, null, lights[i].Color * lights[i].Opacity, 0, centered, scale, SpriteEffects.None, 0);
            }
            spriteBatch.End();

            sceneManager.Game.GraphicsDevice.SetRenderTarget(sceneManager.Game.GameSceneTarget);
            effect.Parameters["MapTexture"].SetValue(target);
            spriteBatch.Begin(effect: effect);
            spriteBatch.Draw(sceneManager.Game.GameSceneTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
#endif
        }
    }
}
