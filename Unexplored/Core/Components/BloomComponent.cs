using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Unexplored.Core.Components.Logic;

namespace Unexplored.Core.Components
{
    public class BloomSettings
    {
        #region Fields


        // Name of a preset bloom setting, for display to the user.
        public readonly string Name;


        // Controls how bright a pixel needs to be before it will bloom.
        // Zero makes everything bloom equally, while higher values select
        // only brighter colors. Somewhere between 0.25 and 0.5 is good.
        public readonly float BloomThreshold;


        // Controls how much blurring is applied to the bloom image.
        // The typical range is from 1 up to 10 or so.
        public readonly float BlurAmount;


        // Controls the amount of the bloom and base images that
        // will be mixed into the final scene. Range 0 to 1.
        public readonly float BloomIntensity;
        public readonly float BaseIntensity;


        // Independently control the color saturation of the bloom and
        // base images. Zero is totally desaturated, 1.0 leaves saturation
        // unchanged, while higher values increase the saturation level.
        public readonly float BloomSaturation;
        public readonly float BaseSaturation;


        #endregion


        /// <summary>
        /// Constructs a new bloom settings descriptor.
        /// </summary>
        public BloomSettings(string name, float bloomThreshold, float blurAmount,
                             float bloomIntensity, float baseIntensity,
                             float bloomSaturation, float baseSaturation)
        {
            Name = name;
            BloomThreshold = bloomThreshold;
            BlurAmount = blurAmount;
            BloomIntensity = bloomIntensity;
            BaseIntensity = baseIntensity;
            BloomSaturation = bloomSaturation;
            BaseSaturation = baseSaturation;
        }


        /// <summary>
        /// Table of preset bloom settings, used by the sample program.
        /// </summary>
        public static BloomSettings[] PresetSettings =
        {
			//                Name           Thresh  Blur Bloom  Base  BloomSat BaseSat
			new BloomSettings("Default",     0.25f,  4,   1.25f, 1,    1,       1),
            new BloomSettings("Soft",        0,      3,   1,     1,    1,       1),
            new BloomSettings("Desaturated", 0.5f,   8,   2,     1,    0,       1),
            new BloomSettings("Saturated",   0.25f,  4,   2,     1,    2,       0),
            new BloomSettings("Blurry",      0,      2,   1,     0.1f, 1,       1),
            new BloomSettings("Subtle",      0.5f,   2,   1,     1,    1,       1),
        };
    }
    
    [GameComponent(0)]
    class BloomComponent : CoreComponent
    {
        Effect bloomExtractEffect;
        Effect bloomCombineEffect;
        Effect gaussianBlurEffect;

        RenderTarget2D sceneRenderTarget;
        RenderTarget2D renderTarget1;
        RenderTarget2D renderTarget2;

        public RenderTarget2D ResultTarget { get; set; } = null;

        public BloomSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        BloomSettings settings = BloomSettings.PresetSettings[0];

        public enum IntermediateBuffer
        {
            PreBloom,
            BlurredHorizontally,
            BlurredBothWays,
            FinalResult,
        }

        public IntermediateBuffer ShowBuffer
        {
            get { return showBuffer; }
            set { showBuffer = value; }
        }

        IntermediateBuffer showBuffer = IntermediateBuffer.FinalResult;

        public BloomComponent(MainGame game)
            : base(game, false)
        {
            Enabled = false;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bloomExtractEffect = content.Load<Effect>("effects/bloom/BloomExtract");
            bloomCombineEffect = content.Load<Effect>("effects/bloom/BloomCombine");
            gaussianBlurEffect = content.Load<Effect>("effects/GaussianBlur");
            
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            //int width = MainGame.WIDTH; int height = MainGame.HEIGHT;
            int width = 1366; int height = 768;

            SurfaceFormat format = pp.BackBufferFormat;
            
            sceneRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false,
                                                   format, pp.DepthStencilFormat, pp.MultiSampleCount,
                                                   RenderTargetUsage.DiscardContents);

            width /= 2;
            height /= 2;

            renderTarget1 = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None);
            renderTarget2 = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None);
        }

        public void BeginDraw()
        {
            GraphicsDevice.SetRenderTarget(sceneRenderTarget);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;

            bloomExtractEffect.Parameters["BloomThreshold"].SetValue(
                Settings.BloomThreshold);

            DrawFullscreenQuad(sceneRenderTarget, renderTarget1,
                               bloomExtractEffect,
                               IntermediateBuffer.PreBloom);

            SetBlurEffectParameters(1.0f / (float)renderTarget1.Width, 0);

            DrawFullscreenQuad(renderTarget1, renderTarget2,
                               gaussianBlurEffect,
                               IntermediateBuffer.BlurredHorizontally);

            SetBlurEffectParameters(0, 1.0f / (float)renderTarget1.Height);

            DrawFullscreenQuad(renderTarget2, renderTarget1,
                               gaussianBlurEffect,
                               IntermediateBuffer.BlurredBothWays);

            GraphicsDevice.SetRenderTarget(ResultTarget);

            EffectParameterCollection parameters = bloomCombineEffect.Parameters;

            parameters["BloomIntensity"].SetValue(Settings.BloomIntensity);
            parameters["BaseIntensity"].SetValue(Settings.BaseIntensity);
            parameters["BloomSaturation"].SetValue(Settings.BloomSaturation);
            parameters["BaseSaturation"].SetValue(Settings.BaseSaturation);

            GraphicsDevice.Textures[1] = sceneRenderTarget;

            Viewport viewport = GraphicsDevice.Viewport;

            DrawFullscreenQuad(renderTarget1,
                               viewport.Width, viewport.Height,
                               bloomCombineEffect,
                               IntermediateBuffer.FinalResult);
        }

        void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget,
                                Effect effect, IntermediateBuffer currentBuffer)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            DrawFullscreenQuad(texture,
                               renderTarget.Width, renderTarget.Height,
                               effect, currentBuffer);
        }

        void DrawFullscreenQuad(Texture2D texture, int width, int height,
                                Effect effect, IntermediateBuffer currentBuffer)
        {
            if (showBuffer < currentBuffer)
            {
                effect = null;
            }

            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.End();
        }

        void SetBlurEffectParameters(float dx, float dy)
        {
            EffectParameter weightsParameter, offsetsParameter;

            weightsParameter = gaussianBlurEffect.Parameters["SampleWeights"];
            offsetsParameter = gaussianBlurEffect.Parameters["SampleOffsets"];

            int sampleCount = weightsParameter.Elements.Count;

            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            float totalWeights = sampleWeights[0];

            for (int i = 0; i < sampleCount / 2; i++)
            {
                float weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            weightsParameter.SetValue(sampleWeights);
            offsetsParameter.SetValue(sampleOffsets);
        }

        float ComputeGaussian(float n)
        {
            float theta = Settings.BlurAmount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
