using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using DigiTrailApp.AzureTables;
using DigiTrailApp.Helpers;
using Java.Lang;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Net.Http;
using DigiTrailApp.Interfaces;
using DigiTrailApp.Azure;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DigiTrailApp.Fragments
{
    /// <summary>
    /// For objective (multiple choice) theme fragment
    /// </summary>
    class ObjectiveThemeMarkerFragment : Android.Support.V4.App.Fragment
    {
        private ImageView ivImage;
        private TextView tvTitle, tvDescription, link;
        private Button btnClose;
        private IOnDialogDismissedListener onDialogDismissedListener;
        private ListView lvAnswers;
        private List<ObjectiveThemeMarkerAnswer> answersList = new List<ObjectiveThemeMarkerAnswer>();
        private ObjectiveThemeMarkerAnswer answer;
        private Marker Marker;

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
            View View = inflater.Inflate(Resource.Layout.ObjectiveThemeMarkerLayout, container, false);
            ivImage = View.FindViewById<ImageView>(Resource.Id.ivObjectiveThemeMarkerLayout);
            tvTitle = View.FindViewById<TextView>(Resource.Id.tvObjectiveThemeMarkerLayoutTitle);
            tvDescription = View.FindViewById<TextView>(Resource.Id.tvObjectiveThemeMarkerLayoutDescription);
            btnClose = View.FindViewById<Button>(Resource.Id.btnObjectiveThemeMarkerLayoutClose);
            btnClose.Click += BtnClose_Click;
            link = View.FindViewById<TextView>(Resource.Id.tvObjectiveThemeMarkerLayoutLink);
            lvAnswers = View.FindViewById<ListView>(Resource.Id.lvObjectiveThemeMarkerLayoutAnswerlist);
            lvAnswers.ItemClick += LvAnswers_ItemClick;

            string arguments = null;
            
            // If Arguments (Bundle) is not null 
            if (Arguments != null)
            {
                // Get arguments 
                arguments = Arguments.GetString(Class.SimpleName);
            }

            ShowMarker(arguments);

            return View;
        }

        private void LvAnswers_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if(answer == null)
            {
                answer = answersList[e.Position];
                //Go through answers in the adapter
                for (int i = 0; i < lvAnswers.Adapter.Count; i++)
                {
                    //Fetch view of the answer
                    CheckedTextView view = (CheckedTextView)lvAnswers.Adapter.GetView(i, lvAnswers.GetChildAt(i), lvAnswers);
                    //Set the answered item as checked
                    if (i == e.Position)
                        view.Checked = true;
                    //Check if the answert is correct
                    if (answersList[i].Correct)
                    {
                        //Colour the answer green for "Correct"
                        view.SetBackgroundResource(Resource.Color.dt_green);
                    }
                    else
                        //Colour the answer dark orange for "Incorrect"
                        view.SetBackgroundResource(Resource.Color.dt_darkOrange);
                }
                Toast.MakeText(Activity, GetString(Resource.String.toastObjectiveReturn), ToastLength.Short).Show();
            }
            else
                Toast.MakeText(Activity, GetString(Resource.String.toastObjectiveReturn), ToastLength.Short).Show();
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

            //Check if we got an answer from the user
            if (answer != null)
                //Report the result
                onDialogDismissedListener.OnDialogDismissed(Class.SimpleName, answer.Correct, answer.Points, Marker.Id);
            else
                //Default callback
                onDialogDismissedListener.OnDialogDismissed(Class.SimpleName, true, null);
        }

        private async void ShowMarker(string markerID)
        {
            try
            {
                //Check that markerId is not null
                if (markerID == null)
                    throw new NullPointerException(GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));
                //Fetch a Marker from DB
                Marker = await AzureClient.GetMarker(markerID);
                //Fetch a translation for the Marker
                MarkerTranslation markerTranslation = await AzureClient.GetMarkerTranslation(Marker.Id, LocaleManager.GetLocale());
                //Fetch an ObjectiveThememarker
                ObjectiveThemeMarker objectiveThemeMarker = await AzureClient.GetObjectiveThemeMarker(Marker.Id);
                //Assign list of answers to local variable
                answersList = await AzureClient.GetObjectiveThemeMarkerAnswers(objectiveThemeMarker.Id);
                //Create new random generator
                Random random = new Random();
                //Randomize the list of answers
                answersList = answersList.OrderBy(x => random.Next()).ToList();
                //Quick solution to show answers
                //>>
                //Create an array for the answers
                string[] answers = new string[answersList.Count];
                for(int i = 0; i < answersList.Count; i++)
                {
                    //Fetch a translation for the answer
                    ObjectiveThemeMarkerAnswerTranslation objectiveThemeMarkerAnswerTranslation = await AzureClient.GetObjectiveThemeMarkerAnswerTranslation(answersList[i].Id, LocaleManager.GetLocale());
                    //Assign the answer to the array
                    answers[i] = objectiveThemeMarkerAnswerTranslation.Text;
                }
                //Get a built in ArrayAdapter
                ArrayAdapter adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItemSingleChoice, answers);
                //Assign the adapter to the ListView
                lvAnswers.Adapter = adapter;
                //<<
                //Assign the title for the Marker
                tvTitle.Text = markerTranslation.Title;
                //Check if the Marker has the optional Description
                if (!string.IsNullOrEmpty(markerTranslation.Description))
                {
                    //Assign the text to the view
                    tvDescription.Text = markerTranslation.Description;
                }
                // Check if there's a link provided
                if (!string.IsNullOrEmpty(markerTranslation.Link))
                {
                    link.Text = markerTranslation.Link;
                    link.Visibility = ViewStates.Visible;
                }

                //Check if the Marker has the optional image
                if (!string.IsNullOrEmpty(Marker.Image))
                {
                    if (ServiceInformation.IsConnectionActive())
                    {
                        //Fetch the image as byte array
                        byte[] vs = await new HttpRequest().GetImage(Marker.Image);
                        //Assign the image to the imageview
                        ivImage.SetImageBitmap(await BitmapFactory.DecodeByteArrayAsync(vs, 0, vs.Length));
                        //Make the imageview visible
                        ivImage.Visibility = ViewStates.Visible;
                    }
                }
            }
            catch (HttpRequestException e)
            {
                ErrorBroadcaster.BroadcastError(Activity, e);
            }
            catch (ArgumentNullException e)
            {
                ErrorBroadcaster.BroadcastError(Activity, e);
            }
            catch (NullPointerException e)
            {
                ErrorBroadcaster.BroadcastError(Activity, e);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                ErrorBroadcaster.BroadcastError(Activity, e);
            }
            catch (UriFormatException e)
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
            catch (WebException e)
            {
                ErrorBroadcaster.BroadcastError(Activity, e);
            }
        }
    }
}