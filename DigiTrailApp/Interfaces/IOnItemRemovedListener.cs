namespace DigiTrailApp.Interfaces
{
    /// <summary>
    /// Item removed listener
    /// </summary>
    public interface IOnItemRemovedListener
    {
        void OnItemRemoved(string fragment, bool state, int position, string id);
    }
}