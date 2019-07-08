using Android.Content;

namespace DigiTrailApp.Interfaces
{
    /// <summary>
    /// Broadcast listener
    /// </summary>
    public interface IBroadcastListener
    {
        void OnBroadcastReceive(Intent intent);
    }
}