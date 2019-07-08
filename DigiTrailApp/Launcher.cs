using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using DigiTrailApp.Helpers;

namespace DigiTrailApp
{
    /// <summary>
    /// First Activity that starts when App is opened. Starts MainActivity <seealso cref="MainActivity"/> if App is running else start SplashScreen <seealso cref="SplashScreen"/>
    /// </summary>
    [Activity(Label = "DigiTrail", MainLauncher = true, LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/DigiTrailTheme", NoHistory = true)]
    public class Launcher : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SplasLayout);

            if (StaticStorage.IsApplicationRunning)
            {
                //Start the MainActivity
                StartActivity(new Intent(this, typeof(MainActivity)));
            }
            else
            {
                //Start the MainActivity
                StartActivity(new Intent(this, typeof(SplashScreen)));

                StaticStorage.IsApplicationRunning = true;
            }

            // Finish this Activity
            Finish();
        }
    }
}