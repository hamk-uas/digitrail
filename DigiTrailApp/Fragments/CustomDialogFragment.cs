using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using DigiTrailApp.Interfaces;

namespace DigiTrailApp.Fragments
{
    /// <summary>
    /// Custom Dialog
    /// </summary>
    public class CustomDialogFragment : Android.Support.V4.App.DialogFragment
    {
        private string contentText;
        private string titleText;
        private string btnPositiveText;
        private string btnNegativeText;
        private string id;

        private Context context;
        private Type type;

        private ICustomDialogListener customDialogListener;

        /// <summary>
        /// Consturctor of CustomDialogFragment
        /// </summary>
        /// <param name="context">Context of activity this fragment is associated with</param>
        /// <param name="titleText">Title of dialog</param>
        /// <param name="contentText">Content of dialog</param>
        /// <param name="btnPositiveText">Text shown on positive button</param>
        /// <param name="btnNegativeText">Text shown on negative button</param>
        /// <param name="id">Identifier of dialog</param>
        public CustomDialogFragment(Context context, string titleText, string contentText, string btnPositiveText, string btnNegativeText, string id)
        {
            SetContent(context, titleText, contentText, btnPositiveText, btnNegativeText, id);
        }


        private void SetContent(Context context, string titleText, string contentText, string btnPositiveText, string btnNegativeText, string id)
        {
            // Make sure that interface is implemented in host activity
            try
            {
                customDialogListener = (ICustomDialogListener)context;
            }
            catch (ClassCastException)
            {
                throw new ClassCastException(context.ToString() + " must implement ICustomDialogListener");
            }

            this.titleText = titleText;
            this.contentText = contentText;
            this.btnPositiveText = btnPositiveText;
            this.btnNegativeText = btnNegativeText;
            this.context = context;
            this.id = id;

            // Replace empty values with default text
            titleText = ((titleText != null) ? titleText : GetString(Resource.String.dialogDefaultTitle));
            contentText = ((contentText != null) ? contentText : GetString(Resource.String.dialogDefaultContent));
            btnPositiveText = ((btnPositiveText != null) ? btnPositiveText : GetString(Resource.String.btnOk));
            btnNegativeText = ((btnNegativeText != null) ? btnNegativeText : GetString(Resource.String.btnCancel));
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // Create view and return it
            View view = inflater.Inflate(Resource.Layout.GpsInternetDialogLayout, container, false);

            var tvTitle = view.FindViewById<TextView>(Resource.Id.tvCustomDialogTitle);
            tvTitle.Text = titleText;

            var tvContent = view.FindViewById<TextView>(Resource.Id.tvCustomDialogContent);
            tvContent.Text = contentText;

            var btnPositive = view.FindViewById<Button>(Resource.Id.btnCustomDialogPositive);
            btnPositive.Text = btnPositiveText;

            var btnNegative = view.FindViewById<Button>(Resource.Id.btnCustomDialogNegative);
            btnNegative.Text = btnNegativeText;

            btnPositive.Click += BtnOk_Click;
            btnNegative.Click += BtnCancel_Click;

            return view;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            customDialogListener.OnCustomDialogDismissed(false, id);
            Dismiss();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            customDialogListener.OnCustomDialogDismissed(true, id);
            Dismiss();
        }
    }
}