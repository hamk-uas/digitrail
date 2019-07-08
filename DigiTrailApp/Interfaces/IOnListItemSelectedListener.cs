namespace DigiTrailApp.Interfaces
{
    /// <summary>
    /// Interface for when a list item is selected
    /// </summary>
    public interface IOnListItemSelectedListener
    {
        void OnListItemSelected(string fragment, string location, string trailName);
    }
}