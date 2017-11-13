using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Unexplored.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Game.Attributes;

namespace Unexplored.Game
{
    public static class StaticResources
    {
        [GameResource("map/tileset")]
        public static Texture2D Tileset;

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
