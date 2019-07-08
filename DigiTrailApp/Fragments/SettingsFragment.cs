using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using DigiTrailApp.Helpers;
using Java.Lang;
using System;
using System.Linq;

namespace DigiTrailApp.Fragments
{
    /// <summary>
    /// Settings (under development #life)
    /// </summary>
    class SettingsFragment : PreferenceFragment, ISharedPreferencesOnSharedPreferenceChangeListener
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AddPreferencesFromResource(Resource.Xml.preferences);

            // When creating settings fragment for the first time, check if device's locale matches any of the locales available and set it as default language if true.
            // If no match was found, then set English as default language.
            ListPreference listPreference = (ListPreference)FindPreference(Constants.PrefLanguage);
            if (listPreference.Value == null)
            {
                var languages = Resources.GetStringArray(Resource.Array.langValues);

                if (languages.Contains(LocaleManager.GetLocale()))
                {
                    listPreference.SetValueIndex(Array.IndexOf(languages, LocaleManager.GetLocale()));
                }
                else
                {
                    listPreference.SetValueIndex(Array.IndexOf(languages, Constants.DefaultLang));
                }

            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = (ListView)inflater.Inflate(Resource.Layout.SettingsLayout, container, false);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }

        public override void OnResume()
        {
            base.OnResume();

            PreferenceManager.SharedPreferences.RegisterOnSharedPreferenceChangeListener(this);
        }

        public override void OnPause()
        {
            base.OnPause();

            PreferenceManager.SharedPreferences.UnregisterOnSharedPreferenceChangeListener(this);
        }

        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            var preferenceKey = key;
            var preference = sharedPreferences;

            switch (key)
            {
                case Constants.PrefLanguage:
                    //    string language = sharedPreferences.GetString(key, Java.Util.Locale.Default.ToString());
                    //    try
                    //    {
                    //        LocaleManager.SetLocale(language);
                    //    }
                    //    catch (ArgumentNullException e)
                    //    {
                    //        ErrorBroadcaster.BroadcastError(Activity, e);
                    //    }

                    //    ////Recreate();

                    //    Intent mStartActivity = new Intent(Application.Context, typeof(LauncherActivity));

                    //    int mPendingIntentId = 123456;
                    //    PendingIntent mPendingIntent = PendingIntent.GetActivity(Application.Context, mPendingIntentId, mStartActivity, PendingIntentFlags.CancelCurrent);
                    //    AlarmManager mgr = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
                    //    mgr.Set(AlarmType.Rtc, JavaSystem.CurrentTimeMillis() + 100, mPendingIntent);
                    //    ((MainActivity)Context).Finish();


                    break;

                case Constants.IgnoreBattery:
                    SharedPreferences.SetIgnoreBatteryState(sharedPreferences.GetBoolean(key, false));
                    break;

                case Constants.HideNotifications:
                    SharedPreferences.SetHideNotifications(sharedPreferences.GetBoolean(key, false));
                    break;

                default:
                    Toast.MakeText(Application.Context, GetString(Resource.String.errorFeatureNotInUse), ToastLength.Short).Show();
                    break;
            }
        }
    }
}