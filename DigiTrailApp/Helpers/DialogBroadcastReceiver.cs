using Android.Content;
using DigiTrailApp.Interfaces;
using Java.Lang;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// Broadcastreseiver/listener
    /// </summary>
    [BroadcastReceiver]
    public class DialogBroadcastReceiver : BroadcastReceiver
    {
        IBroadcastListener broadcastListener;

        // Mandatory default constructor
        public DialogBroadcastReceiver() { }

        public DialogBroadcastReceiver(Context context)
        {
            try
            {
                // Make sure that interface is implemented in host activity
                broadcastListener = (IBroadcastListener)context;
            }
            catch (ClassCastException)
            {
                throw new ClassCastException(context.ToString() + " must implement IBroadcastListener");
            }
        }

        public override void OnReceive(Context context, Intent intent)
        {
            broadcastListener.OnBroadcastReceive(intent);
        }
    }
}