using System;
using Android.App;
using Android.Content;
using Android.Util;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// This class add values and get values from sharedpreferences
    /// </summary>
    static class SharedPreferences
    {
        //Log tag for degub
        private const string tag = "SharedPref";
        //Preference tag for attractions
        private const string lastAttraction = "Attraction";
        //Preference tag for markers
        private const string selectedMarker = "SelectedMarker";
        //Preference tag for language
        private const string language = "Language";
        //Preference tag for overriding power saver and low battery
        private const string ignoreBattery = "ignoreBattery";
        //Preference tag for ignoring popups/notifications
        private const string hideNotifications = "hideNotifications";
        //Preference tag for active trail
        private const string lastTrail = "LastTrail";
        //Preference tag for active theme
        private const string lastTheme = "LastTheme";
        //Preference tag for ID of drawn polyline
        private const string polylineID = "PolylineID";
        //Preference tag for checking if notification has been sent
        private const string notificationSent = "NotificationSent";
        //Preference tag for checking if notification has been sent
        private const string isFirstTimeRan = "IsFirstTimeRan";
        //Preference tag indicating if a dialog is currently open
        private const string dialogOpen = "DialogOpen";
        // Preference tag used to store latitude of the device's location
        private const string latitude = "latitude";
        // Preference tag used to store longitude of the device's location
        private const string longitude = "longitude";
        // Preference tag used to store visibility state of the trail elevation chart
        private const string LineChartVisibilityState = "LineChartVisibilityState";
        // Last active objective
        private const string lastObjective = "LastObjective";

        //This is used to handle that the attraction is not pop up again if user is in marker range
        /// <summary>
        /// Set last visited attraction
        /// </summary>
        /// <param name="attractionID">Value to be saved</param>
        public static void SetLastVisitedAttraction(string attractionID)
        {
            SetStringPreference(lastAttraction, attractionID);
        }

        //This is used to handle that the attraction is not pop up again if user is in marker range
        /// <summary>
        /// Get last visited attraction
        /// </summary>
        /// <returns>Last visited plase</returns>
        public static string GetLastVisitedAttraction()
        {
            return GetStringPreference(lastAttraction);
        }

        /// <summary>
        /// Used to store id of a marker when notification is sent
        /// </summary>
        /// <param name="markerId"></param>
        public static void SetMarkerId(string markerId) {
            SetStringPreference(selectedMarker, markerId);
        }

        public static string GetMarkerId() {
            return GetStringPreference(selectedMarker);
        }

        public static string GetLanguage()
        {
            return GetStringPreference(language);
        }

        public static void SetLanguage(string language)
        {
            SetStringPreference(SharedPreferences.language, language);
        }

        public static void SetIgnoreBatteryState(bool state)
        {
            SetBoolPreference(ignoreBattery, state);
        }

        public static bool GetIgnoreBatteryState()
        {
            return GetBoolPreference(ignoreBattery, false);
        }

        public static void SetHideNotifications(bool state)
        {
            SetBoolPreference(hideNotifications, state);
        }

        public static bool GetHideNotifications()
        {
            return GetBoolPreference(hideNotifications, false);
        }

        public static void SetLatitude(float value)
        {
            SetFloatPreference(latitude, value);
        }

        public static float GetLatitude()
        {
            return GetFloatPreference(latitude);
        }

        public static void SetLongitude(float value)
        {
            SetFloatPreference(longitude, value);
        }


        public static float GetLongitude()
        {
            return GetFloatPreference(longitude);
        }

        public static void SetLineChartVisibilityState(bool state)
        {
            SetBoolPreference(LineChartVisibilityState, state);
        }

        public static bool GetLineChartVisibilityState()
        {
            return GetBoolPreference(LineChartVisibilityState, true);
        }

        /// <summary>
        /// Set active Trail
        /// </summary>
        /// <param name="trailID">Trail ID</param>
        public static void SetLastTrail(string trailID)
        {
            SetStringPreference(lastTrail, trailID);
        }

        /// <summary>
        /// Get last active trail
        /// </summary>
        /// <returns>Trail ID</returns>
        public static string GetLastTrail()
        {
            return GetStringPreference(lastTrail);
        }

        /// <summary>
        /// Set active Theme
        /// </summary>
        /// <param name="themeID">Theme ID</param>
        public static void SetLastTheme(string themeID)
        {
            SetStringPreference(lastTheme, themeID);
        }

        /// <summary>
        /// Get last active theme
        /// </summary>
        /// <returns>Theme ID</returns>
        public static string GetLastTheme()
        {
            return GetStringPreference(lastTheme);
        }

        /// <summary>
        /// Set ID of drawn polyline
        /// </summary>
        /// <param name="ID">ID of Polyline</param>
        public static void SetPolylineID(float ID)
        {
            SetFloatPreference(polylineID, ID);
        }

        /// <summary>
        /// Get ID of last drawn polyline
        /// </summary>
        /// <returns>ID of Polyline</returns>
        public static float GetPolylineID()
        {
            return GetFloatPreference(polylineID);
        }

        /// <summary>
        /// Set preference indicating if the app has been run before
        /// Sets the preference as TRUE, intended to be executed on the first run of the app
        /// </summary>
        public static void SetFirsTimeRan()
        {
            SetBoolPreference(isFirstTimeRan, true);
        }

        /// <summary>
        /// Get preference indicating if the app has been run before
        /// </summary>
        /// <returns>True if the app has been run, false if not</returns>
        public static bool GetFirstTimeRan()
        {
            return GetBoolPreference(isFirstTimeRan, false);
        }

        public static void SetLastObjective(string objectiveID)
        {
            SetStringPreference(lastObjective, objectiveID);
        }

        public static string GetLastObjective()
        {
            return GetStringPreference(lastObjective);
        }

        /// <summary>
        /// Get a stored string value preference
        /// </summary>
        /// <param name="key">Key of preferemce to return</param>
        /// <returns>String value preference to return</returns>
        private static string GetStringPreference(string key)
        {
            //Fetch preferences
            var prefs = Application.Context.GetSharedPreferences(key, FileCreationMode.Private);
            //Retrieve string value from the preferences
            string value = prefs.GetString(key, null);
            return value;
        }

        /// <summary>
        /// Set a string type preference
        /// </summary>
        /// <param name="key">Key for the preference</param>
        /// <param name="value">Value to be stored</param>
        private static void SetStringPreference (string key, string value)
        {
            //Fetch preferences
            var prefs = Application.Context.GetSharedPreferences(key, FileCreationMode.Private);
            //Set preference
            prefs.Edit().PutString(key, value).Commit();
        }

        /// <summary>
        /// Get a stored string type preference
        /// </summary>
        /// <param name="key">Key of preferemce to return</param>
        /// <returns>Float value preference to return</returns>
        private static float GetFloatPreference(string key)
        {
            //Fetch preferences
            var prefs = Application.Context.GetSharedPreferences(key, FileCreationMode.Private);
            //Retrieve string value from the preferences
            float value = prefs.GetFloat(key, 0);
            return value;
        }

        /// <summary>
        /// Set a float type preference
        /// </summary>
        /// <param name="key">Key for the preference</param>
        /// <param name="value">Value to be stored</param>
        private static void SetFloatPreference(string key, float value)
        {
            //Fetch preferences
            var prefs = Application.Context.GetSharedPreferences(key, FileCreationMode.Private);
            //Set preference
            prefs.Edit().PutFloat(key, value).Commit();
        }

        /// <summary>
        /// Get a stored boolean type preference
        /// </summary>
        /// <param name="key">Key for the preference</param>
        private static bool GetBoolPreference(string key, bool defaultValue)
        {
            //Fetch preferences
            var prefs = Application.Context.GetSharedPreferences(key, FileCreationMode.Private);
            //Retrieve string value from the preferences
            bool value = prefs.GetBoolean(key, defaultValue);
            return value;
        }

        /// <summary>
        /// Set a boolean type preference
        /// </summary>
        /// <param name="key">Key for the preference</param>
        /// <param name="value">Value to be stored</param>
        private static void SetBoolPreference(string key, bool value)
        {
            //Fetch preferences
            var prefs = Application.Context.GetSharedPreferences(key, FileCreationMode.Private);
            //Set preference
            prefs.Edit().PutBoolean(key, value).Commit();
        }
    }
}