namespace DigiTrailApp.Interfaces
{
    /// <summary>
    /// Interface for when this dialog is dismissed
    /// </summary>
    public interface ICustomDialogListener
    {
        void OnCustomDialogDismissed(bool state, string id);
    }
}