using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using DigiTrailApp.AsyncTasks;
using DigiTrailApp.Azure;
using DigiTrailApp.Fragments;
using DigiTrailApp.Helpers;
using Java.Lang;
using Java.Util;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DigiTrailApp.Interfaces;
using System.Collections.Generic;

namespace DigiTrailApp
{
    /// <summary>
    /// Loading screen for fetching data from Azure storage
    /// </summary>
    [Activity(Label = "DigiTrail", LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
    ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/DigiTrailTheme", NoHistory = false)]

    public class SplashScreen : AppCompatActivity, ICustomDialogListener, IOnDialogDismissedListener
    {
        // Button for user to try to retry initializing backend connection
        private Button btnSplashRetry;
        // Textview for containing loading texts
        private TextView tvText;
        // Variable for storing current status text
        private static string status;
        // Variable for storing the current window focus status
        private static bool hasFocus;
        // Container for loading indicators
        private LinearLayout rlSplashLoadingContainer;
        // Animation
        private LottieAnimationView animationView;
        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(LocaleManager.UpdateBaseContextLocale(@base));
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Check and set the language preference
            try
            {
                LocaleManager.SetLocale(Locale.Default.Language);
            }
            catch (ArgumentNullException e)
            {
                Log.Error(Class.SimpleName, e.ToString());
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk));
            }

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplasLayout);
            //Button for user to try to retry initializing backend connection
            btnSplashRetry = FindViewById<Button>(Resource.Id.btnSplashRetry);
            // Textview for containing loading texts
            tvText = FindViewById<TextView>(Resource.Id.tvSlpashScreenText);
            //Assign click handler for the button
            btnSplashRetry.Click += BtnRetry_Click;
            // Container for loading indicators
            rlSplashLoadingContainer = FindViewById<LinearLayout>(Resource.Id.rlSplashLoadingContainer);
            animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            animationView.Visibility = ViewStates.Visible;
            animationView.ImageAssetsFolder = "Images";
            SetLoadingAnimationState(true);
        }
        /// <summary>
        /// Handle retry button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnRetry_Click(object sender, EventArgs e)
        {
            //Make the button invisible again
            btnSplashRetry.Visibility = ViewStates.Invisible;
            //Make sure the loading indicator and status text container are visible
            if (rlSplashLoadingContainer.Visibility != ViewStates.Visible)
                rlSplashLoadingContainer.Visibility = ViewStates.Visible;
            //Retry init
            await InitAzure();
        }

        public override async void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            /* TODO: If the main window loses focus during InitAzure task
             * i.e Recents is opened or Home button pressed
             * the UI doesn't update and HasWindowFocus returns false when StartMainActivity is called from InitAzure
             * even if onWindowFocusChanged(true) is called
             * If the main window loses focus after the task has finished and receives it again
             * the app works normally again
             * Is this because await InitAzure is called in a different instance of SplashScreen?
             * Or something else? Idunnolo.jpg
             * A static bool to the rescue :)))))) */

            //Assign the current focus status to a static variable
            SplashScreen.hasFocus = hasFocus;

            //If we regained focus and Retry-button is not visible indicating that a previous attempt at initialization didn't fail
            if (hasFocus && btnSplashRetry.Visibility != ViewStates.Visible)
            {
                // If there is no internet connection AND this is the first time the app runs
                if (!ServiceInformation.IsConnectionActive() && !SharedPreferences.GetFirstTimeRan())
                {
                    // Warn the user that the app needs an Internet connection on the first run
                    ShowCustomDialog(this,
                        GetString(Resource.String.dialogInternetTitle),
                        GetString(Resource.String.dialogInternetContentFirstTime),
                        GetString(Resource.String.btnOpen),
                        GetString(Resource.String.btnExit),
                        Constants.NetworkDialog);
                }
                else if (!ServiceInformation.IsConnectionActive() && !StaticStorage.IsNetworkDialogShown)
                {
                    // Warn the user that the app might not work as intended in offline
                    ShowCustomDialog(this,
                        GetString(Resource.String.dialogInternetTitle),
                        GetString(Resource.String.dialogInternetContent),
                        GetString(Resource.String.btnOpen),
                        GetString(Resource.String.btnClose),
                        Constants.NetworkDialog);

                    // Even though we have no Internet, we can still init local storage because the app has been started once before
                    if (!AzureClient.IsInitialized() || !AzureClient.IsDataFetched && !AzureClient.IsFetchingData)
                        await InitAzure();
                }
                // If GPS is not enabled
                else if (!ServiceInformation.IsGpsEnabled() && !StaticStorage.IsGpsDialogShown)
                {
                    // Warn the user that the app cant show location or do route guidance
                    ShowCustomDialog(this,
                        GetString(Resource.String.dialogGpsTitle),
                        GetString(Resource.String.dialogGpsContent),
                        GetString(Resource.String.btnOpen),
                        GetString(Resource.String.btnClose),
                        Constants.GpsDialog);

                    //Start initializing Azure connection and local storage
                    if (!AzureClient.IsInitialized() || !AzureClient.IsDataFetched && ServiceInformation.IsConnectionActive() && !AzureClient.IsFetchingData)
                        await InitAzure();
                }
                // Check if Azure has been initialized already
                else if (!AzureClient.IsInitialized() || !AzureClient.IsDataFetched && ServiceInformation.IsConnectionActive() && !AzureClient.IsFetchingData)
                {
                    await InitAzure();
                }
                // If Azure has been initialized and is not currently fetching data from the backend, move on
                else if (AzureClient.IsInitialized() && !AzureClient.IsFetchingData)
                {
                    StartMainActivity();
                }

            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (!string.IsNullOrEmpty(tvText.Text))
                //Store the current status text
                status = tvText.Text;
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (!string.IsNullOrEmpty(status))
                //Fetch the previous status text
                tvText.Text = status;
        }

        private async Task InitAzure()
        {
            //Azure init
            try
            {
                if (!AzureClient.IsInitialized())
                {
                    //Init Mobile Services for current platform (Android)
                    SetStatusText(GetString(Resource.String.splashInit));
                    CurrentPlatform.Init();

                    //Initialize Client and Local Storage
                    SetStatusText(GetString(Resource.String.splashConn));
                    await AzureClient.Initialize();
                }

                //Check that we have an active Internet connection
                if (ServiceInformation.IsConnectionActive())
                {
                    await AzureClient.RefreshVersion();


                    if (!IsNewestVersionInstalled())
                    {
                        try
                        {
                            // Nuke old database to force update when new version is installed
                            AzureClient.DropDatabase();
                        }
                        catch (System.IO.IOException ex)
                        {
                            ShowAlertDialog(GetString(Resource.String.errorTitle), ex.Message, GetString(Resource.String.btnOk));
                        }

                        SetStatusText(GetString(Resource.String.splashCheck));

                        // Let user know that an update is required to use the app
                        ShowCustomDialog(this, GetString(Resource.String.dialogOldVersionTitle), GetString(Resource.String.dialogOldVersionContent), GetString(Resource.String.btnOk), GetString(Resource.String.btnExit), Constants.VersionDialog);
                    }
                    else
                    {
                        //Set status text
                        if (!SharedPreferences.GetFirstTimeRan())
                            SetStatusText(GetString(Resource.String.splashFetch));
                        else
                            SetStatusText(GetString(Resource.String.splashSync));

                        //Fetch data from the backend
                        await AzureClient.RefreshTables();

                        //Set the status indicating a successful Azure init
                        SetStatusText(GetString(Resource.String.splashDone));
                        //Set a preference indicating that the app has been succesfully initialized at least once
                        SharedPreferences.SetFirsTimeRan();
                        //Check if the app has focus
                        if (hasFocus /*&& !IsDestroyed*/) //TODO: Test if IsDestroyed is required
                                                          //Start mainactivity
                            StartMainActivity();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                //Make sure that the activity is not destroyed so we can display the alert dialog and modify UI elements
                if (!IsDestroyed)
                {
                    ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.dbErrorGeneral) + "\n" + ex.Message, GetString(Resource.String.btnClose));
                    btnSplashRetry.Visibility = ViewStates.Visible;
                    rlSplashLoadingContainer.Visibility = ViewStates.Invisible;
                }
                Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                if (!IsDestroyed)
                {
                    ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.dbErrorInvalid) + "\n" + ex.Message, GetString(Resource.String.btnClose));
                    btnSplashRetry.Visibility = ViewStates.Visible;
                    rlSplashLoadingContainer.Visibility = ViewStates.Invisible;
                }
                Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
            }
            catch (UriFormatException ex)
            {
                if (!IsDestroyed)
                {
                    ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.dbErrorInvalid) + "\n" + ex.Message, GetString(Resource.String.btnClose));
                    btnSplashRetry.Visibility = ViewStates.Visible;
                    rlSplashLoadingContainer.Visibility = ViewStates.Invisible;
                }
                Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void SetLoadingAnimationState(bool state)
        {
            if (state)
            {
                animationView.Visibility = ViewStates.Visible;
                animationView.Loop(true);
                animationView.PlayAnimation();
            }
            else
            {
                animationView.Loop(false);
                animationView.PauseAnimation();
                animationView.Visibility = ViewStates.Invisible;
            }
        }
        private void StartMainActivity()
        {
            // Start downloading gpx files into device's memory
            DownloadTrailFiles();
            //Start the MainActivity
            StartActivity(new Intent(this, typeof(MainActivity)));
            //Finish this activity

            Finish();
        }

        private bool IsNewestVersionInstalled()
        {
            // Create PackageInfo object and use it to get version code 
            PackageInfo packageInfo = PackageManager.GetPackageInfo(PackageName, 0);
            int installedVersion = packageInfo.VersionCode;

            // Query latest version code from database
            AzureTables.Version latestVersion = Task.Run(() => AzureClient.GetVersion()).Result;

            // Compare version codes of application and one stored in database
            // Equal or greater comparison let's developer test newer version w/o updating database
            return installedVersion >= latestVersion.VersionCode;
        }

        private void ShowAlertDialog(string title, string content, string buttonText)
        {
            if (!IsFinishing)
            {
                // Check if any parameter is empty or null and set default values if true
                title = title ?? GetString(Resource.String.dialogDefaultTitle);
                content = content ?? GetString(Resource.String.dialogDefaultContent);
                buttonText = buttonText ?? GetString(Resource.String.btnClose);

                // Create and show the dialog fragment
                new AlertDialogFragment(this, title, content, buttonText).Show(SupportFragmentManager.BeginTransaction(), Constants.AlertDialogFragment);
            }
        }

        private void ShowCustomDialog(Context context, string title, string content, string positiveButtonText, string negativeButtonText, string id)
        {
            new CustomDialogFragment(context, title, content, positiveButtonText, negativeButtonText, id).Show(SupportFragmentManager, Constants.CustomDialogTag);
        }

        public void OnCustomDialogDismissed(bool state, string id)
        {
            switch (id)
            {
                case Constants.GpsDialog:

                    // Indicate that the dialog has been shown to user
                    StaticStorage.IsGpsDialogShown = true;

                    if (state)
                        StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));

                    break;

                case Constants.NetworkDialog:

                    // Indicate that the dialog has been shown to user
                    StaticStorage.IsNetworkDialogShown = true;

                    if (state)
                        StartActivity(new Intent(Android.Provider.Settings.ActionWifiSettings));
                    else if (!state && !SharedPreferences.GetFirstTimeRan())
                        Finish();

                    break;

                case Constants.VersionDialog:

                    if (state)
                    {
                        var uri = Android.Net.Uri.Parse(Constants.AppURL);
                        var intent = new Intent(Intent.ActionView, uri);
                        StartActivity(intent);
                        Finish();
                    }
                    else
                    {
                        Finish();
                    }
                    break;
            }
        }

        /// <summary>
        /// Set the status text to both stored variable and the textview showing the status to the user
        /// </summary>
        /// <param name="text">Status text</param>
        private void SetStatusText(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("Given variable is null!"); //TODO: Translate

            if (tvText != null)
                //Set the status text to the textview
                tvText.Text = text;

            //Set the status text to the stored variable
            status = text;
        }

        public void OnDialogDismissed(string fragment, bool state, string id)
        {

        }

        public void OnDialogDismissed(string fragment, bool state, int dataInt, string dataString)
        {
            
        }

        private async void DownloadTrailFiles()
        {
            //Fetch all remote filepaths for Trails from DB
            List<string> paths = await AzureClient.GetTrailFiles();

            //Check that the list is not empty
            if (paths.Capacity > 0)
                new DownloadTask(new CancellationTokenSource()).DownloadGPXFiles(paths);
        }
    }
}