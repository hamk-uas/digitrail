using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DigiTrailApp.Helpers
{
    class ServiceInformation
    {
        public static bool IsConnectionActive()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;

            if (networkInfo == null || !networkInfo.IsAvailable || !networkInfo.IsConnected)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsGpsEnabled()
        {
            LocationManager locationManager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }
    }
}