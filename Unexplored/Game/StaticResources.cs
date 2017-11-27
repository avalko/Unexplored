using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Unexplored.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Game.Attributes;
using Microsoft.Xna.Framework.Media;

namespace Unexplored.Game
{
    public static class StaticResources
    {
        [GameResource("images/grey")]
        public static Texture2D MenuBackground;
        [GameResource("images/grey_pressed")]
        public static Texture2D MenuItem;

        [GameResource("map/tileset")]
        public static Texture2D Tileset;
        [GameResource("fonts/baseFont")]
        public static SpriteFont FontBase;
        [GameResource("fonts/textFont")]
        public static SpriteFont FontText;
        [GameResource("fonts/uiFont")]
        public static SpriteFont FontUI;

        [GameResource("shaders/lighting")]
        public static Effect LightingEffect;
        [GameResource("shaders/lightmask")]
        public static Texture2D LightingMask;

        [GameResource("sounds/theme1")]
        public static Song SoundTheme1;
        [GameResource("sounds/theme2")]
        public static Song SoundTheme2;
        [GameResource("sounds/theme3")]
        public static Song SoundTheme3;

        public static void LoadContent(ContentManager Content)
        {
            var load = Content.GetType().GetMethod("Load");

            var fields = typeof(StaticResources).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            Reflection.ForEachFieldsWithAttribute<GameResourceAttribute>(fields, (field, attribute) =>
            {
                field.SetValue(null, load.MakeGenericMethod(field.FieldType).Invoke(Content, new[] { attribute.Path }));
            });
        }
    }
}
