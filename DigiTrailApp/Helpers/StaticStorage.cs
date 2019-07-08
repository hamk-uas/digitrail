using Android.Graphics;
using Android.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// Used to store variables needed for the current runtime
    /// </summary>
    static class StaticStorage
    {
        /// <summary>
        /// Indicates if the user has seen the network warning dialog
        /// True indicates the dialog has been already sent, and the user shouldn't need to see it again
        /// </summary>
        public static bool IsNetworkDialogShown { get; set; }

        /// <summary>
        /// Indicates if the user has seen the GPS warning dialog
        /// True indicates the dialog has been already sent, and the user shouldn't need to see it again
        /// </summary>
        public static bool IsGpsDialogShown { get; set; }

        /// <summary>
        /// Indicates if the user has seen the help dialog
        /// True indicates the dialog has been already sent, and the user shouldn't need to see it again
        /// </summary>
        public static bool IsHelpDialogShown { get; set; }

        /// <summary>
        /// Indicates if the user has selected a trail and it has been drawn
        /// True indicates that a trail has been selected
        /// </summary>
        public static bool IsTrailActive { get; set; }

        /// <summary>
        /// Indicates if the user has selected a theme
        /// True indicates that a theme has been selected
        /// </summary>
        public static bool IsThemeActive { get; set; }

        /// <summary>
        /// Indicates if a theme with an objective has been selected
        /// True indicates that an objective has been selected
        /// </summary>
        public static bool IsObjectiveActive { get; set; }

        /// <summary>
        /// Indicates if foreground service is running
        /// True indicates that service is running
        /// </summary>
        public static bool IsServiceRunning { get; set; }

        /// <summary>
        /// Indicates if the user has seen the compass dialog
        /// True indicates the dialog has been already sent, and the user shouldn't need to see it again
        /// </summary>
        public static bool IsCompassDialogShown { get; set; }

        /// <summary>
        /// Indicates if application is already running
        /// True indicates that application is already running
        /// </summary>
        public static bool IsApplicationRunning { get; set; }

        /// <summary>
        /// Sets all booleans values in StaticStorage to false
        /// </summary>
        public static void ResetStorage()
        {
            // Using Linq, get all boolean properties in this class
            var booleans = typeof(StaticStorage).GetProperties().Where(p => p.PropertyType == typeof(bool));

            foreach (var boolean in booleans)
            {
                // Set value of boolean to false
                boolean.SetValue(boolean, false);
            }
        }
    }
}