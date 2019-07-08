using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Java.Util;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// Handle locale
    /// </summary>
    public class LocaleManager
    {

        /// <summary>
        /// Returns locale used in application
        /// </summary>
        /// <returns>Locale value stored in <see cref="SharedPreferences"/> or system's locale if there's no preferences stored</returns>
        public static String GetLocale()
        {
            // Get locale value from SharedPreferences or use default value
            string locale = ((locale = SharedPreferences.GetLanguage()) != null) ? locale : Locale.Default.Language;
            return locale;
        }

        /// <summary>
        /// Store locale value as prefered language
        /// </summary>
        /// <param name="language">Locale value of prefered language</param>
        public static void SetLocale(string language)
        {
            // Persist language preference using SharedPreferences
            SharedPreferences.SetLanguage(language);
        }

        /// <summary>
        /// Sets a new locale and updates configuration with it
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Context object with updated configuration</returns>
        public static Context UpdateBaseContextLocale(Context context)
        {
            // Get language stored in SharedPreferences or get system's default language
            string language = ((language = SharedPreferences.GetLanguage()) != null) ? language : Locale.Default.Language;

            // Create new locale object using language and set it as application's default locale
            Locale locale = new Locale(language);
            Locale.Default = locale;

            Resources resources = context.Resources;
            Configuration configuration;

            // Check API level
            if ((int)Build.VERSION.SdkInt >= 25)
            {
                // Use context to get configurations and set new locale as configuration locale
                configuration = context.Resources.Configuration;
                configuration.Locale = locale;
                // Return updated context
                return context.CreateConfigurationContext(configuration);
            }
            else
            {
                // Use get configurations and set new locale as configuration locale
                configuration = resources.Configuration;
                configuration.Locale = locale;
                // Update resource configuration and return context
                resources.UpdateConfiguration(configuration, resources.DisplayMetrics);
                return context;
            }
        }
    }
}