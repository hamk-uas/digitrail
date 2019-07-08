using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using DigiTrailApp.Helpers;
using Java.Lang;
using System;
using DigiTrailApp.Interfaces;

namespace DigiTrailApp.Fragments
{
    //DEV purposes
    internal class MarkerFragment : Android.Support.V4.App.Fragment
    {
        private ImageView ivImage;
        private TextView tvTitle, tvDescription, link;
        private Button btnClose;
        private IOnDialogDismissedListener onDialogDismissedListener;
        internal const string MARKER_TITLE = "MARKER_TITLE";
        internal const string MARKER_DESCRIPTION = "MARKER_DESCRIPTION";
        internal const string MARKER_LINK = "MARKER_LINK";
        internal const string MARKER_IMAGE = "MARKER_IMAGE";

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);

            // Make sure that interface is implemented in host activity
            try
            {
                onDialogDismissedListener = (IOnDialogDismissedListener)context;
            }
            catch (ClassCastException)
            {
                throw new ClassCastException(context.ToString() + " must implement IOnDialogDismissedListener");
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // Use this to return your custom view for this Fragment
            View View = inflater.Inflate(Resource.Layout.MarkerLayout, container, false);
            ivImage = View.FindViewById<ImageView>(Resource.Id.ivMarkerLayout);
            tvTitle = View.FindViewById<TextView>(Resource.Id.tvMarkerLayoutTitle);
            tvDescription = View.FindViewById<TextView>(Resource.Id.tvMarkerLayoutDescription);
            btnClose = View.FindViewById<Button>(Resource.Id.btnMarkerLayoutClose);
            btnClose.Click += BtnClose_Click;
            link = View.FindViewById<TextView>(Resource.Id.link);

            try
            {
                // If Arguments (Bundle) is not null 
                if (Arguments != null)
                {
                    // Set variables
                    if (!string.IsNullOrEmpty(Arguments.GetString(MARKER_TITLE)))
                        tvTitle.Text = Arguments.GetString(MARKER_TITLE);

                    if (!string.IsNullOrEmpty(Arguments.GetString(MARKER_DESCRIPTION)))
                        tvDescription.Text = Arguments.GetString(MARKER_DESCRIPTION);

                    if (!string.IsNullOrEmpty(Arguments.GetString(MARKER_LINK)))
                    {
                        link.Text = Arguments.GetString(MARKER_LINK);
                        link.Visibility = ViewStates.Visible;
                    }

                    SetImage();
                }
            }
            catch (NullPointerException e)
            {
                ErrorBroadcaster.BroadcastError(Activity, e);
            }
            catch (IllegalStateException e)
            {
                ErrorBroadcaster.BroadcastError(Activity, e);
            }
            catch (ArgumentException e)
            {
                ErrorBroadcaster.BroadcastError(Activity, e);
            }


            return View;
        }

        private async void SetImage()
        {
            if (!string.IsNullOrEmpty(Arguments.GetString(MARKER_IMAGE)))
            {
                byte[] requestArray = await new HttpRequest().GetImage(Arguments.GetString(MARKER_IMAGE));
                //Assign the image to the imageview
                ivImage.SetImageBitmap(BitmapFactory.DecodeByteArray(requestArray, 0, requestArray.Length));
                //Make the imageview visible
                ivImage.Visibility = ViewStates.Visible;
            }
            else
            {
                byte[] bundleArray = Arguments.GetByteArray(MARKER_IMAGE);

                if (bundleArray != null && bundleArray.Length > 0)
                {
                    //Assign the image to the imageview
                    ivImage.SetImageBitmap(BitmapFactory.DecodeByteArray(bundleArray, 0, bundleArray.Length));
                    //Make the imageview visible
                    ivImage.Visibility = ViewStates.Visible;
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            //Hide the ImageView
            ivImage.Visibility = ViewStates.Gone;
            //Call to hide the Fragment
            Activity.OnBackPressed();
        }

        public override void OnDetach()
        {
            base.OnDetach();
            onDialogDismissedListener.OnDialogDismissed(Class.SimpleName, true, null);
        }

        internal static Bundle GetBundle(string title, string description, string link, byte[] image)
        {
            Bundle bundle = new Bundle();
            bundle.PutString(MARKER_TITLE, title);
            bundle.PutString(MARKER_DESCRIPTION, description);
            bundle.PutString(MARKER_LINK, link);
            bundle.PutByteArray(MARKER_IMAGE, image);
            return bundle;
        }

        internal static Bundle GetBundle(string title, string description, string link, string imageURL)
        {
            Bundle bundle = new Bundle();
            bundle.PutString(MARKER_TITLE, title);
            bundle.PutString(MARKER_DESCRIPTION, description);
            bundle.PutString(MARKER_LINK, link);
            bundle.PutString(MARKER_IMAGE, imageURL);
            return bundle;
        }
    }
}