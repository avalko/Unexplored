using Microsoft.Xna.Framework;
using Unexplored.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Game.Structures;

namespace Unexplored
{
    public static class Constants
    {
        public const double FPS = 120;
        public const double FrameRate = 1 / FPS;
        public const double FrameScale = 120 / FPS;

        public const float ScaleFactor = SceneWidth / (1280 / 4);
        public const float ScaledTile = Tile.Size * ScaleFactor;

        public const int SceneWidth = 1366;
        public const int SceneHeight = 768;
        public static Vector2 SceneSize = new Vector2(SceneWidth, SceneHeight);
        public static Vector2 ScaledSceneSize { get; } = SceneSize * ScaleFactor;
        public static Vector2 ScaledTileSize { get; }  = new Vector2(ScaledTile);
        public static Color BackgroundColor = Color.White;
        public static Color ForegroundColor = Color.White;

        private static int width, height;
        private static Vector2 size;

        public static int WindowWidth => width;
        public static int WindowHeight => height;
        public static Vector2 Size => size;

        public static event Action OnSizeChanged;

        public static void SetWidth(int width)
        {
            Constants.width = width;
            size.X = width;
            OnSizeChanged?.Invoke();
        }

        public static void SetHeight(int height)
        {
            Constants.height = height;
            size.Y = height;
            OnSizeChanged?.Invoke();
        }

        public static void SetSize(int width, int height)
        {
            Constants.width = width;
            Constants.height = height;
            size = new Vector2(width, height);
            OnSizeChanged?.Invoke();
        }
    }
}
