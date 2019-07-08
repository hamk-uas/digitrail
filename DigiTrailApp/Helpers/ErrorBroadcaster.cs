using System;
using Android.App;
using Android.Content;
using Android.Util;
using Java.Lang;
using Exception = System.Exception;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// Used to broadcast an error from a Fragment to an underlying Activity
    /// </summary>
    static class ErrorBroadcaster
    {
        /// <summary>
        /// Broadcast an error from the Fragment to the underlying Activity
        /// </summary>
        /// <param name="activity">Activity for broadcasting the error</param>
        /// <param name="e">Exception</param>
        public static void BroadcastError(Activity activity, SystemException e)
        {
            // Log exception + call stack
            Log.Error(Constants.LogTag, e.ToString());
            // Broadcast exception message
            Intent intent = new Intent(Constants.ErrorDIalog);
            intent.PutExtra(Constants.ErrorDIalog, e.Message);
            activity.SendBroadcast(intent);
        }

        /// <summary>
        /// Broadcast an error from the Fragment to the underlying Activity
        /// </summary>
        /// <param name="activity">Activity for broadcasting the error</param>
        /// <param name="e">Exception</param>
        public static void BroadcastError(Activity activity, RuntimeException e)
        {
            // Log exception + call stack
            Log.Error(Constants.LogTag, e.ToString());
            // Broadcast exception message
            Intent intent = new Intent(Constants.ErrorDIalog);
            intent.PutExtra(Constants.ErrorDIalog, e.Message);
            activity.SendBroadcast(intent);
        }

        /// <summary>
        /// Broadcast an error from the Fragment to the underlying Activity
        /// </summary>
        /// <param name="activity">Activity for broadcasting the error</param>
        /// <param name="e">Exception</param>
        public static void BroadcastError(Activity activity, Exception e)
        {
            // Log exception + call stack
            Log.Error(Constants.LogTag, e.ToString());
            // Broadcast exception message
            Intent intent = new Intent(Constants.ErrorDIalog);
            intent.PutExtra(Constants.ErrorDIalog, e.Message);
            activity.SendBroadcast(intent);
        }
    }
}