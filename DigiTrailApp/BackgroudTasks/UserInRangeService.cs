using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using DigiTrailApp.Helpers;
using DigiTrailApp.Interfaces;
using System;

namespace DigiTrailApp.BackgroudTasks
{
    /// <summary>
    /// UserInRangeService is Background service for checking user location and show notification if user in marker impact range
    /// </summary>
    [Service]
    public class UserInRangeService : Service, ILocationListener, IBroadcastListener
    {
        #region Class variables
        // Unique integer value to the application.
        private const int SERVICE_ID = 10000;
        // Time limit to location accuracy check
        private const int TWO_MINUTES = 1000 * 60 * 2;

        private Location currentBestLocation;
        private LocationManager locationManager;
        private DialogBroadcastReceiver dialogBroadcastReceiver;
        #endregion

        #region Override methods
        // Required OnBind method
        public override IBinder OnBind(Intent intent)
        {
            // No need to bind this service so return value is null
            return null;
        }

        /// <summary>
        ///  Called when StartService() is called
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="flags"></param>
        /// <param name="startId"></param>
        /// <returns></returns>
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            dialogBroadcastReceiver = new DialogBroadcastReceiver(this);

            // Setup intent filter and register broadcast receiver
            IntentFilter dialogIntentFilter = new IntentFilter();
            dialogIntentFilter.AddAction(Constants.ServiceStopAction);
            RegisterReceiver(dialogBroadcastReceiver, dialogIntentFilter);

            // Acquire a reference to the system Location Manager
            locationManager = (LocationManager)Application.Context.GetSystemService(LocationService);

            // Register listeners with the locationManager to receive location updates
            if (locationManager.AllProviders.Contains(LocationManager.GpsProvider))
            {
                locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 4500, 0, this);

            }
            if (locationManager.AllProviders.Contains(LocationManager.NetworkProvider))
            {
                locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 2500, 0, this);

            }
            // Get notification for the service by calling NotificationProvider.CreateForegroundNotification()
            var notification = NotificationProvider.CreateForegroundNotification(GetString(Resource.String.notificationLocationTrackingTitle), GetString(Resource.String.notificationLocationTrackingContent),false);            
            // Enlist this instance of the service as a foreground service
            StartForeground(SERVICE_ID, notification);
            StaticStorage.IsServiceRunning = true;
            // Return sticky to keep service running until StopService() or StopSelf() is called
            return StartCommandResult.StickyCompatibility;
        }

        // Called when StopService() or StopSelf() is called
        public override void OnDestroy()
        {
            UnregisterReceiver(dialogBroadcastReceiver);
            // Remove updates when service gets destroyed
            locationManager.RemoveUpdates(this);

            base.OnDestroy();
        }
        #endregion

        #region Location accuracy checks
        /// <summary>
        /// Determines whether one Location reading is better than the current Location fix
        /// </summary>
        /// <param name="location">The new Location that you want to evaluate</param>
        /// <param name="currentBestLocation">The current Location fix, to which you want to compare the new one</param>
        /// <returns></returns>
        private bool IsBetterLocation(Location location, Location currentBestLocation)
        {
            if (currentBestLocation == null)
            {
                // A new location is always better than no location
                return true;
            }

            // Check whether the new location fix is newer or older
            long timeDelta = location.ElapsedRealtimeNanos - currentBestLocation.ElapsedRealtimeNanos;
            bool isSignificantlyNewer = timeDelta > TWO_MINUTES;
            bool isSignificantlyOlder = timeDelta < -TWO_MINUTES;
            bool isNewer = timeDelta > 0;
            
            // If it's been more than two minutes since the current location, use the new location, because the user has likely moved
            if (isSignificantlyNewer)
            {
                return true;
            }
            // If the new location is more than two minutes older, it must be worse
            else if (isSignificantlyOlder)
            {
                return false;
            }

            // Check whether the new location fix is more or less accurate
            int accuracyDelta = (int)(location.Accuracy - currentBestLocation.Accuracy);
            bool isLessAccurate = accuracyDelta > 0;
            bool isMoreAccurate = accuracyDelta < 0;
            bool isSignificantlyLessAccurate = accuracyDelta > 200;
            
            // Check if the old and new location are from the same provider
            bool isFromSameProvider = IsSameProvider(location.Provider,
            currentBestLocation.Provider);

            // Determine location quality using a combination of timeliness and accuracy
            if (isMoreAccurate)
            {
                return true;
            }
            else if (isNewer && !isLessAccurate)
            {
                return true;
            }
            else if (isNewer && !isSignificantlyLessAccurate && isFromSameProvider)
            {
                return true;
            }
            return false;
        }

        // Checks whether two providers are the same
        private bool IsSameProvider(string provider1, string provider2)
        {
            if (provider1 == null)
            {
                return provider2 == null;
            }
            return provider1.Equals(provider2);
        }
        #endregion

        #region Implemented Listener

        #region Location Callbacks

        /// <summary>
        ///  Called when the location has been updated.
        /// </summary>
        /// <param name="location"></param>
        public void OnLocationChanged(Location location)
        {
            // Check if new location is more accuracy / better than current best location
            if (IsBetterLocation(location, currentBestLocation))
            {
                currentBestLocation = location;

                // Broadcast new location
                Intent intent = new Intent(Constants.LocationUpdate);
                intent.PutExtra("location", currentBestLocation);
                SendBroadcast(intent);
            }
        }

        // Called when the user disables the provider
        public void OnProviderDisabled(string provider)
        {
            // Broadcast new provider state
            Intent intent = new Intent(Constants.ProviderStatusChanged);
            intent.PutExtra("status", "disabled");
            SendBroadcast(intent);
        }

        // Called when the user enables the provider
        public void OnProviderEnabled(string provider)
        {
            // Broadcast new provider state
            Intent intent = new Intent(Constants.ProviderStatusChanged);
            intent.PutExtra("status", "enabled");
            SendBroadcast(intent);
        }

        // Called when the status of the provider changes (there are a variety of reasons for this)
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        { }
        #endregion

        public void OnBroadcastReceive(Intent intent)
        {
            switch (intent.Action)
            {
                // Stop service when broadcast is received
                case Constants.ServiceStopAction:
                    StaticStorage.IsServiceRunning = false;
                    locationManager.RemoveUpdates(this);
                    StopSelf();
                    break;
            }
        }
        #endregion
    }
}