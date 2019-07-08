namespace DigiTrailApp
{
    /// <summary>
    /// Class for storing constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// String for the name of Database used by Azure
        /// </summary>
        public const string Database = "OfflineStore.db";

        /// <summary>
        /// URL for Azure Backend
        /// </summary>
        public const string BackendURL = "YOUR BACKEND URL HERE";

        /// <summary>
        /// String for start service action
        /// </summary>
        public const string ServiceStartAction = "SERVICE_START";

        /// <summary>
        /// String for stop service action
        /// </summary>
        public const string ServiceStopAction = "SERVICE_STOP";

        /// <summary>
        /// String for location provider status changed action
        /// </summary>
        public const string ProviderStatusChanged = "PROVIDER_STATUS_CHANGED";

        /// <summary>
        /// String for location update action
        /// </summary>
        public const string LocationUpdate = "LOCATION_UPDATE";

        /// <summary>
        /// String for action broadcasted from the UserInRangeService
        /// </summary>
        public const string ServiceNewDialogAction = "START_NEW_DIALOG";

        /// <summary>
        /// String for action sent in an intent by UserInRangeService
        /// </summary>
        public const string BroadcastIntentAction = "BROADCAST_INTENT_ACTION";

        // Constants for dialogs
        public const string InternetDialogAction = "INTERNET_DIALOG";
        public const string GPSDialogAction = "GPS_DIALOG_ACTION";
        public const string MarkerAction = "MARKER_ACTION";
        public const string MarkerId = "MARKER_ID";
        public const string ExitAppDialog = "EXIT_DIALOG";
        public const string FeedbackDialog = "FEEDBACK_DIALOG";
        public const string ErrorDIalog = "ERROR_DIALOG";
        public const string AlertDialog = "ALERT_DIALOG";
        public const string GpsDialog = "GPS_DIALOG";
        public const string NetworkDialog = "NETWORK_DIALOG";
        public const string FinishTrailDialog = "FINISH_TRAIL_DIALOG";
        public const string CompassDialog = "COMPASS_DIALOG";
        public const string VersionDialog = "VERSION_DIALOG";

        // Constants for Fragments
        public const string AlertDialogFragment = "AlertDialogFragment";
        public const string PopUpFragment = "PopUpFragment";
        public const string MarkerFragment = "MarkerFragment";
        public const string LocationsFragment = "LocationsFragment";
        public const string TrailsFragment = "TrailsFragment";
        public const string PopupFragment = "PopupFragment";
        public const string ObjectiveThemeMarkerFragment = "ObjectiveThemeMarkerFragment";

        //Constants for settings
        public const string IgnoreBattery = "ignoreBattery";
        public const string HideNotifications = "hideNotifications";
        public const string PopUpAdapter = "PopupListAdapter";
        public const string PopUpRecycleFragment = "PopUpRecycleFragment";
        public const string RecyclerPopUpAdapter = "RecyclerPopUpAdapter";

        // Constants for bundles used in OnSaveInstanceState() call
        public const string SETTINGS_FRAGMENT_OPEN = "SETTINGS_FRAGMENT_OPEN";
        public const string TRAIL_ACTIVE = "TRAIL_ACTIVE";
        public const string THEME_ACTIVE = "THEME_ACTIVE";
        public const string OBJECTIVE_ACTIVE = "OBJECTIVE_ACTIVE";

        /// <summary>
        /// String used to indicate CustomDialogFragment in transactions and logs
        /// </summary>
        public const string CustomDialogTag = "CustomDialogTag";

        /// <summary>
        /// String for storing default UI language
        /// </summary>
        public const string DefaultLang = "fi";

        /// <summary>
        /// Array containing all MapBoxMap layers
        /// </summary>
        public static readonly string[] MapBoxMapLayers = { "RoutesEasy", "RoutesMedium", "RoutesHard", "harkatieRoute" };

        /// <summary>
        /// Unique request code for requesting location permission
        /// </summary>
        public const int LocationRequestCode = 0;

        /// <summary>
        /// Constant for how much 1km is in degrees of latitude
        /// </summary>
        public const double LatInKm = 0.0089;

        // Quick fixes for preferences, used in settings related calles
        public const string PrefLanguage = "language";

        /// <summary>
        /// Used for log messages
        /// </summary>
        public const string LogTag = "DigiTrailApp";

        /// <summary>
        /// Used to direct user to Google Play Store to download the app
        /// </summary>
        public const string AppURL = "https://play.google.com/store/apps/details?id=fi.hamk.digitrailapp";

        /// <summary>
        /// URL for users to send free form feedback about the app
        /// </summary>
        public const string FeedbackURL = "YourFeedBackURLhere";
        /// <summary>
        /// URL to digitrail.fi events
        /// </summary>
        public const string EventsUrl = "YourEventsUrlHere";

        /// <summary>
        /// Url to digitrail.fi activities
        /// </summary>
        public const string ActivitiesUrl = "YourActivitiesUrlHere";

        /// <summary>
        /// URL for opening the End User License Agreement
        /// </summary>
        public const string EULAURL = "YOUR EULA URL HERE";
    }
}