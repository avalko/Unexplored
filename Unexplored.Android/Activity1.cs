using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
//using System;
using System.IO;

namespace Unexplored.Android
{
    [Activity(Label = "Unexplored"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        public static AssetManager AssetManager { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.Window.ClearFlags(WindowManagerFlags.Fullscreen);
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.Fullscreen | SystemUiFlags.HideNavigation | SystemUiFlags.LayoutFullscreen | SystemUiFlags.ImmersiveSticky);

            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = Path.Combine(path, "errors.txt");

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

