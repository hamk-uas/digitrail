namespace DigiTrailApp.Interfaces
{
    /// <summary>
    /// Interface for when this dialog is dismissed
    /// </summary>
    public interface IOnDialogDismissedListener
    {
        void OnDialogDismissed(string fragment, bool state, string data);

        void OnDialogDismissed(string fragment, bool state, int dataInt, string dataString);
    }
}