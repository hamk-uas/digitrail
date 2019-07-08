
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using System;

namespace DigiTrailApp.Helpers
{

    // This class creates new notification, displays and cancels them
    class NotificationProvider
    {

        private static NotificationManager manager;
        private static Vibrator vibrator;

        private const string tag = "NotificationProvider";

        private const string NOTIFICATION_CHANNEL_IDHIGH = "DIGITRAIL_NOTIFICATION_HIGH";
        private const string NOTIFICATION_CHANNEL_IDDEFAULT = "DIGITRAIL_NOTIFICATION_DEFAULT";

        /// <summary>
        /// Creates a new notification using <see cref="Notification.Builder"/>
        /// Title and content are provided on call
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="content">Content of the notification</param>
        /// <param name="label">String id of the notification. Can be null</param>
        /// <param name="id">Int id of the notification</param>
        public static void CreateNotification(string title, string content, string label, int id, bool vibrate)
        {

            // Set up an intent so that tapping the notifications returns to this app
            Intent intent = new Intent(Application.Context, typeof(MainActivity));

            // Create a PendingIntent. Currently using only one PendingIntent (ID = 0)
            PendingIntent pendingIntent = PendingIntent.GetActivity(Application.Context, 0, intent, PendingIntentFlags.OneShot);

            // If build version API is 26 or higher it is required to use notification channel
            if ((int)Build.VERSION.SdkInt >= 26)
            {
                CreateNotificationChannel(true, vibrate);
            }

            // Create notification builder object
            Notification.Builder builder;

            // If build version API is 26 or higher it is required to use builder with channel id parameter
            if ((int)Build.VERSION.SdkInt >= 26)
            {
                if(vibrate)
                    builder = new Notification.Builder(Application.Context, NOTIFICATION_CHANNEL_IDHIGH);
                else
                    builder = new Notification.Builder(Application.Context, NOTIFICATION_CHANNEL_IDDEFAULT);
            }
            else
            {
                builder = new Notification.Builder(Application.Context);
                // Set notification defaults

                builder.SetDefaults(NotificationDefaults.Vibrate | NotificationDefaults.Sound);


            }

            // Set notification title, content, icon and pending intent
            builder.SetContentTitle(title);
            builder.SetContentText(content);
            builder.SetSmallIcon(Resource.Drawable.iconNotification);
            builder.SetContentIntent(pendingIntent);
            // Set auto cancel true so notification is removed when clicked
            builder.SetAutoCancel(true);

            // Finalize notification by calling builder.Build()
            Notification notification = builder.Build();


            // Get manager and show notification with id
            GetNotificationManager().Notify(label, id, notification);

            // Check if there's notification already sent, cancel it if true
            string markerId = SharedPreferences.GetMarkerId();
            if (markerId != null)
            {
                CancelNotification(markerId, 0);
                SharedPreferences.SetMarkerId(null);
                markerId = null;
            }

            // Save marker id to SharedPreferences
            SharedPreferences.SetMarkerId(label);
        }

