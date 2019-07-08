using Android.App;
using Android.Content;
using Android.OS;
using Java.Lang;
using DigiTrailApp.Interfaces;

namespace DigiTrailApp.Fragments
{
    /// <summary>
    /// Custom Alert dialog
    /// </summary>
    public class AlertDialogFragment : Android.Support.V4.App.DialogFragment
    {

        private string title;
        private string message;
        private string positiveButtonText;
        private string negativeButtonText;
        private string id;

        private IOnDialogDismissedListener onDialogDismissedListener;

        /// <summary>
        /// Constructor used to create an alert dialog containing title, message and positive button
        /// </summary>
        /// <param name="context">Context of activity this fragment is associated with</param>
        /// <param name="title">Title of dialog</param>
        /// <param name="message">Content of dialog</param>
        /// <param name="positiveButtonText">Text shown on positive button</param>
        public AlertDialogFragment(Context context, string title, string message, string positiveButtonText)
        {
            ValidateContext(context);

            this.title = title;
            this.message = message;
            this.positiveButtonText = positiveButtonText;

            // Replace empty values with default text
            title = (title ?? GetString(Resource.String.dialogDefaultTitle));
            message = (message ?? GetString(Resource.String.dialogDefaultContent));
            positiveButtonText = (positiveButtonText ?? GetString(Resource.String.btnClose));
        }

        public AlertDialogFragment(Context context, string title, string message, string positiveButtonText, string negativeButtonText, string id)
        {
            ValidateContext(context);

            this.title = title;
            this.message = message;
            this.positiveButtonText = positiveButtonText;
            this.negativeButtonText = negativeButtonText;
            this.id = id;

            //Check if any parameter is empty
            // NOTE! negativeButtonText can be null if negative button is not required 
            title = ((title != null) ? title : GetString(Resource.String.dialogDefaultTitle));
            message = ((message != null) ? message : GetString(Resource.String.dialogDefaultContent));
            positiveButtonText = ((positiveButtonText != null) ? positiveButtonText : GetString(Resource.String.btnClose));
        }

        private void ValidateContext(Context context)
        {
            // Make sure that interface is implemented in host activity
            try
            {
                onDialogDismissedListener = (IOnDialogDismissedListener)context;
            }
            catch (ClassCastException)
            {
                throw new ClassCastException(context.ToString() + " must implement IOnDismissListener");
            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            // Create dialog using builder
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity, Resource.Style.DigiTrailAlterDialogTheme);
            builder.SetTitle(title);
            builder.SetMessage(message);

            // What happens when positive button is pressed
            builder.SetPositiveButton(positiveButtonText, (sender, args) =>
            {
                onDialogDismissedListener.OnDialogDismissed(Class.SimpleName, true, id);
                Dismiss();
            });

            if (!string.IsNullOrEmpty(negativeButtonText))
            {
                // What happens when negative button is pressed
                builder.SetNegativeButton(negativeButtonText, (sender, args) =>
                {
                    onDialogDismissedListener.OnDialogDismissed(Class.SimpleName, false, id);
                    Dismiss();
                });
            }

            // Return created dialog
            return builder.Create();
        }
    }
}