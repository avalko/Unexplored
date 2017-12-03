using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Android.Core.Types;
using Unexplored.Core.Attributes;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Game.Attributes;
using Unexplored.Game.Structures;

namespace Unexplored.Game.Components
{
    class TextRendererComponent : BehaviorComponent
    {
        public string Text;
        public Color Color;
        [CustomProperty]
        public float Opacity;

        private bool opacityUp;
        private bool changeOpacity;

        private ObjectStateComponent parentState;

        public TextRendererComponent(MapText text)
        {
            Opacity = 1;
            Text = text.Text;
            Color = text.Color;
        }

        private string WrapText(string text)
        {
            if (StaticResources.FontText.MeasureString(text).X < Transform.Size.X * Constants.ScaleFactor)
            {
                return text;
            }

            string[] words = text.Split(' ');
            StringBuilder wrappedText = new StringBuilder();
            float linewidth = 0f;
            float spaceWidth = StaticResources.FontText.MeasureString(" ").X;
            for (int i = 0; i < words.Length; ++i)
            {
                Vector2 size = StaticResources.FontText.MeasureString(words[i]);
                if (linewidth + size.X < Transform.Size.X * Constants.ScaleFactor)
                {
                    linewidth += size.X + spaceWidth;
                }
                else
                {
                    wrappedText.Append("\n");
                    linewidth = size.X + spaceWidth;
                }
                wrappedText.Append(words[i]);
                wrappedText.Append(" ");
            }

            return wrappedText.ToString();
        }

        public override void Initialize()
        {
            if (Opacity == 0)
                opacityUp = false;
            else
                opacityUp = true;
            Text = WrapText(Text);
        }

        public override void Update(GameTime gameTime)
        {
            if (parentState != null && !changeOpacity)
            {
                if (Opacity == 0 && parentState.State)
                {
                    changeOpacity = true;
                    opacityUp = true;
                }
                else if (Opacity == 1 && !parentState.State)
                {
                    changeOpacity = true;
                    opacityUp = false;
                }
            }

            if (changeOpacity)
            {
                float to = 0;
                if (opacityUp)
                    to = 1;
                if (Math.Abs(Opacity - to) < 0.01f)
                {
                    changeOpacity = false;
                    Opacity = to;
                }
                else
                    Opacity = MathHelper.LerpPrecise(Opacity, to, (float)gameTime.ElapsedGameTime.TotalSeconds * (opacityUp ? 2.5f : 10f));
            }
        }

        public override void OnEventBegin(GameEvent gameEvent)
        {
            if (gameEvent.GameObject.GetComponent<ObjectStateComponent>() is ObjectStateComponent state)
            {
                parentState = state;
                return;
            }

            if (gameEvent.Type == "opacity")
            {
                //Opacity = 1;
                opacityUp = true;
                changeOpacity = true;
            }
        }

        public override void OnEventEnd(GameEvent gameEvent)
        {
            if (parentState != null)
                return;

            if (gameEvent.Type == "opacity")
            {
                //Opacity = 0;
                opacityUp = false;
                changeOpacity = true;
            }
        }

        public override void Draw()
        {
            spriteBatch.DrawString(StaticResources.FontText, Text, Transform.Position * Constants.ScaleFactor, Color * Opacity);
        }
    }
}
