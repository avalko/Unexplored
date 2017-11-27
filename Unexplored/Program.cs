using Unexplored.Core;
using System;
using System.Diagnostics;
using System.IO;

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
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                try
                {
                    File.AppendAllText("errors.log", $"{new string('_', 50)}\r\n[{DateTime.Now}] - {e.ExceptionObject}\r\n\r\n");
                }
                catch
                {
                }
            };

            using (var game = new MainGame())
                game.Run();
        }
    }
}
