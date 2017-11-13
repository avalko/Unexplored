using Unexplored.Core;
using System;
using System.Diagnostics;

namespace Unexplored
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*for (int i = 0; i < 100; i++)
            {
                Example1();
            }*/

            using (var game = new MainGame())
                game.Run();
        }

        const int WIDTH = 1000;
        const int HEIGHT = 1000;
        const int TileSize = 10;
        const int CameraSpeed = 10;
        const int CameraSize = 100;

        static void Example1()
        {
            Random random = new Random();
            float[,] array = new float[HEIGHT, WIDTH];
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    array[y, x] = 1;
                }
            }

            Stopwatch sw = Stopwatch.StartNew();

            int cameraX = 0;
            int cameraY = 0;
            double result = 0;

            const int max = ((WIDTH * HEIGHT * TileSize) / (CameraSize * CameraSize));
            for (int cameraIndex = 0; cameraIndex < max; cameraIndex++)
            {
                cameraX = (cameraIndex * CameraSize) % WIDTH;
                cameraY = (cameraIndex * CameraSize) / WIDTH;

                int startX = cameraX / TileSize;
                int startY = cameraY / TileSize;
                int endX = (int)Math.Ceiling((cameraX + CameraSize) / (double)TileSize);
                int endY = (int)Math.Ceiling((cameraY + CameraSize) / (double)TileSize);

                if (startX < 0) startX = 0;
                if (startY < 0) startY = 0;
                if (endX > WIDTH) endX = WIDTH;
                if (endY > HEIGHT) endY = HEIGHT;

                for (int y = startY; y < endY; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        result += array[y, x];
                    }
                }
            }

            Debug.WriteLine($"Add Time: {sw.ElapsedMilliseconds}ms");
        }
    }
}