        public static Notification CreateForegroundNotification(string title, string content, bool vibrate)
        {
            // Create a pending intent so that MainActivity opens up when the notification is pressed
            PendingIntent OpenMainActivityIntent = PendingIntent.GetActivity(Application.Context, 001, new Intent(Application.Context, typeof(MainActivity)), PendingIntentFlags.OneShot);
            // Create pending intent to broadcast intent to stop service when button in notification is pressed
            PendingIntent stopServiceIntent = PendingIntent.GetBroadcast(Application.Context, 101, new Intent(Constants.ServiceStopAction), PendingIntentFlags.UpdateCurrent);

            if ((int)Build.VERSION.SdkInt >= 26)
            {
                CreateNotificationChannel(false, vibrate);
            }

            Notification.Builder builder;

            // If build version API is 26 or higher it is required to use builder with channel id parameter
            if ((int)Build.VERSION.SdkInt >= 26)
            {
                if(vibrate)
                    builder = new Notification.Builder(Application.Context, NOTIFICATION_CHANNEL_IDHIGH);
                else
                    builder = new Notification.Builder(Application.Context, NOTIFICATION_CHANNEL_IDDEFAULT);
            }
            else
            {
                builder = new Notification.Builder(Application.Context);
            }

            // Notification be shown while service is running
            builder.SetContentTitle(title);
            builder.SetContentText(content);
            builder.SetSmallIcon(Resource.Drawable.iconNotification);
            builder.SetContentIntent(OpenMainActivityIntent);
            builder.SetOngoing(true);

            // Check build version
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                Icon icon = Icon.CreateWithResource(Application.Context, Resource.Drawable.iconNotification);

                // If build version is 23 or higher, use builder.AddAction(action)
                Notification.Action action = new Notification.Action.Builder(icon, Application.Context.GetString(Resource.String.notificationStopServiceButtonContent), stopServiceIntent).Build();
                builder.AddAction(action);
            }
            else
            {
                // If build version is lower than 23, use builder.AddAction(icon, title, intent)
                builder.AddAction(Resource.Drawable.iconNotification, Application.Context.GetString(Resource.String.notificationStopServiceButtonContent), stopServiceIntent);
            }

            // Finalize notification with builder.Build()
            var notification = builder.Build();

            return notification;
        }

        public static void CreateNotificationChannel(bool sound, bool vibrate)
        {

            NotificationChannel notificationChannel;
            if (vibrate)
            {
                // Set notification channel for popups
                notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_IDHIGH, "DigiTrail", NotificationImportance.High);
                notificationChannel.SetVibrationPattern(new long[] { 0, 500, 0, 250, 0, 500, 250 });
                notificationChannel.EnableVibration(true);
            }
            else
            {
                // set notification channel for notification that informs user that digitrails is tracking
                notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_IDDEFAULT, "DigiTrail", NotificationImportance.Default);
                notificationChannel.SetVibrationPattern(new long[] { });
                notificationChannel.EnableVibration(false);
            }
            notificationChannel.Description = null;
            notificationChannel.EnableLights(true);
            notificationChannel.LightColor = Color.Red;
            
            if (!sound)
            {
                // Create the uri for the alarm file                 
                notificationChannel.SetSound(null, null);
            }
            else
            {
                Android.Net.Uri alarmUri = Android.Net.Uri.Parse("android.resource://" + Application.Context.PackageName + "/" + Resource.Raw.notification);
                Ringtone r = RingtoneManager.GetRingtone(Application.Context, alarmUri);
                r.Volume = 1f;
                r.Play();
            }
            // Create notification channel
            GetNotificationManager().CreateNotificationChannel(notificationChannel);
        }

        public static void Vibrate(long ms)
        {

            // If vibrator is null, get a new Vibrator object using GetSystemService()
            if (vibrator == null)
            {
                vibrator = (Vibrator)Application.Context.GetSystemService(Context.VibratorService);
            }

            if ((int)Build.VERSION.SdkInt >= 26)
                vibrator.Vibrate(VibrationEffect.CreateOneShot(ms, VibrationEffect.DefaultAmplitude));
            else
            {
                // This method was deprecated in API 26, but must be used if there's a need to support < API 26
                vibrator.Vibrate(ms);
            }
        }


        /// <summary>
        /// Cancels notification matching provided id
        /// </summary>
        /// <param name="label">String id of the notification</param>
        public static void CancelNotification(string label, int id)
        {

            GetNotificationManager().Cancel(label, id);
        }

        /// <summary>
        /// Cancels all notifications sent by this application
        /// </summary>
        public static void CancelAllNotifications()
        {

            // Get manager and cancel all notifications
            GetNotificationManager().CancelAll();
        }

        /// <summary>
        /// Provides access to <see cref="NotificationManager"/>
        /// </summary>
        /// <returns> Object of <see cref="NotificationManager"/> </returns>
        private static NotificationManager GetNotificationManager()
        {

            // If manager is null, get NotificationManager object using GetSystemService()
            if (manager == null)
            {
                manager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            }
            return manager;
        }
    }
}