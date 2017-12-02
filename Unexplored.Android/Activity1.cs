using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
//using System;
using System.IO;

namespace Unexplored.Android
{
    [Activity(Label = "Unexplored.Android"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        public static AssetManager AssetManager { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = Path.Combine(path, "myfile.txt");
            File.AppendAllText(filename, "123");

            System.AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                try
                {
                    File.AppendAllText(filename, $"{new string('_', 50)}\r\n[{System.DateTime.Now}] - {e.ExceptionObject}\r\n\r\n");
                }
                catch
                {
                }
            };

            var g = new MainGame();

            AssetManager = this.Assets;

            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

