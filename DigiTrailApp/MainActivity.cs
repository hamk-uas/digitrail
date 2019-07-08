using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Airbnb.Lottie;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using Com.Mapbox.Mapboxsdk.Plugins.Locationlayer;
using Com.Mapbox.Mapboxsdk.Style.Layers;
using DigiTrailApp.AsyncTasks;
using DigiTrailApp.Azure;
using DigiTrailApp.AzureTables;
using DigiTrailApp.BackgroudTasks;
using DigiTrailApp.Fragments;
using DigiTrailApp.Helpers;
using DigiTrailApp.Models;
using Java.Lang;
using Java.Util;
using Microsoft.WindowsAzure.MobileServices;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ListFragment = DigiTrailApp.Fragments.ListFragment;
using Location = Android.Locations.Location;
using MapboxAccountManager = Com.Mapbox.Mapboxsdk.Mapbox;
using Marker = DigiTrailApp.AzureTables.Marker;
using Property = Com.Mapbox.Mapboxsdk.Style.Layers.Property;
using SupportFragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using DrawerLayout = Android.Support.V4.Widget.DrawerLayout;
using Com.Mapbox.Mapboxsdk.Plugins.Locationlayer.Modes;
using Com.Mapbox.Android.Core.Location;
using Com.Mapbox.Mapboxsdk.Style.Sources;
using Android.Graphics;
using Com.Mapbox.Geojson;
using Com.Mapbox.Mapboxsdk.Style.Expressions;
using System.IO;
using Android.Animation;
using Android.Support.V7.Widget;
using Android.Media;
using DigiTrailApp.Interfaces;

namespace DigiTrailApp
{

    /// <summary>
    /// Main Class
    /// </summary>
    [Activity(Label = "DigiTrail", LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.Locale,
    ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/DigiTrailTheme")]
    public class MainActivity : AppCompatActivity, IOnListItemSelectedListener, IOnDialogDismissedListener, ICustomDialogListener, IBroadcastListener, IOnMapReadyCallback, ILocationEngineListener, ICompassListener, IOnItemRemovedListener

    {
        #region Class Variables
        // Developer objects
        private Button btnDev1;
        private Button btnDev2;
        private Button btnDev3;
        private Button btnDev4;
        private Button btnDev5;
        private Button btnDev6;
        private Button ShowDeveloperUI;
        private TextView tvDev1;
        // Elevation chartview
        private Button expandeChart, minimizeChart, hideChart, showChart;
        private View chartView;
        // Variable to know if the activity is currently active
        private static bool mActive;
        //New view for leftdrawer. Resource.Menu.nav_menuLeft.xml contains buttons and header is in Resource.Layout.nav_header.axml
        private NavigationView navigationView;
        private DrawerLayout mDrawerLayout;
        // MapBox variables
        private MapView mapView;
        private MapboxMap mapBoxMap;
        private LocationLayerPlugin locationPlugin;
        private LocationEngine locationEngine;
        //Map Buttons
        private Button btnClientLocation;
        private Button btnBearing;
        private Button btnFinish;
        // Buttons for showing drawers
        private Button ShowMenu;
        private Button ShowLegend;
        //Button to show active notifications
        Button btnShowNotifications;
        // Bar showing active road progress
        private View cardViewDistanceMeter;
        //TextView for Distance
        private TextView tvDistanceMeter;        
        //Textview for selected trail 
        private TextView tvDistanceMeterTitle;
        //distancemeter start and finish icons
        private ImageView ivStartTrailMarker;
        private ImageView ivFinishTrailMarker;
        //Active route variable
        private string currentRoute;
        //Active location variable
        string locationName;
        // Back button pressed tracker
        private static int backPressed = 0;
        // Camera controls
        private static bool isCamFollowingUser;
        // Animation
        private LottieAnimationView animationView;
        private static int fadeAnimDuration = 2000;
        // Broadcast receiver
        private DialogBroadcastReceiver dialogBroadcastReceiver;
        // Markers
        private List<Marker> nearbyMarkers = new List<Marker>();
        private List<LatLng> trailPoints;
        private List<string> visitedMarkers = new List<string>();
        //Location requests
        private Location lastRequestLocation;
        //FeatureCollection, markers
        private FeatureCollection featureCollection;
        //GeoJsonSource for clustering and showing markers featurecollection
        private GeoJsonSource geoSource;
        //markerid list to track currently opened popups
        public List<string> markerIdList;
        // Clusters settings       
        private readonly Float textSize = (Float)17f;
        private readonly Float IconSize = (Float)0.8f;
        //Sensors
        private static bool deviceHasCompass;
        //MapBox Layer and source names
        private readonly string clusterLayerName = "clusterLayer";
        private readonly string symbolLayerName = "symbolLayer";
        private readonly string markersSourceName = "markersSource";
        //Layers for mapbox
        SymbolLayer symbolLayer;
        SymbolLayer clusterLayer;
        //Fragment for popup list
        PopUpRecycleFragment popfrag;
        //Alarmuri and ringTone, NOT IN USE, (Using vibration for notifications, if in background will play phone default notification sound).
        Android.Net.Uri alarmUri;
        Ringtone ringTone;
        //Vibrator
        private static Vibrator vibrator;
        //For cheking if notification fragment is open (fragemnt isVisible not workin, so this is workaround)
        bool isMarkerFragmentOpen;
        //loding cardview
        public CardView loadingCardview;
        //Controller class for Themes with Objective
        private ObjectiveController objectiveController;

        #endregion

        #region Override Methods
        /// <summary>
        /// BaseContext
        /// </summary>
        /// <param name="base"></param>
        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(LocaleManager.UpdateBaseContextLocale(@base));
        }
        /// <summary>
        /// Define what happens on startup
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {           
            dialogBroadcastReceiver = new DialogBroadcastReceiver(this);
            //Set locale language
            try
            {
                //Change language for debug and testing puprposes
                //-->
                //string language = "en_US";                
                //Locale.SetDefault(Locale.Category.Format, Locale.English);
                //LocaleManager.SetLocale(language);              
                //<--
                LocaleManager.SetLocale(Locale.Default.Language);
            }
            catch (ArgumentNullException e)
            {
                Log.Error(Class.SimpleName, e.ToString());
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk));
            }

            base.OnCreate(savedInstanceState);

            // MAIN LAYOUT
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            //Set animations
            animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            animationView.Visibility = ViewStates.Invisible;
            animationView.ImageAssetsFolder = "Images";
            //Notification ringtone (Not in use, using vibration. Phone default notification sound when app in background)
            alarmUri = Android.Net.Uri.Parse("android.resource://" + Application.Context.PackageName + "/" + Resource.Raw.notification);            
            ringTone = RingtoneManager.GetRingtone(Application.Context, alarmUri);
            // MAPBOX
            //MapBox applicaton key
            MapboxAccountManager.GetInstance(ApplicationContext, GetString(Resource.String.accessToken));
            //Set the UI elements
            mapView = FindViewById<MapView>(Resource.Id.mapView);
            //Setting styleurl for map
            mapView.SetStyleUrl(GetString(Resource.String.styleUrl));
            //Call mapview's onCreate
            mapView.OnCreate(savedInstanceState);
            //Initialize the map
            mapView.GetMapAsync(this);

            //Fetch the SensorManager
            SensorManager sensorManager = (SensorManager)GetSystemService(SensorService);
            //Check if the device has a magnetic field sensor, a compass
            if (sensorManager.GetDefaultSensor(SensorType.MagneticField) != null)
                deviceHasCompass = true;

            //setting toolbar         
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = GetString(Resource.String.appName);
            SupportActionBar.Hide();

            //setting drawer
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            var drawerTogle = new Android.Support.V7.App.ActionBarDrawerToggle(this, mDrawerLayout, toolbar, Resource.String.openDrawer, Resource.String.closeDrawer);
            mDrawerLayout.AddDrawerListener(drawerTogle);
            drawerTogle.SyncState();
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            SetupDrawerContent(navigationView);

            //Map buttons
            //Button for showing the menu
            ShowMenu = FindViewById<Button>(Resource.Id.btnShowMenu);
            ShowMenu.Click += ShowMenu_Click;
            //Button for showing the right hand drawer
            ShowLegend = FindViewById<Button>(Resource.Id.btnShowLegend);
            ShowLegend.Click += ShowLegend_Click;
            //Button for finishing an active trail
            btnFinish = FindViewById<Button>(Resource.Id.btnFinishTrail);
            btnFinish.Click += BtnFinish_Click;
            //Button for bearing
            btnBearing = FindViewById<Button>(Resource.Id.btnMainBearing);
            btnBearing.Click += BtnBearing_Click;
            //Button for location
            btnClientLocation = FindViewById<Button>(Resource.Id.btnMainMyLocation);
            btnClientLocation.Click += BtnMyLocation_Click;
            //Button for show notifications
            btnShowNotifications = FindViewById<Button>(Resource.Id.btnShowNotifications);
            btnShowNotifications.Click += BtnShowNotifications_Click;
            //textviews for distnacemeter
            tvDistanceMeter = FindViewById<TextView>(Resource.Id.tvMainDistanceMeter);
            cardViewDistanceMeter = FindViewById<View>(Resource.Id.CardViewTamplate);
            tvDistanceMeterTitle = FindViewById<TextView>(Resource.Id.tvMainDistanceMeterTitle);
            //start and finish icons
            ivFinishTrailMarker = FindViewById<ImageView>(Resource.Id.IVmarkerFinish);
            ivStartTrailMarker = FindViewById<ImageView>(Resource.Id.IVmarkerStart);
            cardViewDistanceMeter.Visibility = ViewStates.Invisible;
            ivFinishTrailMarker.Visibility = ViewStates.Invisible;
            //Active route name and location
            currentRoute = "";
            locationName = "";
            //list for opened popups, showing this in popupfragment
            markerIdList = new List<string>();
           
            //Button for showing developer ui
            ShowDeveloperUI = FindViewById<Button>(Resource.Id.btnShowDevelop);
            ShowDeveloperUI.Click += ShowDeveloperUI_Click;

#if DEBUG
            ShowDeveloperUI.Visibility = ViewStates.Visible;
#else
            ShowDeveloperUI.Visibility = ViewStates.Gone;
#endif

            //Buttons for developer ui
            btnDev1 = FindViewById<Button>(Resource.Id.btnDev1);
            btnDev2 = FindViewById<Button>(Resource.Id.btnDev2);
            btnDev3 = FindViewById<Button>(Resource.Id.btnDev3);
            btnDev4 = FindViewById<Button>(Resource.Id.btnDev4);
            btnDev5 = FindViewById<Button>(Resource.Id.btnDev5);
            btnDev6 = FindViewById<Button>(Resource.Id.btnDev6);
            btnDev1.Click += BtnDev1_Click;
            btnDev2.Click += BtnDev2_Click;
            btnDev3.Click += BtnDev3_Click;
            btnDev4.Click += BtnDev4_Click;
            btnDev5.Click += BtnDev5_Click;
            btnDev6.Click += BtnDev6_Click;
            //TextView in dev UI
            tvDev1 = FindViewById<TextView>(Resource.Id.tvDev1);
            //Elevation chart
            expandeChart = (Button)FindViewById(Resource.Id.expandeChart);
            minimizeChart = (Button)FindViewById(Resource.Id.minimizeChart);
            hideChart = (Button)FindViewById(Resource.Id.hideChart);
            showChart = (Button)FindViewById(Resource.Id.showChart);
            chartView = FindViewById(Resource.Id.chartView);

            loadingCardview = (CardView)FindViewById(Resource.Id.CVloading);
            loadingCardview.Visibility = ViewStates.Gone;
            expandeChart.Click += ExpandeChart_Click;
            minimizeChart.Click += MinimizeChart_Click;
            hideChart.Click += HideChart_Click;
            showChart.Click += ShowChart_Click;

            if (HasWindowFocus)
            {
                // Check if access to fine location is granted
                if (!IsPermissionGranted(Manifest.Permission.AccessFineLocation))
                {
                    // Request access to fine location
                    RequestPermission(Manifest.Permission.AccessFineLocation, Constants.LocationRequestCode);
                }
            }

            if (savedInstanceState != null)
            {
                // Don't show map UI elements if fragment is open
                if (savedInstanceState.GetBoolean(Constants.SETTINGS_FRAGMENT_OPEN))
                {
                    HideMapUIElements();
                }

                if (savedInstanceState.GetBoolean(Constants.TRAIL_ACTIVE))
                {
                    StartTrail(SharedPreferences.GetLastTrail());

                    if (savedInstanceState.GetBoolean(Constants.THEME_ACTIVE))
                    {
                        //Check if Objective has been active
                        if (savedInstanceState.GetBoolean(Constants.OBJECTIVE_ACTIVE) && savedInstanceState.GetBundle(ObjectiveController.OBJECTIVE_BUNDLE) != null)
                            //Initialize new ObjectiveController with ObjectiveThemeMarkers of last Theme and a saved Bundle
                            objectiveController = new ObjectiveController(Task.Run(() => AzureClient.GetObjectiveThemeMarkers(SharedPreferences.GetLastTheme())).Result, savedInstanceState.GetBundle(ObjectiveController.OBJECTIVE_BUNDLE));
                    }
                }
            }

            //Fade out the bearing button
            BearingBtnAnim(1000, false, false);
        }
        /// <summary>
        /// Dafine what happens on save instance state
        /// </summary>
        /// <param name="outState"></param>
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            mapView.OnSaveInstanceState(outState);

            if (SupportFragmentManager.BackStackEntryCount > 0 || FragmentManager.BackStackEntryCount > 0)
            {
                outState.PutBoolean(Constants.SETTINGS_FRAGMENT_OPEN, true);
            }

            if (StaticStorage.IsTrailActive)
            {
                outState.PutBoolean(Constants.TRAIL_ACTIVE, true);

                if (StaticStorage.IsThemeActive)
                {
                    outState.PutBoolean(Constants.THEME_ACTIVE, true);

                    //Check if we have an Objective active
                    if (StaticStorage.IsObjectiveActive && objectiveController != null)
                    {
                        //Store a flag indicating an active Objective
                        outState.PutBoolean(Constants.OBJECTIVE_ACTIVE, true);
                        //Ask the ObjectiveController to create a Bundle to store it's information
                        outState.PutBundle(ObjectiveController.OBJECTIVE_BUNDLE, objectiveController.GetBundle());
                    }
                }
            }
        }
        /// <summary>
        /// Navigation Drawer creation
        /// </summary>
        /// <param name="menu">Imenu view</param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Navigation Drawer Layout Menu Creation  
            navigationView.InflateMenu(Resource.Menu.nav_menuLeft);
            return base.OnCreateOptionsMenu(menu);
        }
        /// <summary>
        /// Called when the application starts
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();

            //Mapbox calls
            mapView.OnStart();
            if (locationPlugin != null)
                locationPlugin.OnStart();

        }

        /// <summary>
        /// Called when the application is resumed, after being sent to the background.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            //Indicate that the activity is active again
            mActive = true;

            // Stop service when activity is visible
            SendBroadcast(new Intent(Constants.ServiceStopAction));

            // If marker id were sent, then call ShowPopUpDialog() with that id and remove marker id from intent extras
            string markerId = SharedPreferences.GetMarkerId();
            //open new popup if not null
            if (markerId != null)
            {
                ShowPopUpDialog(markerId);
                NotificationProvider.CancelNotification(markerId, 0);
            }

            //Mapbox calls
            mapView.OnResume();
            if (locationEngine != null)
                //Continue requesting location updates
                locationEngine.RequestLocationUpdates();

            // Create intent filter for broadcast receiver
            IntentFilter dialogIntentFilter = new IntentFilter();
            // Action for when the service starts
            dialogIntentFilter.AddAction(Constants.ServiceStartAction);
            // Action for when the service stops
            dialogIntentFilter.AddAction(Constants.ServiceStopAction);
            // Action for showing a dialog when user is close enough to a marker
            dialogIntentFilter.AddAction(Constants.ServiceNewDialogAction);
            // Action for when the service provides location updates
            dialogIntentFilter.AddAction(Constants.LocationUpdate);
            // Action for when the location provider status changes
            dialogIntentFilter.AddAction(Constants.ProviderStatusChanged);
            // Action for when the device battery is low
            dialogIntentFilter.AddAction(Intent.ActionBatteryLow);
            // Action for when the device enters to battery saving mode
            dialogIntentFilter.AddAction(PowerManager.ActionPowerSaveModeChanged);
            // Action for when an error has occured
            dialogIntentFilter.AddAction(Constants.ErrorDIalog);

            // Register receiver using intent filter
            RegisterReceiver(dialogBroadcastReceiver, dialogIntentFilter);

            //Check if the help dialog is shown
            if (!StaticStorage.IsHelpDialogShown)
            {
                //Show the help dialog
                ShowAlertDialog(GetString(Resource.String.firstRunHelpTitle), GetString(Resource.String.firstRunHelpText), GetString(Resource.String.btnClose));
                //Indicate that help dialog has been shown
                StaticStorage.IsHelpDialogShown = true;
            }
        }
        /// <summary>
        /// Called each time the application goes to the background.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            // Indicate that the activity is deactivated
            mActive = false;
            //set last known location
            var lastKnownLocation = LastKnownLocation();
            if (lastKnownLocation != null)
            {
                SharedPreferences.SetLatitude((float)lastKnownLocation.Latitude);
                SharedPreferences.SetLongitude((float)lastKnownLocation.Longitude);
            }

            //Mapbox calls
            mapView.OnPause();
            if (locationEngine != null)
                locationEngine.RemoveLocationUpdates();
            
            // If activity is in process of finishing
            if (!IsFinishing)
            {
                // Start service if access to fine location is granted and gps provider is enabled
                if (IsPermissionGranted(Manifest.Permission.AccessFineLocation) && ServiceInformation.IsGpsEnabled())
                {
                    var powerManager = (PowerManager)GetSystemService(PowerService);

                    if (!powerManager.IsPowerSaveMode || SharedPreferences.GetIgnoreBatteryState())
                    {
                        //Check version code
                        //Use StartService android 7 and older versions
                        if ((int)Build.VERSION.SdkInt >= 26)
                        {
                            StartForegroundService(new Intent(this, typeof(UserInRangeService)));
                        }
                        else
                        {
                            StartService(new Intent(this, typeof(UserInRangeService)));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Called when the activity is no longer visible to the user
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();

            //Mapbox calls
            mapView.OnStop();

            if (locationPlugin != null)
                locationPlugin.OnStop();
        }
        /// <summary>
        /// Final method that is called on an Activity instance before it's destroyed and completely removed from memory.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            //stop task if running
            if (DownloadTask.IsTaskRunning)
            {
                DownloadTask.TokenSource.Cancel();
            }

            // Unregister broadcast receiver to avoid leaking context
            if (dialogBroadcastReceiver != null)
                UnregisterReceiver(dialogBroadcastReceiver);

            // Send stop service broadcast
            SendBroadcast(new Intent(Constants.ServiceStopAction));
            //handle azure client and storage
            AzureClient.ResetClient();
            StaticStorage.ResetStorage();

            //Mapbox calls
            mapView.OnDestroy();
            if (locationEngine != null)
                locationEngine.Deactivate();
            //set shared null, without this popup will open at startup
            SharedPreferences.SetMarkerId(null);
            SharedPreferences.SetLastVisitedAttraction(null);
            //Cancel notifications
            NotificationProvider.CancelAllNotifications();
        }
        /// <summary>
        /// Handle back pressed
        /// </summary>
        public override async void OnBackPressed()
        {
            // If left drawer is closed and there's no fragment currently displayed
            if (!mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left) && SupportFragmentManager.BackStackEntryCount == 0 && FragmentManager.BackStackEntryCount == 0)
            {
                //Handle back pressed
                backPressed++;
                if (backPressed == 1)
                {
                    // Show exit toast when back key is pressed once. Reset backPressed counter back to 0 after 1,5 s or 1500 ms
                    Toast.MakeText(this, GetString(Resource.String.toastExit), ToastLength.Long).Show();
                    await Task.Run(async () =>
                    {
                        await Task.Delay(1500);
                        backPressed = 0;
                    });
                }
                if (backPressed >= 2)
                {
                    // Display exit app dialog to user after user presses back key second time within time limit                   
                    ShowAlertDialog(GetString(Resource.String.dialogSendFeedBackTitle), GetString(Resource.String.dialogSendFeedBackContent), GetString(Resource.String.btnYes), GetString(Resource.String.btnExit), Constants.ExitAppDialog);

                }
            }
            // If only left drawer is opened, close it
            else if (mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
            {
                mDrawerLayout.CloseDrawer((int)GravityFlags.Left);
            }
            // If there's a fragment currently shown, let system take care of it by calling base method
            else if (SupportFragmentManager.BackStackEntryCount > 0 || FragmentManager.BackStackEntryCount > 0)
            {
                base.OnBackPressed();
                var map = navigationView.Menu.FindItem(Resource.Id.nav_Map);
                if (!map.IsChecked)
                {
                    map.SetChecked(true);
                }

                // After base.OnBackPressed() if there's no fragment currently shown - map view is currently shown - display UI elemtens and hide action bar
                if (SupportFragmentManager.BackStackEntryCount == 0 && FragmentManager.BackStackEntryCount == 0 && !mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
                {
                    // If left drawer is open, close it
                    SupportActionBar.Hide();
                    ShowMapUIElements();
                }
            }
        }
        /// <summary>
        /// Called when the overall system is running low on memory
        /// </summary>
        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mapView.OnLowMemory();
        }

        /// <summary>
        /// Called when this activity gains (true) and lose (false) focus
        /// </summary>
        /// <param name="hasFocus">True if activity has focus and false if not</param>
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus)
            {
                // Check if gps provider is still enabled. Let user know if user has seen this dialog before
                if (!ServiceInformation.IsGpsEnabled())
                {
                    if (!StaticStorage.IsGpsDialogShown)
                    {
                        new CustomDialogFragment(this,
                    GetString(Resource.String.dialogGpsTitle),
                    GetString(Resource.String.dialogGpsContent),
                    GetString(Resource.String.btnOpen),
                    GetString(Resource.String.btnClose),
                    Constants.GpsDialog);
                    }
                }

                // Check if network connection is still active. Let user know if user has seen this dialog before
                if (!ServiceInformation.IsConnectionActive())
                {
                    if (!StaticStorage.IsNetworkDialogShown)
                    {
                        new CustomDialogFragment(this,
                            GetString(Resource.String.dialogGpsTitle),
                            GetString(Resource.String.dialogGpsContent),
                            GetString(Resource.String.btnOpen),
                            GetString(Resource.String.btnClose),
                            Constants.NetworkDialog);
                    }
                }
            }
        }

        #endregion

        #region Private Functions
        /// <summary>
        /// Start new activity with Eula URL
        /// </summary>
        private void OpenEULA()
        {
            StartActivity(new Intent(Intent.ActionView).SetData(Android.Net.Uri.Parse(Constants.EULAURL)));
        }
        /// <summary>
        /// Open Feedback URL in browser
        /// </summary>
        private void OpenFeedback()
        {
            // Create new intent containing uri parse data and start activity using this intent
            StartActivity(new Intent(Intent.ActionView).SetData(Android.Net.Uri.Parse(Constants.FeedbackURL)));
        }
        /// <summary>
        /// Open Events URL in browser
        /// </summary>
        private void OpenEvents()
        {
            // Create new intent containing uri parse data and start activity using this intent
            StartActivity(new Intent(Intent.ActionView).SetData(Android.Net.Uri.Parse(Constants.EventsUrl)));
        }
        /// <summary>
        /// Open Activities URL in browser
        /// </summary>
        private void OpenActivities()
        {
            // Create new intent containing uri parse data and start activity using this intent
            StartActivity(new Intent(Intent.ActionView).SetData(Android.Net.Uri.Parse(Constants.ActivitiesUrl)));
        }
        /// <summary>
        /// Download trail files from Azure
        /// </summary>
        private async void DownloadTrailFiles()
        {
            //Fetch all remote filepaths for Trails from DB
            List<string> paths = await AzureClient.GetTrailFiles();

            //Check that the list is not empty
            if (paths.Capacity > 0)
                new DownloadTask(new CancellationTokenSource()).DownloadGPXFiles(paths);
        }

        /// <summary>
        /// Show an appropriate Marker Fragment for the given Marker
        /// </summary>
        /// <param name="markerID">ID of Marker to show</param>
        private async void ShowMarkerFragment(string markerID)
        {
            //Check if the marker is an ObjectiveThemeMarker
            if (await AzureClient.IsObjectiveMarker(markerID))
            {
                //If the encountered marker is an ObjectiveMarker, we should have an ObjectiveController initialized
                if (objectiveController != null)
                {
                    //Check that the device is within the ObjectiveThemeMarker's impact range
                    if (IsMarkerInRange(markerID))
                        //Check if user has already visited this Marker
                        if (!objectiveController.IsVisited(markerID))
                            //Show a fragment for ObjectiveThemeMarker
                            ShowFragment(new ObjectiveThemeMarkerFragment(), markerID);
                        else
                            //User has already visited the marker, show a toast
                            Toast.MakeText(this, GetString(Resource.String.toastObjectiveMarkerAlreadyVisited), ToastLength.Short).Show();
                    //Check if we even have a known location for user
                    else if (LastKnownLocation() == null)
                        //No known location available, cannot confirm that user is close to the marker
                        Toast.MakeText(this, GetString(Resource.String.toastCannotConfirmNearToMarker), ToastLength.Short).Show();
                    //Check if user has already visited this Marker
                    else if (objectiveController.IsVisited(markerID))
                        //User has already visited the marker, show a toast
                        Toast.MakeText(this, GetString(Resource.String.toastObjectiveMarkerAlreadyVisited), ToastLength.Short).Show();
                    else
                        //User is not close enough to the marker
                        Toast.MakeText(this, GetString(Resource.String.toastNearToMarker), ToastLength.Short).Show();
                }
                else
                    //Show an error to user
                    ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.errorNoObjectiveController), GetString(Resource.String.btnOk));
            }
            else
            {
                loadingCardview.Visibility = ViewStates.Visible;
                //Show a fragment for other Markers
                ShowFragment(new PagerFragment(markerID), "");
            }

        }

        /// <summary>
        /// Check that the device is within the Marker's impact range
        /// </summary>
        /// <param name="markerID">ID of Marker</param>
        /// <returns>True if device is within range, otherwise false</returns>
        private bool IsMarkerInRange(string markerID)
        {
            // Check if we have acquired a location
            if (LastKnownLocation() == null)
                // We can't measure distance from marker to last known device location, return false
                return false;

            // Fetch a marker
            Marker marker = Task.Run(() => AzureClient.GetMarker(markerID)).Result;
            // Call override method
            return IsMarkerInRange(marker, LastKnownLocation());
        }

        /// <summary>
        /// Check that the given location is within the Marker's impact range
        /// </summary>
        /// <param name="marker">ID of Marker</param>
        /// <param name="location">Location from which to measure distance</param>
        /// <returns>True if location is within impact range, otherwise false</returns>
        private bool IsMarkerInRange(Marker marker, Location location)
        {
            // Array to store distance result
            float[] distance = new float[2];
            // Calculate distance to marker from device's location
            Location.DistanceBetween(marker.Lat, marker.Lon, location.Latitude, location.Longitude, distance);
            // Check if device is within impact range of the marker
            return distance.FirstOrDefault() <= marker.Impactrange;
        }
        #endregion

        #region UI Controls
        /// <summary>
        /// Handle left drawer content
        /// </summary>
        /// <param name="navigationView"></param>
        private void SetupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                // Use switch to determine which menu item was pressed
                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_Map:
                        SupportActionBar.Hide();
                        ShowMapUIElements();
                        if (SupportFragmentManager.BackStackEntryCount == 0 && FragmentManager.BackStackEntryCount == 0)
                        {
                            Toast.MakeText(this, GetString(Resource.String.errorAlreadyInMap), ToastLength.Short).Show();
                        }
                        else
                        {
                            ClearFragmentStacks();
                        }

                        break;
                    //Will open website, replace url
                    case Resource.Id.nav_Activities:
                        //OpenActivities();
                        Toast.MakeText(this, Resource.String.errorFeatureNotInUse, ToastLength.Short).Show();
                        break;
                    //Will open website, replace url
                    case Resource.Id.nav_Feedback:
                        Toast.MakeText(this, Resource.String.errorFeatureNotInUse, ToastLength.Short).Show();
                        //OpenFeedback();
                        break;

                    case Resource.Id.nav_Guide:
                        ShowAlertDialog(GetString(Resource.String.dialogHelpTitle), GetString(Resource.String.firstRunHelpText), GetString(Resource.String.btnClose), null, null);
                        break;

                    case Resource.Id.nav_Locations:
                        SupportActionBar.SetTitle(Resource.String.drawerTrails);
                        ShowFragment(new LocationsFragment(), "");
                        break;

                    case Resource.Id.nav_Events:
                        Toast.MakeText(this, Resource.String.errorFeatureNotInUse, ToastLength.Short).Show();
                        //Will open website, replace url
                        ///OpenEvents();
                        break;

                    case Resource.Id.nav_Settings:
                        SupportActionBar.SetTitle(Resource.String.drawerSettings);

                        FragmentManager.BeginTransaction()
                        .Replace(Resource.Id.frameLayout1, new SettingsFragment())
                        .AddToBackStack(null)
                        .Commit();

                        HideMapUIElements();
                        SupportActionBar.Show();
                        break;

                    case Resource.Id.nav_TermsOS:
                        OpenEULA();
                        break;

                    default:
                        Toast.MakeText(this, Resource.String.errorFeatureNotInUse, ToastLength.Short).Show();
                        break;
                }

                // Finally close drawer
                mDrawerLayout.CloseDrawers();
            };
        }

        /// <summary>
        /// Simple method to hide map related buttons when called
        /// </summary>
        public void HideMapUIElements()
        {
            //Clear animations before hiding the elements!            
            btnShowNotifications.ClearAnimation();
            btnClientLocation.ClearAnimation();
            btnBearing.ClearAnimation();
            btnClientLocation.Visibility = ViewStates.Invisible;
            btnBearing.Visibility = ViewStates.Invisible;
            btnFinish.Visibility = ViewStates.Invisible;
            showChart.Visibility = ViewStates.Invisible;
            btnShowNotifications.Visibility = ViewStates.Invisible;
            // Developer buttons
#if DEBUG
            ShowDeveloperUI.Visibility = ViewStates.Invisible;
#else
            ShowDeveloperUI.Visibility = ViewStates.Gone;
#endif
            ShowLegend.Visibility = ViewStates.Invisible;
            ShowMenu.Visibility = ViewStates.Invisible;
            cardViewDistanceMeter.Visibility = ViewStates.Invisible;
        }

        /// <summary>
        /// Simple method to display map related UI elements
        /// </summary>
        public void ShowMapUIElements()
        {
            //notify startpopup method that there is no open fragments.
            isMarkerFragmentOpen = false;
            loadingCardview.Visibility = ViewStates.Gone;
            //Clear animations before showing the elements
            btnClientLocation.ClearAnimation();
            btnBearing.ClearAnimation();
            btnShowNotifications.ClearAnimation();
            if (markerIdList != null)
            {
                if (markerIdList.Count != 0)
                    btnShowNotifications.Visibility = ViewStates.Visible;
            }
            btnClientLocation.Visibility = ViewStates.Visible;
            //Check if the camera is following the user
            if (isCamFollowingUser)
                btnBearing.Visibility = ViewStates.Visible;
#if DEBUG
            ShowDeveloperUI.Visibility = ViewStates.Visible;
#else
            ShowDeveloperUI.Visibility = ViewStates.Gone;
#endif
            ShowLegend.Visibility = ViewStates.Visible;
            ShowMenu.Visibility = ViewStates.Visible;
            //Check if trail active
            if (StaticStorage.IsTrailActive)
            {
                cardViewDistanceMeter.Visibility = ViewStates.Visible;
                btnFinish.Visibility = ViewStates.Visible;

                if (SharedPreferences.GetLineChartVisibilityState())
                {
                    chartView.Visibility = ViewStates.Visible;
                }
                else
                {
                    showChart.Visibility = ViewStates.Visible;
                }
            }
        }
#endregion

#region Fragment & Dialog Management
        /// <summary>
        /// Show fragment given as parameter
        /// </summary>
        /// <param name="fragment">Fragment to be shown</param>
        private void ShowFragment(SupportFragment fragment, string data)
        {
            //Hide map buttons
            HideMapUIElements();
            //Show the action bar
            SupportActionBar.Show();

            // Start transaction
            // NOTE! Adding transaction to stack allows the back key to bring the user back to last view
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.frameLayout1, fragment)
                .AddToBackStack(null)
                .Commit();

            if (!string.IsNullOrEmpty(data))
            {
                // Pass parameters to fragment using bundle
                Bundle bundle = new Bundle();
                bundle.PutString(fragment.Class.SimpleName, data);
                fragment.Arguments = bundle;
            }
            isMarkerFragmentOpen = true;
        }

        private void ShowFragment(SupportFragment fragment, Bundle bundle)
        {
            //Hide map buttons
            HideMapUIElements();
            //Show the action bar
            SupportActionBar.Show();

            // Start transaction
            // NOTE! Adding transaction to stack allows the back key to bring the user back to last view
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.frameLayout1, fragment)
                .AddToBackStack(null)
                .Commit();

            if (bundle != null)
                fragment.Arguments = bundle;

            isMarkerFragmentOpen = true;
        }
        /// <summary>
        /// Show Alert dialog
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="positiveButtonText"></param>
        /// <param name="negativeButtonText"></param>
        /// <param name="id"></param>
        public void ShowAlertDialog(string title, string content, string positiveButtonText, string negativeButtonText, string id)
        {
            if (!IsFinishing && HasWindowFocus)
            {
                new AlertDialogFragment(this, title, content, positiveButtonText, negativeButtonText, id).Show(SupportFragmentManager.BeginTransaction(), Constants.AlertDialogFragment);
            }
        }

        public void ShowAlertDialog(string title, string content, string positiveButtonText)
        {
            if (!IsFinishing && HasWindowFocus)
            {
                new AlertDialogFragment(this, title, content, positiveButtonText).Show(SupportFragmentManager.BeginTransaction(), Constants.AlertDialogFragment);

            }
        }
        /// <summary>
        /// Show popupdialog
        /// </summary>
        /// <param name="markerID"></param>
        public async void ShowPopUpDialog(string markerID)
        {
            bool isAlreadyOnList = false;
            //set vibration
            long vibrateLong = 200;
            long vibrateShort = 100;
            SupportActionBar.Title = GetString(Resource.String.appName);
            if (markerIdList == null)
            {
                markerIdList = new List<string>();
            }
            //check if marker is already open
            if (!markerIdList.Contains(markerID))
            {
                markerIdList.Add(markerID);
                isAlreadyOnList = false;
            }
            else
            {
                isAlreadyOnList = true;
            }
            //If there is something to show and fragment is not open
            if (markerIdList.Count != 0 && !isMarkerFragmentOpen)
            {
                btnShowNotifications.ClearAnimation();
                btnShowNotifications.Visibility = ViewStates.Visible;
                btnShowNotifications.Text = markerIdList.Count.ToString();
                //sheck the item position on the list
                int ItemPos = markerIdList.FindIndex(x => x == markerID);
                
                if (popfrag == null || popfrag.Context == null)
                {
                    //if fragment has focus vibrate short
                    if (HasWindowFocus)
                        PlayNotificationSound(vibrateShort);
                    else
                        PlayNotificationSound(vibrateLong);
                    //show popup fragment and pass the markerlist
                    popfrag = new PopUpRecycleFragment(this, markerIdList, ItemPos);        
                    popfrag.Show(SupportFragmentManager, Constants.PopUpRecycleFragment);
                    if (ItemPos == 0)
                        ItemPos = markerIdList.Count;
                }
                else
                {
                    //Make sure that fragment has context
                    popfrag.RequireActivity();
                    popfrag.RequireContext();
                    if (popfrag.IsDetached || popfrag.IsHidden)
                    {
                        popfrag.Show(SupportFragmentManager, Constants.PopUpRecycleFragment);
                                             
                    }
                    if (!isAlreadyOnList)
                    {
                        //if marker is not in the list, refresh fragment
                        await popfrag.RefreshList(markerID);
                    }
                    else
                    {
                        //SmootScroll list to already opened item
                        popfrag.SmoothScrollToItem(ItemPos);
                    }
                    PlayNotificationSound(vibrateShort);
                }
            }
            if (isMarkerFragmentOpen)
            {
                PlayNotificationSound(vibrateShort);
            }
            //Delete marker from preferences, it will popup again when onpause -> onresume if not deleted.
            SharedPreferences.SetMarkerId(null);
        }
        /// <summary>
        /// PlayVibration
        /// </summary>
        /// <param name="ms">how long phone vibrate in ms</param>
        private void PlayNotificationSound(long ms)
        {
            try
            {
                //Enable this if notification sound wanted
                //new System.Threading.Thread(new ThreadStart(() =>
                //{
                //    if (alarmUri == null)
                //        alarmUri = Android.Net.Uri.Parse("android.resource://" + Application.Context.PackageName + "/" + Resource.Raw.notification);
                //    if (ringTone == null)
                //        ringTone = RingtoneManager.GetRingtone(Application.Context, alarmUri);
                //    //ringTone.Volume = 0.5F;
                //    ringTone.Play();
                //})).Start();
                new System.Threading.Thread(new ThreadStart(() =>
                {
                    // If vibrator is null, get a new Vibrator object using GetSystemService()
                    if (vibrator == null)
                    {
                        vibrator = (Vibrator)Application.Context.GetSystemService(Context.VibratorService);
                    }

                    if ((int)Build.VERSION.SdkInt >= 26)
                        vibrator.Vibrate(VibrationEffect.CreateOneShot(ms, VibrationEffect.DefaultAmplitude));
                    else
                    {
                        // This method was deprecated in API 26, but must be used if there's a need to support < API 26
                        vibrator.Vibrate(ms);
                    }
                })).Start();

            }
            catch(System.Exception ex)
            {
                //ToDo handle errors
            }

        }
        /// <summary>
        /// Shows a warning dialog to the user, either warning about lack of GPS or Internet connection
        /// </summary>
        /// <param name="action"><see cref="Constants.GPSDialogAction"/> or <see cref="Constants.InternetDialogAction"/></param>
        public void ShowWarningDialog(string action)
        {
            if (action == Constants.GPSDialogAction)
            {
                // Create and show the new dialog
                new CustomDialogFragment(this,
                    GetString(Resource.String.dialogGpsTitle),
                    GetString(Resource.String.dialogGpsContent),
                    GetString(Resource.String.btnOpen),
                    GetString(Resource.String.btnClose),
                    Constants.GpsDialog)
                .Show(SupportFragmentManager, Constants.CustomDialogTag);
                //Set the static storage indicating that this dialog has already been sent to user
            }
            else if (action == Constants.InternetDialogAction)
            {
                // Create and show the new dialog
                new CustomDialogFragment(this,
                    GetString(Resource.String.dialogInternetTitle),
                    GetString(Resource.String.dialogInternetContent),
                    GetString(Resource.String.btnOpen),
                    GetString(Resource.String.btnClose),
                    Constants.NetworkDialog)
                .Show(SupportFragmentManager, Constants.CustomDialogTag);
                //Set the static storage indicating that this dialog has already been sent to suer
                StaticStorage.IsNetworkDialogShown = true;
            }
        }

        private void ClearFragmentStacks()
        {
            // Clear FragmentManager BackStack 
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                for (int i = 0; i < SupportFragmentManager.BackStackEntryCount; i++)
                {
                    SupportFragmentManager.PopBackStack();
                }
            }

            // Clear SupportFragmentManager BackStack 
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                for (int i = 0; i < SupportFragmentManager.BackStackEntryCount; ++i)
                {
                    SupportFragmentManager.PopBackStack();
                }
            }

        }

#endregion

#region Developer Methods
        private void ShowDeveloperUI_Click(object sender, EventArgs e)
        {
            RelativeLayout rlDeveloperUI = FindViewById<RelativeLayout>(Resource.Id.rlDeveloperUI);
            if (!rlDeveloperUI.IsShown)
            {
                rlDeveloperUI.Visibility = ViewStates.Visible;
            }
            else
            {
                rlDeveloperUI.Visibility = ViewStates.Invisible;
            }
        }

        private void ShowMenu_Click(object sender, EventArgs e)
        {
            mDrawerLayout.OpenDrawer(navigationView);
        }

        private void ShowLegend_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, Resource.String.errorFeatureNotInUse, ToastLength.Short).Show();
            //mDrawerLayout.OpenDrawer();
        }

        private void BtnDev1_Click(object sender, EventArgs e)
        {
            //Start Ruostejärven vesiluontopolku from Production DB
            StartTrail("B334F5A7-C605-434D-BB6C-18910EA94944");
        }

        private void BtnDev2_Click(object sender, EventArgs e)
        {
            Bundle bundle = MarkerFragment.GetBundle(
                "This is a title",
                "This is a descritpion",
                "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                "http://digitalspyuk.cdnds.net/15/48/1280x719/gallery-1448452218-rick-astley-never-gonna-give-you-up.jpg");
            ShowFragment(new MarkerFragment(), bundle);
        }

        private async void BtnDev3_Click(object sender, EventArgs e)
        {
            Bundle bundle = MarkerFragment.GetBundle(
                "This is a title",
                "This is a descritpion",
                "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                await new HttpRequest().GetImage("http://digitalspyuk.cdnds.net/15/48/1280x719/gallery-1448452218-rick-astley-never-gonna-give-you-up.jpg"));
            ShowFragment(new MarkerFragment(), bundle);
        }

        private async void BtnDev4_Click(object sender, EventArgs e)
        {
            //Fetch all marker from DB to listfragment
            try
            {
                SetLoadingAnimationState(true);
                List<Marker> markers = await AzureClient.GetClient().GetSyncTable<Marker>().ToListAsync();
                List<ListFragmentItem> items = new List<ListFragmentItem>();
                foreach (Marker marker in markers)
                {
                    MarkerTranslation translation = await AzureClient.GetMarkerTranslation(marker.Id, LocaleManager.GetLocale());
                    items.Add(new ListFragmentItem(translation.Title, translation.PopupDescription, marker.Id));
                }
                ShowFragment(new ListFragment(items), "");
            }
            catch (NullPointerException ex)
            {
                Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), ex.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), ex.Message, GetString(Resource.String.btnOk), null, null);
            }
            finally
            {
                SetLoadingAnimationState(false);
            }
        }

        private async void BtnDev5_Click(object sender, EventArgs e)
        {
            try
            {
                SetLoadingAnimationState(true);
                List<Marker> markers = await AzureClient.GetMarkersAroundPosition(new LatLng(locationPlugin.LastKnownLocation.Latitude, locationPlugin.LastKnownLocation.Longitude));
                List<ListFragmentItem> items = new List<ListFragmentItem>();
                foreach (Marker marker in markers)
                {
                    MarkerTranslation translation = await AzureClient.GetMarkerTranslation(marker.Id, LocaleManager.GetLocale());
                    items.Add(new ListFragmentItem(translation.Title, translation.PopupDescription, marker.Id));
                }
                ShowFragment(new ListFragment(items), "");
            }
            catch (NullPointerException ex)
            {
                Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), ex.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), ex.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (MobileServiceODataException ex)
            {
                Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), ex.Message, GetString(Resource.String.btnOk), null, null);
            }
            finally
            {
                SetLoadingAnimationState(false);
            }
        }

        private void BtnDev6_Click(object sender, EventArgs e)
        {
            ShowAlertDialog("Dev6", "This button doesn't do anything yet!", GetString(Resource.String.btnClose));
        }
#endregion

#region MapBoxMap Methods
    
        private void BtnBearing_Click(object sender, EventArgs e)
        {
            //Check if LocationLayerPlugin is initialized and if the mode is set to show bearing
            if (locationPlugin != null && locationPlugin.RenderMode == RenderMode.Compass)
            {
                //Show the normal icon
                btnBearing.SetBackgroundResource(Resource.Drawable.btnMapRotation);
                //Set the mode to normal tracking
                locationPlugin.LocationLayerEnabled = true;
                //Remove the compass listener
                locationPlugin.RemoveCompassListener(this);
                //Set the tilt to zero
                CameraPosition.Builder position = new CameraPosition.Builder();
                position.Tilt(0)
                    .Bearing(0);
                mapBoxMap.EaseCamera(CameraUpdateFactory.NewCameraPosition(position.Build()));
                //setting rendermode normal
                locationPlugin.RenderMode = RenderMode.Normal;

                //Reset the map to face north
                //mapBoxMap.ResetNorth();
            }
            else
            {
                //Show the icon resembling activated bearing tracking
                btnBearing.SetBackgroundResource(Resource.Drawable.btnMapRotationOn);
                if (locationPlugin != null)
                {
                    //Set LocationLayerPlugin to show user bearing
                    locationPlugin.RenderMode = RenderMode.Compass;
                    //Check if the device has a magnetic field sensor, a compass
                    if (deviceHasCompass)
                    {
                        //Add the compass listener
                        locationPlugin.AddCompassListener(this);
                    }
                    //Check if the user has seen the compass warning dialog
                    else if (!StaticStorage.IsCompassDialogShown)
                    {
                        //Warn the user that the device doesn't have a compass and will use GPS bearing
                        ShowAlertDialog(GetString(Resource.String.dialogAttentionTitle), GetString(Resource.String.dialogNoCompass), GetString(Resource.String.btnOk), null, Constants.CompassDialog);
                    }
                    //Tilt the camera
                    CameraPosition.Builder position = new CameraPosition.Builder();
                    position.Tilt(30);
                    mapBoxMap.EaseCamera(CameraUpdateFactory.NewCameraPosition(position.Build()));
                }
            }
        }

        private void BtnFinish_Click(object sender, EventArgs e)
        {
            ShowAlertDialog(GetString(Resource.String.dialogFinishTrailTitle), GetString(Resource.String.dialogFinishTrailContent),
                GetString(Resource.String.btnYes), GetString(Resource.String.btnCancel), Constants.FinishTrailDialog);

        }

        private void MapBoxMap_Scroll(object sender, EventArgs e)
        {
            if (isCamFollowingUser)
            {
                BearingBtnAnim(1000, false, false);
            }
            isCamFollowingUser = false;

            //Check if MapBox LocationLayerPlugin has been initialized and check it's render mode
            if (locationPlugin != null && locationPlugin.RenderMode != RenderMode.Normal)
                //Set the render mode to regular tracking
                locationPlugin.RenderMode = RenderMode.Normal;

            if (btnBearing != null)
                //Set the bearing icon to normal
                btnBearing.SetBackgroundResource(Resource.Drawable.btnMapRotation);
            if (btnClientLocation != null)
                btnClientLocation.SetBackgroundResource(Resource.Drawable.btnMyLocation);
        }

        private void BtnShowNotifications_Click(object sender, EventArgs e)
        {
            btnShowNotifications.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.fadeOutAnim));
            //if there is no markers to show hide notification button
            if (markerIdList != null && markerIdList.Count == 0)
            {
                btnShowNotifications.ClearAnimation();
                btnShowNotifications.Visibility = ViewStates.Invisible;
            }
            else
            {
                //Open popup fragment with last markerId
                ShowPopUpDialog(markerIdList.LastOrDefault());
            }
            btnShowNotifications.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.fadeInAnim));
        }
        private void BtnMyLocation_Click(object sender, EventArgs e)
        {
            btnClientLocation.SetBackgroundResource(Resource.Drawable.btnMyLocationEnabled);
            btnClientLocation.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.fadeOutAnim));

            // Is gps provider enabled
            if (!ServiceInformation.IsGpsEnabled())
            {
                ShowWarningDialog(Constants.GPSDialogAction);
            }

            // Is access to fine location granted
            if (!IsPermissionGranted(Manifest.Permission.AccessFineLocation))
            {
                RequestPermission(Manifest.Permission.AccessFineLocation, Constants.LocationRequestCode);
            }
            //Check if device's location has been acquired
            else if (LastKnownLocation() != null)
            {
                if (!isCamFollowingUser)
                {
                    BearingBtnAnim(1000, true, false);
                }
                isCamFollowingUser = true;
                // Move camera to device location
                SetCameratoMyLocation();
            }
            //If permission is granted and locationEngine is initialized but no location has yet been acquired
            else
            {
                Toast.MakeText(this, GetString(Resource.String.errorNoUserLocation), ToastLength.Short).Show();
            }

            btnClientLocation.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.fadeInAnim));
        }

        /// <summary>
        /// Callback for MapBoxMap
        /// </summary>
        /// <param name="p0">MapboxMap object</param>
        public async void OnMapReady(MapboxMap p0)
        {
            //Assign the initialzied map to field in the activity
            mapBoxMap = p0;
            //Handling mapbox events
            mapBoxMap.MapClick += MapBoxMap_MapClick;
            mapBoxMap.Scroll += MapBoxMap_Scroll;
            mapBoxMap.UiSettings.CompassEnabled = true;
            mapBoxMap.UiSettings.RotateGesturesEnabled = true;
            mapBoxMap.UiSettings.ScrollGesturesEnabled = true;
            mapBoxMap.UiSettings.TiltGesturesEnabled = true;
            mapBoxMap.UiSettings.SetCompassFadeFacingNorth(true);

            //Enable LocationLayerPlugin to show user location on the map
            EnableLocationPlugin();

            // Set the max and min for camera zoom, min 1 max 22. Min 1 = WORLD : Max 18 = buildings
            mapBoxMap.SetMinZoomPreference(5);
            mapBoxMap.SetMaxZoomPreference(19);

            //Make sure that AzureClient is initialized by this point
            if (!AzureClient.IsInitialized())
                await AzureClient.Initialize();

            //Fetch default shown markers from DB and set them to the map
            SetMarkers(await AzureClient.GetDefaultShownMarkers());
            if (LastKnownLocation() != null)
            {
                //If there is last known location, Set the camera to user's location
                SetCameratoMyLocation();
            }
        }

        /// <summary>
        /// Handle mapbox mapclick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapBoxMap_MapClick(object sender, MapboxMap.MapClickEventArgs e)
        {
            PointF screenPoint = mapBoxMap.Projection.ToScreenLocation(e.P0);
            List<Feature> features = mapBoxMap.QueryRenderedFeatures(screenPoint, symbolLayerName).ToList();
            List<Feature> circleFeatures = mapBoxMap.QueryRenderedFeatures(screenPoint, clusterLayerName).ToList();
            //zoom to cluster group

            if (circleFeatures.Count == 0)
            {
                if (features.Count != 0)
                {
                    Feature selectedFeature = features.FirstOrDefault();
                    string title = selectedFeature.GetStringProperty("title");
                    string id = selectedFeature.GetStringProperty("markerid");
                    ShowPopUpDialog(id);
                }
                SetCameraTo(new LatLng(e.P0.Latitude, e.P0.Longitude), mapBoxMap.CameraPosition.Zoom);
            }
            else
            {
                SetCameraTo(new LatLng(e.P0.Latitude, e.P0.Longitude), 16);
            }
        }

        /// <summary>
        /// Used to enable MapBox LocationLayerPlugin that shows the user's location as a view on the map.
        /// This method also initializes LocationEngine in order to fetch location data
        /// </summary>
        private void EnableLocationPlugin()
        {
            //Check that we have location permissions
            if (!IsPermissionGranted(Manifest.Permission.AccessFineLocation))
            {
                RequestPermission(Manifest.Permission.AccessFineLocation, Constants.LocationRequestCode);
            }
            else
            {
                //Check if LocationEngine needs to be initialized
                InitializeLocationEngine();

                //Check that all required parameters aren't null
                if (mapView != null && mapBoxMap != null && locationEngine != null)
                    locationPlugin = new LocationLayerPlugin(mapView, mapBoxMap, locationEngine);
                //if(locationPlugin != null)
                //Set the LocationLayerPlugin enabled and set mode to Tracking
                locationPlugin.RenderMode = RenderMode.Normal;
                locationPlugin.AddCompassListener(this);               
            }
        }

        /// <summary>
        /// Used to enable MapBox LocationEngine to track user location
        /// </summary>
        private void InitializeLocationEngine()
        {
            //Check that we have location permission
            if (!IsPermissionGranted(Manifest.Permission.AccessFineLocation))
            {
                RequestPermission(Manifest.Permission.AccessFineLocation, Constants.LocationRequestCode);
            }
            else
            {
                //Initialize the LocationEngine
                locationEngine = new LocationEngineProvider(this).ObtainBestLocationEngineAvailable();
                //Set the LocationEngine priority to High Accuracy
                locationEngine.Priority = LocationEnginePriority.HighAccuracy;
                //Activate the LocationEngine
                locationEngine.Activate();
                //Add a listener for tracking a locationchange
                locationEngine.AddLocationEngineListener(this);
            }
        }

        /// <summary>
        /// Callback for LocationEngine
        /// </summary>
        public void OnConnected()
        {
            if (locationEngine != null)
                //Start requesting location updates from the LocationEngine
                locationEngine.RequestLocationUpdates();
        }

        /// <summary>
        /// Callback for MapBox when device location changes
        /// </summary>
        /// <param name="p0">Latest location of the device</param>
        public void OnLocationChanged(Location p0)
        {
            if (p0 != null && mapView != null && mapBoxMap != null && !IsFinishing)
            {
                //Check the trail status
                CheckTrailGuide(p0);
   
                //Check if user is being currently tracked
                if (isCamFollowingUser)
                    //Set camera to user's location
                    SetCameratoMyLocation();
                //set notification button invisible if there is no notifications
                if (markerIdList != null && markerIdList.Count == 0)
                {
                    btnShowNotifications.ClearAnimation();
                    btnShowNotifications.Visibility = ViewStates.Invisible;
                }

            }
        }

        /// <summary>
        /// Callback for MapBox when the device's compass accuracy changes
        /// </summary>
        /// <param name="p0">Compass status identical to <see cref="SensorManager" /> class constants</param>
        public void OnCompassAccuracyChange(int p0)
        {
            //For debug purposes
            switch ((SensorStatus)p0)
            {
                case SensorStatus.Unreliable:
                    tvDev1.Text = "Compass: Unreliable";
                    break;

                case SensorStatus.NoContact:
                    tvDev1.Text = "Compass: NoContact";
                    break;

                case SensorStatus.AccuracyLow:
                    tvDev1.Text = "Compass: Low";
                    break;

                case SensorStatus.AccuracyMedium:
                    tvDev1.Text = "Compass: Medium";
                    break;

                case SensorStatus.AccuracyHigh:
                    tvDev1.Text = "Compass: High";
                    break;

                default:
                    tvDev1.Text = "No status";
                    break;
            }
        }

        /// <summary>
        /// Callback for when device's compass updates a new bearing
        /// </summary>
        /// <param name="p0">The new compass heading</param>
        public void OnCompassChanged(float p0)
        {

            //Check if the current camera bearing matches the new bearing and if the tracking mode for LocationLayerPlugin is set to Compass
            if (mapBoxMap.CameraPosition.Bearing != p0 && locationPlugin.RenderMode == RenderMode.Compass && mapBoxMap != null)
            {
                //Set the camera to match the latest bearing
                CameraPosition.Builder position = new CameraPosition.Builder();
                position.Bearing(p0);
                mapBoxMap.EaseCamera(CameraUpdateFactory.NewCameraPosition(position.Build()));
            }
        }

        /// <summary>
        /// Hide the bearing button by fading it out and then changing it's visibility.
        /// Also stops camera tracking user.
        /// </summary>
        private void HideBearingButtonAndStopTracking()
        {
            if (isCamFollowingUser)
            {
                BearingBtnAnim(1000, false, false);
            }
            isCamFollowingUser = false;

            //Check if MapBox LocationLayerPlugin has been initialized and check it's render mode
            if (locationPlugin != null && locationPlugin.RenderMode != RenderMode.Normal)
                //Set the render mode to regular tracking
                locationPlugin.RenderMode = RenderMode.Normal;

            if (btnBearing != null)
                //Set the bearing icon to normal
                btnBearing.SetBackgroundResource(Resource.Drawable.btnMapRotation);

            if (mapBoxMap != null)
            {
                //Set the tilt to zero
                CameraPosition.Builder position = new CameraPosition.Builder();
                position.Tilt(0)
                    .Bearing(0);
                mapBoxMap.EaseCamera(CameraUpdateFactory.NewCameraPosition(position.Build()));
            }
            if (btnClientLocation != null)
                btnClientLocation.SetBackgroundResource(Resource.Drawable.btnMyLocation);
        }

        /// <summary>
        /// Check for device's last known location. If no location is found, null is returned
        /// </summary>
        /// <returns>Last known device location or null</returns>
        private Location LastKnownLocation()
        {
            //Check if LocationLayerPlugin has a location
            if (locationPlugin != null && locationPlugin.LastKnownLocation != null)
            {
                return locationPlugin.LastKnownLocation;
            }
            //Check if LocationEngine has a location
            else if (locationEngine != null && locationEngine.LastLocation != null)
            {
                return locationEngine.LastLocation;
            }
            //Check if RangeService has reported a location
            else if (lastRequestLocation != null)
                return lastRequestLocation;
            //Return null indicating that we couldn't find a location
            else
                return null;
        }

        /// <summary>
        /// SetCameraTo last known user location
        /// </summary>
        public void SetCameratoMyLocation()
        {
            Location location = LastKnownLocation();
            //Check that MapBox LocationEngine has been initialized and the engine has acquired a location
            if (location != null && locationPlugin != null)
            {
                //Check that the camera isn't already at the same spot
                if (mapBoxMap.CameraPosition.Target != new LatLng(location))
                {
                    //Check if currently camera zoom is less or equal to 14, meaning the camera is far away
                    if (mapBoxMap.CameraPosition.Zoom <= 14)
                        //Zoom the camera closer to the user location
                        mapBoxMap.CameraPosition.Zoom = 14;

                    //Create new camera position object
                    CameraPosition.Builder position = new CameraPosition.Builder();
                    //Check if device doesn't have a compass and LocationPlugin render mode has been set to compass
                    if (!deviceHasCompass && locationPlugin.RenderMode == RenderMode.Compass)
                    {
                        //Device doesn't have a compass, set bearing according to GPS data
                        position
                            .Target(new LatLng(location))
                            .Zoom(mapBoxMap.CameraPosition.Zoom)
                            .Bearing(location.Bearing)
                            .Tilt(mapBoxMap.CameraPosition.Tilt);

                        //Call event method, as there is no compass sensor to call for this event
                        OnCompassChanged(location.Bearing);
                    }

                    else
                        //Device has a compass sensor, bearing is going to be set by the compass listener
                        position
                            .Target(new LatLng(location))
                            .Zoom(mapBoxMap.CameraPosition.Zoom)
                            .Tilt(mapBoxMap.CameraPosition.Tilt);

                    //Set the new camera position and animate the movement of the camera
                    mapBoxMap.EaseCamera(CameraUpdateFactory.NewCameraPosition(position.Build()));

                }
            }
            else
            {
                var lat = SharedPreferences.GetLatitude();
                var lng = SharedPreferences.GetLongitude();

                if (lat != 0.0F && lng != 0.0F)
                {
                    SetCameraTo(new LatLng(lat, lng), 14);
                }
 
            }
        }

        public void SetCameraTo(LatLng latlang, double zoomLevel)
        {
            if (isCamFollowingUser)
            {
                BearingBtnAnim(1000, false, false);
            }
            isCamFollowingUser = false;

            //Check if MapBox LocationLayerPlugin has been initialized and check it's render mode
            if (locationPlugin != null && locationPlugin.RenderMode != RenderMode.Normal)
                //Set the render mode to regular tracking
                locationPlugin.RenderMode = RenderMode.Normal;
            //Set the bearing and location icon to normal
            if (btnBearing != null)
                btnBearing.SetBackgroundResource(Resource.Drawable.btnMapRotation);
            if (btnClientLocation != null)
                btnClientLocation.SetBackgroundResource(Resource.Drawable.btnMyLocation);

            //Create new camera position
            CameraPosition.Builder position = new CameraPosition.Builder();
            position.Target(latlang) // Sets the new camera position
                .Zoom(zoomLevel)
                .Bearing(0)
                .Tilt(0);
            mapBoxMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(position.Build()), 3000);
        }
#endregion

#region Trails & Themes
        /// <summary>
        /// Update distancemeter and elevation chartview based on location.
        /// </summary>
        /// <param name="location">Users known loaction</param>
        private void CheckTrailGuide(Location location)
        {
            try
            {
                //Get only relevant/nearby markers
                GetNearbyMarkers(location);
                //if there is trail and it is active
                if (trailPoints != null && StaticStorage.IsTrailActive)
                {
                    const int difficulty = 1; //not relevant
                    const int distanceToReact = 80; // If user is 80m off track, notify user
                    //Store trail guide results
                    new TrailGuide(location, trailPoints, distanceToReact, difficulty).CheckTrailResult();
                    //handle elevation chart changes
                    if (HasWindowFocus)
                    {
                        ChartManager.UpdateLocationDataSet((float)TrailResult.CalculatedDistanceToStart / 1000, (float)trailPoints[TrailResult.NearestTrailPointIndex].Altitude);
                    }
                    //get trail guide results to distance meter
                    string tvDistanceMeterText;
                    double unitsToStart = TrailResult.CalculatedDistanceToStart;
                    double unitsToEnd = TrailResult.CalculatedDistanceToEnd;
                    string unitEnd = "m";
                    string unitStart = "m";
                    //Round units
                    if (unitsToStart > 500)
                    {
                        unitsToStart = System.Math.Round(unitsToStart / 1000, 1);
                        unitStart = "km";
                    }
                    if (unitsToEnd > 500)
                    {
                        unitsToEnd = System.Math.Round(unitsToEnd / 1000, 1);
                        unitEnd = "km";
                    }
                    //If user is on range and trail is started start calculate
                    if (TrailResult.IsTrailStarted||TrailResult.UserOnRange)
                    {
                        tvDistanceMeterText = unitsToStart + unitStart + " | " + unitsToEnd + unitEnd;
                        ivFinishTrailMarker.Visibility = ViewStates.Visible;
                    }
                    //If user is too faraway from starting point, calculate distance to startpoint (beeline)
                    else
                    {
                        float[] floatMeters = new float[2];
                        double latitude = LastKnownLocation().Latitude;
                        double longitude = LastKnownLocation().Longitude;
                        double firstPointLat = trailPoints.FirstOrDefault().Latitude;
                        double firstPointLon = trailPoints.FirstOrDefault().Longitude;
                        Location.DistanceBetween(latitude, longitude, firstPointLat, firstPointLon, floatMeters);
                        ivStartTrailMarker.Visibility = ViewStates.Visible;
                        ivFinishTrailMarker.Visibility = ViewStates.Invisible;
                        double meterstoStart = double.Parse(floatMeters.FirstOrDefault().ToString());
                        unitStart = " m";
                        if (meterstoStart > 500)
                        {
                            meterstoStart = System.Math.Round(meterstoStart / 1000, 1);
                            unitStart = " km";
                        }
                        else
                        {
                            meterstoStart = System.Math.Round(meterstoStart, 0);
                        }
                        tvDistanceMeterText = "<-- " + meterstoStart + unitStart;
                    }
                    tvDistanceMeter.Text = tvDistanceMeterText;
                    cardViewDistanceMeter.Visibility = ViewStates.Visible;
                    if (!TrailResult.IsTrailFinished)
                    {
                        // If user is going back, display alert dialog one time
                        if (TrailResult.BackCounter >= 3 && !TrailResult.GoingBackDialogSent)
                        {
                            //ShowAlertDialog("Heading back?", "Seems like you are going back, keep walking to start calculate time back to start", "Ok");
                            TrailResult.GoingBackDialogSent = true;
                        }

                        // if user has started trail and goes off track, display alert dialog one time
                        if (!TrailResult.UserOnRange && TrailResult.IsTrailStarted && !TrailResult.LostDialogSent)
                        {
                            ShowAlertDialog(GetString(Resource.String.dialogUserLostTitle), GetString(Resource.String.dialogUserLostContent), GetString(Resource.String.btnOk), null, null);
                            TrailResult.LostDialogSent = true;
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (ArgumentNullException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (NullPointerException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (UriFormatException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (IllegalStateException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (ArgumentException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (NullReferenceException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
        }

        /// <summary>
        /// Fetch TrailMarkers for a Trail and add them to the map, fetch a GPX file and draw a polyline from it
        /// </summary>
        /// <param name="trailID">ID of the selected Trail</param>
        public async void StartTrail(string trailID)
        {
            try
            {
                // Start the loading animation
                SetLoadingAnimationState(true);
                // Fetch the Trail from DB
                Trail trail = await AzureClient.GetTrail(trailID);
                // Reset TrailResult
                TrailResult.ResetResult();
                //Start figuring out the filename of the GPX file
                int pos = trail.File.LastIndexOf("/");
                //Define the path of the file in the filesystem
                string path = FilesDir.AbsolutePath + trail.File.Substring(pos, trail.File.Length - pos);
                //Check that the ID given is not null
                if (string.IsNullOrEmpty(trailID))
                {
                    throw new NullPointerException(GetString(Resource.String.dbErrorTrailIdNullOrEmpty));
                }

                // Check if we have location permission
                if (!IsPermissionGranted(Manifest.Permission.AccessFineLocation))
                {
                    //Warn the user that we may not be able to guide the user along the trail without location data
                    ShowAlertDialog(GetString(Resource.String.dialogAttentionTitle), GetString(Resource.String.dialogGpsAttentionContent), GetString(Resource.String.btnOk), null, null);
                }

                // Make sure this activity is visible
                ClearFragmentStacks();
                // Show the map UI elements
                ShowMapUIElements();

                // If user has alredy selected a trail, finish it first by removing it's markers and polyline first
                if (StaticStorage.IsTrailActive)
                {
                    //IF user had an active theme, remove it's markers
                    if (StaticStorage.IsThemeActive)
                        FinishTheme();

                    //Remove markers of the last trail from the map
                    AddMarkerListToMap(await AzureClient.GetDefaultShownMarkers());
                    //Clear the height chart
                    ChartManager.ClearChart();

                    //Loop through existing polylines on the map
                    foreach (Polyline polyline in mapBoxMap.Polylines)
                    {
                        //Check if any of the polylines match with the saved ID
                        //if (SharedPreferences.GetPolylineID() == polyline.Id)
                        //Remove the polyline
                        mapBoxMap.RemovePolyline(polyline);
                    }
                }

                //Set the trail ID to preferences
                SharedPreferences.SetLastTrail(trailID);
                //Indicate that a trail has been activated
                StaticStorage.IsTrailActive = true;

                /*Start a new background task to fetch the GPX file.
                    * The process may take a variable amount of time, so we don't want
                    * to lock up the UI thread while waiting for the process to finish*/
                Task getGPX = Task.Run(async () =>
                {
                    FileCheckHelper fch = new FileCheckHelper();
                    bool IsFileOlderThan = fch.IsFileOlderThan(path, -1);

                    //Check if we have an active internet connection and DownloadTask is running
                    if (ServiceInformation.IsConnectionActive() && DownloadTask.IsTaskRunning && IsFileOlderThan)
                    {
                        //Cancel the downloadtask
                        DownloadTask.TokenSource.Cancel();
                        //Fetch the GPX file from Internet
                        string content = await new HttpRequest().GetString(trail.File);
                        //Check if the list is populated
                        if (content != null && !string.IsNullOrEmpty(content))
                        {
                            //Write the GPX file into the memory of the device
                            File.WriteAllText(path, content);
                        }
                        //Decode the content to the list
                        trailPoints = new DecodeTask().DecodeGpxById(trailID);
                        //Restart the download task
                        DownloadTrailFiles();
                    }
                    //If DownloadTask is not running but active connection is present
                    else if (ServiceInformation.IsConnectionActive() && IsFileOlderThan)
                    {
                        //Fetch the GPX file from Internet
                        string content = await new HttpRequest().GetString(trail.File);
                        //Check if the list is populated
                        if (content != null && !string.IsNullOrEmpty(content))
                        {
                            //Write the GPX file into the memory of the device
                            File.WriteAllText(path, content);
                        }
                        //Decode the content to the list
                        trailPoints = new DecodeTask().DecodeGpxById(trailID);
                    }
                    //if file are old, download trailpoints
                    else if (!IsFileOlderThan)
                    {
                        trailPoints = new DecodeTask().DecodeGpxById(trailID);
                    }
                });
                //Wait until the GPX file is fetched
                await getGPX;

                //Draw a polyline from the list of trailpoints, and store the float ID of the drawn polyline in preferences
                //SharedPreferences.SetPolylineID(MapDrawer.DrawPolyLine(trailPoints, mapBoxMap, 5, ContextCompat.GetColor(this, Resource.Color.mapbox_blue)).Id);
                if (trailPoints == null)
                    await getGPX;
                //stop follow user
                HideBearingButtonAndStopTracking();
                // Sets polyline options
                PolylineOptions options = new PolylineOptions().InvokeWidth(5).InvokeColor(ContextCompat.GetColor(this, Resource.Color.mapbox_blue));
                MapDrawer md = new MapDrawer(this);
                md.DrawPolyLine(options, trailPoints, mapBoxMap, trailID);
                //Show elevation chart
                ShowTrailElevation(trailPoints);

                //Add markers to list of tracked markers
                if (LastKnownLocation() != null)
                    nearbyMarkers.AddRange(await AzureClient.GetTrailMarkersAroundPosition(trailID, new LatLng(LastKnownLocation())));

                //Set the trail ID to preferences
                SharedPreferences.SetLastTrail(trailID);
                //Make the finish trail button visible
                btnFinish.Visibility = ViewStates.Visible;
                //Indicate that a trail has been activated
                StaticStorage.IsTrailActive = true;

                //Hide layers from the map
                //Loop through all the known layers
                foreach (string layerId in Constants.MapBoxMapLayers)
                {
                    //Fetch the layer from the map
                    Layer layer = mapBoxMap.GetLayer(layerId);
                    //Check that the layer actually exists
                    if (layer != null)
                    {
                        //Set the visibility of the layer to none
                        layer.SetProperties(PropertyFactory.Visibility(Property.None));
                    }
                }
                //Show distance to route start point, just backup if there is no current location
                float[] floatMeters = new float[2];
                double meterstoStart = 0;
                if (LastKnownLocation() != null)
                {
                    Location.DistanceBetween(LastKnownLocation().Latitude, LastKnownLocation().Longitude, trailPoints.First().Latitude, trailPoints.First().Longitude, floatMeters);
                    meterstoStart = double.Parse(floatMeters.FirstOrDefault().ToString());
                }
                string unitStart = " m";
                if (meterstoStart > 500)
                {
                    meterstoStart = System.Math.Round(meterstoStart / 1000, 1);
                    unitStart = " km";
                }
                else
                {
                    meterstoStart = System.Math.Round(meterstoStart, 0);
                }
                tvDistanceMeter.Text = "<-- " + meterstoStart + unitStart;

                //Try to fetch a default theme for the trail
                Theme defaulTheme = await AzureClient.GetDefaultTheme(trailID);
                //Check if the returned theme is not null
                if (defaulTheme != null)
                    //Start the default theme, TrailMarkers and ThemeMarkers are going to be set in StartTheme
                    StartTheme(defaulTheme.Id);
                //Trail doesn't have default theme, set TrailMarkers for the trail
                else
                {
                    //Fetch a list of shown TrailMarkers to the map
                    List<Marker> markers = await AzureClient.GetShownTrailMarkers(trailID);
                    // If the Trail doesn't have TrailMarkers, no need to clear the map and
                    // re-add the default markers which should be on the map already
                    if (markers != null && markers.Count > 0)
                    {
                        //Add default shown markers
                        markers.AddRange(await AzureClient.GetDefaultShownMarkers());
                        //Add the list to the map
                        AddMarkerListToMap(markers);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (ArgumentNullException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (NullPointerException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (UriFormatException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (IllegalStateException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (ArgumentException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (NullReferenceException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (WebException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (AggregateException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            finally
            {
                SetLoadingAnimationState(false);
            }
        }

        /// <summary>
        /// Remove Markers of the last active Trail from map and remove polyline of the map
        /// </summary>
        private async Task FinishTrail()
        {
            try
            {
                if (string.IsNullOrEmpty(SharedPreferences.GetLastTrail()))
                    throw new IllegalStateException("No trail to be finished!");
                currentRoute = "";
                SetLoadingAnimationState(true);
                //If user has activated a theme, make sure to finish it too
                if (StaticStorage.IsThemeActive)
                    FinishTheme();
                else
                    //Remove markers from the map and add defaults
                    AddMarkerListToMap(await AzureClient.GetDefaultShownMarkers());

                //Remove markers from list of tracked markers
                nearbyMarkers.Clear();


                //Go through polylines drawn on the MapBoxMap
                foreach (Polyline polyline in mapBoxMap.Polylines)
                {
                    //Remove the polyline
                    mapBoxMap.RemovePolyline(polyline);
                }

                //Add layers back to the map
                //Loop through all the known layers
                foreach (string layerId in Constants.MapBoxMapLayers)
                {
                    //Fetch the layer from the map
                    Layer layer = mapBoxMap.GetLayer(layerId);
                    if (layer != null)
                    {
                        //Set the visibility of the layer to visible
                        layer.SetProperties(PropertyFactory.Visibility(Property.Visible));
                    }
                }

                //Make the trail finish button invisible again
                btnFinish.Visibility = ViewStates.Invisible;
                //Hide the distance meter
                cardViewDistanceMeter.Visibility = ViewStates.Invisible;
                //Indicate that a trail is no longer active
                StaticStorage.IsTrailActive = false;
                ChartManager.ClearChart();
                HideAndCloseChart();
            }
            catch (ArgumentNullException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (NullPointerException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (IllegalStateException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (ArgumentException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (NullReferenceException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            finally
            {
                SetLoadingAnimationState(false);
            }
        }

        /// <summary>
        /// Fetch Markers for the Theme and place them on the map
        /// </summary>
        /// <param name="themeID">ID of the Theme</param>
        private async void StartTheme(string themeID)
        {
            try
            {
                if (themeID == null)
                    throw new ArgumentNullException("Cannot start theme! Theme ID is null!");

                //Check if we have an active theme
                if (StaticStorage.IsThemeActive)
                    //Finish before starting a new one
                    FinishTheme();

                //Indicate that theme has been activated
                StaticStorage.IsThemeActive = true;
                //Set the Theme ID to preferences
                SharedPreferences.SetLastTheme(themeID);
                //Fetch all shown Markers
                List<Marker> markers = await AzureClient.GetDefaultShownMarkers();
                //Add list of ThemeMarkers from DB
                markers.AddRange(await AzureClient.GetShownThemeMarkers(themeID));
                //Check if the Theme has an Objective
                if (await AzureClient.ThemeHasObjective(themeID))
                {
                    if (objectiveController == null)
                        //Initialize new ObjectiveController
                        objectiveController = new ObjectiveController(await AzureClient.GetObjectiveThemeMarkers(themeID));
                    //Fetch ObjectiveThemeMarkers and add them to list of Markers
                    markers.AddRange(await AzureClient.GetShownObjectiveThemeMarkers(themeID));
                    //Add markers to list of tracked markers if user location is known
                    if (LastKnownLocation() != null)
                        nearbyMarkers.AddRange(await AzureClient.GetObjectiveThemeMarkersAroundPosition(themeID, new LatLng(LastKnownLocation())));
                    //Indicate that an Objective is active
                    StaticStorage.IsObjectiveActive = true;
                    //Fetch a ThemeObjective
                    ThemeObjective objective = await AzureClient.GetThemeObjective(themeID);
                    //Set objective ID to preferences
                    SharedPreferences.SetLastObjective(objective.Id);
                }
                //TODO:
                // TrailMarkers should only be set in StartTrail  this is a fix as the AddMarkerListToMap function
                // clears all markers from the map, so they need to be added to the list of markers that is going to be added to the map
                if (!string.IsNullOrEmpty(SharedPreferences.GetLastTrail()))
                    markers.AddRange(await AzureClient.GetShownTrailMarkers(SharedPreferences.GetLastTrail()));
                //Add list of markers to the map
                AddMarkerListToMap(markers);
                //Add markers to list of tracked markers if user location is known
                if (LastKnownLocation() != null)
                    nearbyMarkers.AddRange(await AzureClient.GetThemeMarkersAroundPosition(themeID, new LatLng(LastKnownLocation())));
            }
            catch (NullPointerException ex)
            {
                Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.errorThemeStart) + "\n" + ex.Message, GetString(Resource.String.btnClose), null, null);
            }
            catch (ArgumentNullException ex)
            {
                Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.errorThemeStart) + "\n" + ex.Message, GetString(Resource.String.btnClose), null, null);
            }
        }

        /// <summary>
        /// Remove Markers of the Theme from the map
        /// </summary>
        private async void FinishTheme()
        {
            //Indicate that Theme is no longer active
            StaticStorage.IsThemeActive = false;
            //Indicate that an Objective for the theme is no longer active
            StaticStorage.IsObjectiveActive = false;
            /* TODO: 
             * We shouldnt have to null objects, instead creating a new one when needed.
             * However, if the app lifecycle requires the activity to go to onSaveInstance and back to
             * onCreate, we currently have to null objectiveController in order for a new Objective
             * to start normally.
            */
            //Set the objectiveController to null
            objectiveController = null;
            //Clear the map and add defaults to it
            AddMarkerListToMap(await AzureClient.GetDefaultShownMarkers());
        }
        /// <summary>
        /// Show trail elevation on chartview
        /// </summary>
        /// <param name="latLngs">list of lat and lng</param>
        private void ShowTrailElevation(List<LatLng> latLngs)
        {
            //TODO: scale shart, now looks weird if height interval is a few meters, should be flat line
            // Find the main and line chart views
            View view = FindViewById(Resource.Id.frameLayout1);
            ChartManager.LineChart = (LineChart)FindViewById(Resource.Id.lineChart);

            // Get left right y axes and x axis
            ChartManager.LeftAxis = ChartManager.LineChart.AxisLeft;
            ChartManager.RightAxis = ChartManager.LineChart.AxisRight;
            ChartManager.XAxis = ChartManager.LineChart.XAxis;

            float width = view.Width * 0.5F;
            float height = view.Width * 0.5F * 0.66F;

            // Calculate default width and height for chart view
            ChartManager.SetChartWidthAndHeight((int)width, (int)height);

            // Simple calls to set prefered stylings to chart and axes. You can change these in ChartManager Class
            ChartManager.InitChartStyling();
            ChartManager.InitAxisStyling();

            // Entry list for chart entries
            Entry[] entries = new Entry[latLngs.Count];

            // TODO: Remove when  total distance can be aquired elsewhere >>>>
            // Distance calculator for debuggin
            float[] distance = new float[2];
            float totalDistance = 0;
            for (int i = 0; i < latLngs.Count - 1; i++)
            {
                Location.DistanceBetween(latLngs[i].Latitude, latLngs[i].Longitude, latLngs[i + 1].Latitude, latLngs[i + 1].Longitude, distance);
                totalDistance += distance.FirstOrDefault();
            }
            // <<<<

            // Variable to help calculate x coordinates
            float currentDistance = 0;
            // Variable to help crudely check if Altitude values are available
            int zeroCount = 0;

            for (int i = 0; i < latLngs.Count; i++)
            {
                // Atleast for now we just asume that the value of altitude cannot be 0, so add one to count if value is 0
                if (latLngs[i].Altitude == 0)
                {
                    zeroCount++;
                }
                // Create new entry object from data list and add created entry to entries list
                entries[i] = new Entry(currentDistance / 1000, (float)latLngs[i].Altitude);
                currentDistance += totalDistance / latLngs.Count;
            }

            // Create new entry to be shown as tracker. Set first value of entries as default
            Entry[] entry = new Entry[1] { entries.FirstOrDefault() };

            // If there were no altitude with value 0
            if (zeroCount == 0)
            {
                // Create new data set for entries
                ChartManager.ElevationSet = new LineDataSet(entries, "elevation");
                // Create new data set for tracker entry
                ChartManager.LocationSet = new LineDataSet(entry, "location");
                // SetDataSetStyling
                ChartManager.InitDataSetStylings();

                // Create and add line data to chart
                ChartManager.CreateAndSetLineChartData(new LineDataSet[] { ChartManager.ElevationSet, ChartManager.LocationSet });
            }

            // Get prefered state of chart view 
            if (SharedPreferences.GetLineChartVisibilityState())
            {
                // Show view
                chartView.Visibility = ViewStates.Visible;
            }
            else
            {
                // Show a button that opens view when pressed
                showChart.Visibility = ViewStates.Visible;
            }
        }

        private void HideAndCloseChart()
        {
            // Hide parent view containing all buttons and line chart
            chartView.Visibility = ViewStates.Gone;
            // Set expande and minimize buttons to default states 
            // Note! expande button will not be shown because it's parent is hidden
            expandeChart.Visibility = ViewStates.Visible;
            minimizeChart.Visibility = ViewStates.Gone;

            // Shrink chart back to it's original size
            ChartManager.SetExpandedState(false);

            // showChart button is only visible if user has hid the chart 
            if (showChart.Visibility == ViewStates.Visible)
            {
                // Hide showChart button when it's not needed
                showChart.Visibility = ViewStates.Gone;
            }

            // Show showChart button only if trail is active
            if (StaticStorage.IsTrailActive)
            {
                showChart.Visibility = ViewStates.Visible;
            }
        }

        private void ShowChart()
        {
            chartView.Visibility = ViewStates.Visible;
            showChart.Visibility = ViewStates.Gone;
            cardViewDistanceMeter.Visibility = ViewStates.Visible;
            if (markerIdList != null)
            {
                if (markerIdList.Count != 0)
                {
                    btnShowNotifications.ClearAnimation();
                    btnShowNotifications.Visibility = ViewStates.Visible;
                }
            }
            // Store visibility state to preference so that chart is shown every time when trail is started
            SharedPreferences.SetLineChartVisibilityState(true);
        }

        private void HideChart_Click(object sender, EventArgs e)
        {
            cardViewDistanceMeter.Visibility = ViewStates.Visible;
            if (markerIdList != null)
            {
                if (markerIdList.Count != 0)
                {
                    btnShowNotifications.ClearAnimation();
                    btnShowNotifications.Visibility = ViewStates.Visible;
                }
            }
            HideAndCloseChart();
            // Store visibility state to preference so that chart is hidden when trail is started
            SharedPreferences.SetLineChartVisibilityState(false);
        }

        private void MinimizeChart_Click(object sender, EventArgs e)
        {
            minimizeChart.Visibility = ViewStates.Gone;
            expandeChart.Visibility = ViewStates.Visible;
            cardViewDistanceMeter.Visibility = ViewStates.Visible;
            if (markerIdList != null)
            {
                if (markerIdList.Count != 0)
                {
                    btnShowNotifications.ClearAnimation();
                    btnShowNotifications.Visibility = ViewStates.Visible;
                }
            }

            // Minimize chart by setting it's expanded state false
            ChartManager.SetExpandedState(false);
        }

        private void ExpandeChart_Click(object sender, EventArgs e)
        {
            minimizeChart.Visibility = ViewStates.Visible;
            expandeChart.Visibility = ViewStates.Gone;
            cardViewDistanceMeter.Visibility = ViewStates.Invisible;
            btnShowNotifications.ClearAnimation();
            btnShowNotifications.Visibility = ViewStates.Invisible;
            // Expande chart by setting it's expanded state true
            ChartManager.SetExpandedState(true);
        }

        private void ShowChart_Click(object sender, EventArgs e)
        {
            cardViewDistanceMeter.Visibility = ViewStates.Visible;
            if (markerIdList != null)
            {
                if (markerIdList.Count != 0)
                {
                    btnShowNotifications.ClearAnimation();
                    btnShowNotifications.Visibility = ViewStates.Visible;
                }
            }
            ShowChart();
        }
#endregion

#region Marker Management
        /// <summary>
        /// Set markers to map
        /// </summary>
        /// <param name="markerList">List of markers</param>
        public void SetMarkers(List<Marker> markerList)
        {
            //Check the list
            if (markerList == null || markerList.Count <= 0)
            {
                ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.dbErrorMarkerListEmptyError), GetString(Resource.String.btnClose));
            }
            else
            {
                AddMarkerListToMap(markerList);
            }
        }

        /// <summary>
        /// Fetch an icon from resources matching the filename. If a matching icon can't be found, the function returns the default marker icon.
        /// </summary>
        /// <param name="filename">Filename of the desired icon, excluding file extension</param>
        /// <returns>Marker icon from resources</returns>
        private Icon GetIcon(string filename)
        {
            //Variable for marker icon
            Icon markerIcon = null;
            //Iconfactory for fetching an icon from resources
            IconFactory iconFactory = IconFactory.GetInstance(this);
            //Get the resource int of the icon
            int resource = (int)typeof(Resource.Drawable).GetField(filename).GetValue(null);
            //Check that we get a valid int
            if (resource >= 0)
                //Fetch an icon from drawables
                return markerIcon = iconFactory.FromResource((int)typeof(Resource.Drawable).GetField(filename).GetValue(null));


            //If we couldn't find an icon in drawables matching the MarkerTypeIcon field...
            if (markerIcon == null || string.IsNullOrEmpty(markerIcon.Id))
            {
                //Assign a the default icon
                return iconFactory.FromResource(Resource.Drawable.markerDefault);
            }
            return markerIcon;
        }
        /// <summary>
        /// Fetch an icon from resources matching the filename. If a matching icon can't be found, the function returns the default marker icon.
        /// </summary>
        /// <param name="filename">Filename of the desired icon</param>
        /// <returns></returns>
        private Bitmap GetIconBitmap(string filename)
        {
            //Variable for marker icon
            Bitmap markerIcon = null;
            //Iconfactory for fetching an icon from resources
            IconFactory iconFactory = IconFactory.GetInstance(this);
            //Get the resource int of the icon
            int resource = (int)typeof(Resource.Drawable).GetField(filename).GetValue(null);
            //Check that we get a valid int
            if (resource >= 0)
                //Fetch an icon from drawables
                return markerIcon = BitmapFactory.DecodeResource(Resources, (int)typeof(Resource.Drawable).GetField(filename).GetValue(null));
            //If we couldn't find an icon in drawables matching the MarkerTypeIcon field...
            if (markerIcon == null)
            {
                //Assign a the default icon
                return BitmapFactory.DecodeResource(Resources, Resource.Drawable.markerDefault);
            }
            return markerIcon;
        }
        /// <summary>
        /// Remove all markers from map
        /// </summary>
        private void RemoveAllMarkers()
        {
            if (symbolLayer != null && clusterLayer != null)
            {
                ValueAnimator layeranimation = new ValueAnimator();
                layeranimation.SetObjectValues(1f, 0f);
                layeranimation.SetDuration(500);
                layeranimation.Update += (sender, e) =>
                {
                    //Float value = new Float(layeranimation.AnimatedValue.ToString());
                    symbolLayer.SetProperties(
                        PropertyFactory.IconOpacity((Float)layeranimation.AnimatedValue)
                        );
                    clusterLayer.SetProperties(
                    PropertyFactory.IconOpacity((Float)layeranimation.AnimatedValue)
                    );
                };
                layeranimation.Start();
                mapBoxMap.RemoveLayer(clusterLayer);
                mapBoxMap.RemoveLayer(symbolLayer);
            }
        }
        /// <summary>
        /// Add a list of Markers to the MapBoxMap using featurecollection
        /// also set clustering options
        /// </summary>
        /// <param name="markerList">List of Markers to add</param>
        public async void AddMarkerListToMap(List<Marker> markerList)
        {
            Java.Lang.Boolean javaTrue = new Java.Lang.Boolean(true);
            Float iconcompsize = (Float)(-0.2f);
            Float n = (Float)0f;
            Float up = (Float)(-0.5f);
            Float iconUp = (Float)(-25f);
            Float[] textOffSet = new Float[2] { n, up };
            Float[] iconOffSet = new Float[2] { n, iconUp };
            //Check that the list is not null
            if (markerList == null)
            {
                throw new NullPointerException(GetString(Resource.String.errorListIsNull));
            }
            //List for features, features include everything about markers
            List<Feature> listFeatures = new List<Feature>();
            //Single feature
            Feature feature = null;
            foreach (Marker value in markerList)
            {
                //Fetch a tranlation for the Marker
                MarkerTranslation markerTranslation = await AzureClient.GetMarkerTranslation(value.Id, LocaleManager.GetLocale());
                //Fetch a MarkerType for the marker, used to fetch a correct icon for the marker
                MarkerType markerType = await AzureClient.GetMarkerType(value.MarkerType);
                //Marker location for feature
                var point = Com.Mapbox.Geojson.Point.FromLngLat(value.Lon, value.Lat);
                //Construct feature
                feature = Feature.FromGeometry(point);
                feature.AddStringProperty("title", markerTranslation.Title);
                feature.AddStringProperty("poi", markerType.MarkerTypeIcon);
                feature.AddStringProperty("image", value.Image);
                feature.AddStringProperty("popupimage", value.PopupImage);
                feature.AddStringProperty("impactrange", value.Impactrange.ToString());
                feature.AddStringProperty("markerid", value.Id);
                if (markerTranslation.PopupDescription != null)
                {
                    feature.AddStringProperty("style", markerTranslation.PopupDescription);
                }
                else
                {
                    feature.AddStringProperty("style", Resource.String.dialogNoContent.ToString());
                }
                if (markerTranslation.Description != null)
                {
                    feature.AddStringProperty("call-out", markerTranslation.Description);
                }
                else
                {
                    feature.AddStringProperty("call-out", Resource.String.dialogNoContent.ToString());
                }
                feature.AddStringProperty("selected", "false");
                feature.AddStringProperty("favourite", "false");
                listFeatures.Add(feature);
                //if icons are not added already
                if (mapBoxMap.GetImage(markerType.MarkerTypeIcon) == null)
                {
                    //Get bitmap for icons
                    Bitmap icon = GetIconBitmap(markerType.MarkerTypeIcon);
                    //Add icons                  
                    mapBoxMap.AddImage(markerType.MarkerTypeIcon, icon);
                }
                //What is a point of this? #life
                //if (!IsFinishing)
                //{
                //}
            }
            //Get bitmap for cluster icon
            Bitmap clusterIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.markerCluster);
            //Add icons                  
            mapBoxMap.AddImage("clusterMarker", clusterIcon);
            featureCollection = FeatureCollection.FromFeatures(listFeatures);
            if (mapBoxMap.GetSource(markersSourceName) == null)
            {
                geoSource = new GeoJsonSource(markersSourceName, featureCollection, new GeoJsonOptions().WithCluster(true).WithClusterMaxZoom(18).WithClusterRadius(22));
                mapBoxMap.AddSource(geoSource);
            }
            else
            {
                geoSource.SetGeoJson(featureCollection);
            }
            //Add clusters' circles
            if (clusterLayer == null)
            {
                clusterLayer = new SymbolLayer(clusterLayerName, markersSourceName);
            }
            Expression pointCount = Expression.ToNumber(Expression.Get("point_count"));
            //Debuggin purposes
            //clusterLayer.SetProperties(
            //  PropertyFactory.CircleColor(Expression.Color(CircleColor)),
            //  PropertyFactory.CircleRadius(radius),
            //  PropertyFactory.CircleStrokeWidth(strokeWidth),
            //  PropertyFactory.CircleStrokeColor(Color.DarkOrange)
            //  );               
            //Cluster settings
            clusterLayer.SetProperties(
                PropertyFactory.IconImage("clusterMarker"),
                PropertyFactory.IconAllowOverlap(javaTrue),
                PropertyFactory.IconOpacity(n),
                //PropertyFactory.IconOffset(textOffSet),
                PropertyFactory.IconIgnorePlacement(javaTrue));
            clusterLayer.MaxZoom = 22;
            clusterLayer.MinZoom = 0;
            //Setting filtter for layer, hiding layer when there is only 1 marker in featurecollection.
            //Neg = Check the property does not equals the given value
            clusterLayer.Filter = Expression.Neq(Expression.Get("point_count"), Expression.Get("0"));          
            if (mapBoxMap.GetLayer(clusterLayerName) == null)
            {
                mapBoxMap.AddLayer(clusterLayer);
            }
            if (symbolLayer == null)
            {
                symbolLayer = new SymbolLayer(symbolLayerName, markersSourceName);
            }
            //min-max zooms for layer
            symbolLayer.MaxZoom = 22;
            symbolLayer.MinZoom = 0;
            //symbollayer properties (Marker)
            symbolLayer.SetProperties(PropertyFactory.TextField("{point_count}"),
                PropertyFactory.TextIgnorePlacement(javaTrue),
                PropertyFactory.IconSize(IconSize),
                PropertyFactory.TextSize(textSize),
                PropertyFactory.TextHaloColor(Expression.Color(Color.White)),
                PropertyFactory.TextColor(Expression.Color(Color.WhiteSmoke)),
                PropertyFactory.TextOffset(textOffSet),
                PropertyFactory.TextAllowOverlap(javaTrue),
                PropertyFactory.IconAllowOverlap(javaTrue),
                PropertyFactory.IconIgnorePlacement(javaTrue),
                PropertyFactory.Visibility(Property.Visible),
                PropertyFactory.IconImage("{poi}"),
                PropertyFactory.IconOffset(iconOffSet),
                PropertyFactory.IconAnchor("bottom"),
                PropertyFactory.IconOpacity(n),
                PropertyFactory.TextOpacity((Float)0f)
                );
            if (mapBoxMap.GetLayer(symbolLayerName) == null)
            {
                mapBoxMap.AddLayer(symbolLayer);
            }
            //Layer animations
            //Opacity
            ValueAnimator layeranimation = new ValueAnimator();
            layeranimation.SetObjectValues(0f, 1f);
            layeranimation.SetDuration(1000);
            layeranimation.Update += (sender, e) =>
            {
                symbolLayer.SetProperties(
                PropertyFactory.IconOpacity((Float)layeranimation.AnimatedValue),
                PropertyFactory.IconSize((Float)layeranimation.AnimatedValue)
                );
                clusterLayer.SetProperties(
                PropertyFactory.IconOpacity((Float)layeranimation.AnimatedValue)
               );

            };
            //IconOffset
            ValueAnimator layeranimation2 = new ValueAnimator();
            layeranimation2.SetObjectValues(50f, 0f);
            layeranimation2.SetDuration(1000);
            layeranimation2.Update += (sender, e) =>
            {
                symbolLayer.SetProperties(
               PropertyFactory.IconOffset(new Float[] { n, (Float)layeranimation2.AnimatedValue })
               );
                clusterLayer.SetProperties(
                PropertyFactory.IconOffset(new Float[] { n, (Float)layeranimation2.AnimatedValue })
               );
            };
            //IconSize
            ValueAnimator layeranimation3 = new ValueAnimator();
            layeranimation3.SetObjectValues(1f, 0.8f);
            layeranimation3.SetDuration(200);
            layeranimation3.Update += (sender, e) =>
            {
                symbolLayer.SetProperties(
                PropertyFactory.IconSize((Float)layeranimation3.AnimatedValue)
                );
            };
            layeranimation.SetInterpolator(new BounceInterpolator());
            layeranimation2.SetInterpolator(new BounceInterpolator());
            layeranimation3.SetInterpolator(new AccelerateInterpolator(2));
            layeranimation.Start();
            layeranimation2.Start();
            //run animation after iconOffset
            layeranimation2.AnimationEnd += (sender, e) =>
            {
                layeranimation3.Start();
            };
            layeranimation3.AnimationEnd += (sender, e) =>
             {
                 symbolLayer.SetProperties(
                 PropertyFactory.TextOpacity((Float)1f));
             };
        }
        /// <summary>
        /// Update nearbymarkers list.
        /// </summary>
        /// <param name="location"></param>
        private async void GetNearbyMarkers(Location location)
        {
            try
            {
                float[] distance = new float[2];
                //Distance to get relevant markers
                const int distanceToUpdate = 900;

                //Check that MapBox LocationEngine has been initialized and the engine has acquired a location
                if (lastRequestLocation != null)
                {
                    Location.DistanceBetween(location.Latitude, location.Longitude, lastRequestLocation.Latitude, lastRequestLocation.Longitude, distance);
                }

                // Update nearbyMarkers list if distance from last location to current location is greater than distance to update or if lastLocation is null 
                if (distance.FirstOrDefault() > distanceToUpdate || lastRequestLocation == null)
                {
                    lastRequestLocation = location;
                    if (visitedMarkers.Count > 0)
                    {
                        visitedMarkers.Clear();
                        if (objectiveController != null)
                            foreach (TrackedObjectiveThemeMarker tracked in objectiveController.GetTrackedMarkersList(true))
                                visitedMarkers.Add(tracked.Marker);
                    }

                    nearbyMarkers = await AzureClient.GetDefaultTrackedMarkers(new LatLng(location.Latitude, location.Longitude));

                    if (StaticStorage.IsTrailActive)
                    {
                        nearbyMarkers.AddRange(await AzureClient.GetTrailMarkersAroundPosition(SharedPreferences.GetLastTrail(), new LatLng(location.Latitude, location.Longitude)));
                    }

                    if (StaticStorage.IsThemeActive)
                    {
                        nearbyMarkers.AddRange(await AzureClient.GetThemeMarkersAroundPosition(SharedPreferences.GetLastTheme(), new LatLng(location.Latitude, location.Longitude)));
                    }

                    if (StaticStorage.IsObjectiveActive)
                    {
                        nearbyMarkers.AddRange(await AzureClient.GetObjectiveThemeMarkersAroundPosition(SharedPreferences.GetLastTheme(), new LatLng(location.Latitude, location.Longitude)));
                    }
                }

                if (nearbyMarkers != null)
                {
                    CheckForNearbyMarkers(nearbyMarkers, location);
                }
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
            catch (MobileServiceODataException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
        }
        /// <summary>
        /// Check nearby markers and show popup, or add marker to markerlist
        /// </summary>
        /// <param name="markers"></param>
        /// <param name="location"></param>
        private async void CheckForNearbyMarkers(List<Marker> markers, Location location)
        {
            try
            {
                // Loop through each marker in a list
                foreach (var marker in markers)
                {
                    // Check if device is within impact range of the marker
                    if (IsMarkerInRange(marker, location))
                    {
                        // If marker id is not same as visited marker id
                        if (!visitedMarkers.Contains(marker.Id))
                        {
                            visitedMarkers.Add(marker.Id);
                            
                            // If this activity currently is active, start dialog, else send notification
                            if (mActive)
                            {
                                ShowPopUpDialog(marker.Id);
                            }
                            else
                            {
                                //add id to markerlist so drawer keep track of markers, without this notifications not stack on popupfragment listview
                                if (!markerIdList.Contains(marker.Id))
                                {
                                    markerIdList.Add(marker.Id);                                   
                                }
                                
                                if (!SharedPreferences.GetHideNotifications())
                                {

                                    MarkerTranslation translation = await AzureClient.GetMarkerTranslation(marker.Id, LocaleManager.GetLocale());
                                    NotificationProvider.CreateNotification(translation.Title, translation.PopupDescription, marker.Id, 0, true);
                                }
                            }
                        }
                    }

                }
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ShowAlertDialog(GetString(Resource.String.errorTitle), e.Message, GetString(Resource.String.btnOk), null, null);
            }
        }
#endregion

#region Animation Handling
        /// <summary>
        /// Used in BearingButton! Hideing and showing when location button is clicked. fadeOut true if FALSE it will fadeIn
        /// </summary>
        /// <param name="duration">fade duration</param>   
        /// <param name="fadeOut">true fedaout, false fadein</param>
        /// <param name="noFade">True no fede</param>
        private void BearingBtnAnim(int duration, bool fadeOut, bool noFade)
        {
            fadeAnimDuration = duration;
            if (noFade)
            {
                AlphaAnimation disappearAnimation = new AlphaAnimation(1, 1) { Duration = duration };
                btnBearing.StartAnimation(disappearAnimation);
                disappearAnimation.AnimationStart += DisappearAnimation_AnimationStart;
                disappearAnimation.AnimationEnd += DisappearAnimation_AnimationEnd;
            }
            else
            {
                if (fadeOut)
                {
                    AlphaAnimation disappearAnimation = new AlphaAnimation(0, 1) { Duration = duration };
                    btnBearing.StartAnimation(disappearAnimation);
                    disappearAnimation.AnimationStart += DisappearAnimation_AnimationStart;
                    disappearAnimation.AnimationEnd += DisappearAnimation_AnimationEnd;
                }
                else
                {
                    AlphaAnimation disappearAnimation = new AlphaAnimation(1, 0) { Duration = duration };
                    btnBearing.StartAnimation(disappearAnimation);
                    disappearAnimation.AnimationStart += DisappearAnimation_AnimationStart;
                    disappearAnimation.AnimationEnd += DisappearAnimation_AnimationEnd;
                }
            }

        }
        /// <summary>
        /// Bearing button disappear animation start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisappearAnimation_AnimationStart(object sender, Animation.AnimationStartEventArgs e)
        {
            if (!isCamFollowingUser)
            {
                btnBearing.Animate().TranslationY(btnBearing.Width).SetDuration(fadeAnimDuration);
            }
            else
            {
                btnBearing.Animate().TranslationY(0).SetDuration(fadeAnimDuration);
            }
        }
        /// <summary>
        /// Bearing button animation end, but visible or invisible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisappearAnimation_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            if (!isCamFollowingUser)
            {
                btnBearing.Visibility = ViewStates.Invisible;
            }
            else
            {
                btnBearing.Visibility = ViewStates.Visible;
            }
        }
        /// <summary>
        /// Loading animation state
        /// </summary>
        /// <param name="state">true play visible, false pause and invisible</param>
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
#endregion

#region Permissions Handling

        /// <summary>
        /// Check if permission is granted
        /// </summary>
        /// <param name="permission">Permission to be check</param>
        /// <returns>Returns true if permission is granted</returns>
        private bool IsPermissionGranted(string permission)
        {
            // How to check permission state depends on used API
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                // If API is 23 or newer
                return CheckSelfPermission(permission) == (int)Permission.Granted;
            }
            else
            {
                // If API is 22 or older
                return PermissionChecker.CheckSelfPermission(this, permission) == PermissionChecker.PermissionGranted;
            }
        }

        /// <summary>
        /// Request permission
        /// </summary>
        /// <param name="permission">Permission that is required</param>
        /// <param name="requestCode">Unique request code</param>
        private void RequestPermission(string permission, int requestCode)
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                RequestPermissions(new string[] { permission }, requestCode);
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new string[] { permission }, requestCode);
            }
        }

        /// <summary>
        /// Called when a result to permission request has been received.
        /// </summary>
        /// <param name="requestCode">Request code belonging to permission request</param>
        /// <param name="permissions">Permission that was requested</param>
        /// <param name="grantResults">Granted result for the request, true or false</param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            // If request is cancelled, the result array is empty
            if (grantResults.Length > 0)
            {
                switch (requestCode)
                {
                    case Constants.LocationRequestCode:
                        if (grantResults[0] == Permission.Granted)
                        {
                            // Permission granted
                            EnableLocationPlugin();
                        }
                        else if (grantResults[0] == Permission.Denied)
                        {
                            if ((int)Build.VERSION.SdkInt >= 23)
                            {
                                // Should we show an explanation?
                                if (ShouldShowRequestPermissionRationale(permissions[0]))
                                {
                                    // Show permission explanation dialog
                                    ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.errorGpsPermissionDenied), GetString(Resource.String.btnOk));
                                }
                                else
                                {
                                    // Never ask again selected, or device policy prohibits the app from having that permission.
                                    ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.errorGpsPermissionNeverAgain), GetString(Resource.String.btnOk));
                                }
                            }
                            else
                            {
                                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permissions[0]))
                                {
                                    // Show permission explanation dialog
                                    ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.errorGpsPermissionDenied), GetString(Resource.String.btnOk));
                                }
                            }
                        }
                        break;
                }
            }
        }

#endregion

#region Implemented Listeners
        /// <summary>
        /// Callback when item from popupAdapter is deleted
        /// </summary>
        /// <param name="fragment">name of the fragment</param>
        /// <param name="state">just true #life</param>
        /// <param name="position">list position</param>
        /// <param name="id">Marker ID</param>
        public void OnItemRemoved(string fragment, bool state, int position, string id)
        {
            switch (fragment)
            {
                //Handle removed item
                case (Constants.RecyclerPopUpAdapter):
                    if (state)
                    {
                        //Check that markerId is not null
                        if (string.IsNullOrEmpty(id))
                        {
                            throw new ArgumentException(GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));
                        }
                        markerIdList.Remove(id);
                        if (markerIdList.Count == 0)
                        {
                            btnShowNotifications.ClearAnimation();
                            btnShowNotifications.Visibility = ViewStates.Invisible;
                            popfrag.Dismiss();
                        }
                        else
                        {
                            btnShowNotifications.ClearAnimation();
                            btnShowNotifications.Visibility = ViewStates.Visible;
                            btnShowNotifications.Text = markerIdList.Count.ToString();
                            popfrag.RemoveViewAtPosition(position, markerIdList.Count);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Callback when a dialog fragment is dismissed
        /// </summary>
        /// <param name="fragment">Simple name of the dismissed dialog fragment</param>
        /// <param name="state">Boolean result of the dialog. True if positive button was pressed and false if negative button was pressed</param>
        /// <param name="dataString">Data transferred from the dialog</param>
        public async void OnDialogDismissed(string fragment, bool state, string dataString)
        {
            // Use switch determine correct dialog fragment using fragment name
            switch (fragment)
            {
                case (Constants.AlertDialogFragment):
                    // Use switch determine correct AlertDialogFragment using dialog id
                    switch (dataString)
                    {
                        // User is about to exit app
                        case Constants.ExitAppDialog:
                            if (state)
                            {
                                //OpenFeedback();
                                Toast.MakeText(this, "Not in use, put feedback url in Constants", ToastLength.Long).Show();
                                // Finish this activity;
                                Finish();
                            }
                            else
                            {
                                // Finish this activity;
                                Finish();
                            }
                            break;

                        // User is about to finish trail
                        case Constants.FinishTrailDialog:
                            if (state)
                            {
                                //Check if we had a Theme with Objective active
                                if (StaticStorage.IsObjectiveActive)
                                {
                                    //Check if objectiveController succesfully initialized when starting Theme
                                    if (objectiveController != null)
                                    {
                                        //Fetch a new ObjectiveResult from ObjectiveController
                                        ObjectiveResult result = objectiveController.FinishObjective();
                                        //Finish Trail
                                        await FinishTrail();
                                        //Check if user visited a single marker
                                        if (result.VisitedMarkers < 1)
                                            //User didn't visit a single marker, show feedback dialog
                                            ShowAlertDialog(GetString(Resource.String.dialogSendFeedBackTitle), GetString(Resource.String.dialogSendFeedBackContent), GetString(Resource.String.btnYes), GetString(Resource.String.btnNo), Constants.FeedbackDialog);
                                        //Check if ThemeObjective has a Feedback
                                        else if (await AzureClient.ThemeObjectiveHasFeedback(SharedPreferences.GetLastObjective()))
                                        {
                                            ThemeObjectiveFeedback feedback = await AzureClient.GetThemeObjectiveFeedback(SharedPreferences.GetLastTheme(), result.Score);
                                            ThemeObjectiveFeedbackTranslation feedbackTranslation = await AzureClient.GetThemeObjectiveFeedbackTranslation(feedback.Id, LocaleManager.GetLocale());
                                            //Generate a bundle that can be passed to MarkerFragment
                                            Bundle bundle = MarkerFragment.GetBundle(feedbackTranslation.Title, feedbackTranslation.Text, null, feedback.Image);
                                            ShowFragment(new MarkerFragment(), bundle);
                                        }
                                        //Show default feedback
                                        else
                                        {
                                            string feedbackText = GetString(Resource.String.defaultFeedbackTextPart1) + result.Score +
                                                GetString(Resource.String.defaultFeedbackTextPart2) + result.VisitedMarkers +
                                                GetString(Resource.String.defaultFeedbackTextPart3) + result.TotalMarkers +
                                                GetString(Resource.String.defaultFeedbackTextPart4);
                                            //Generate a bundle that can be passed to MarkerFragment
                                            Bundle bundle = MarkerFragment.GetBundle(GetString(Resource.String.defaultFeedbackTitle), feedbackText, null, "");
                                            ShowFragment(new MarkerFragment(), bundle);
                                        }
                                    }
                                    else
                                        //No objectiveController was initialized
                                        ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.errorNoObjectiveController), GetString(Resource.String.btnOk));
                                }
                                else
                                {
                                    //Finish Trail
                                    await FinishTrail();

                                    //Show feedback dialog
                                    ShowAlertDialog(GetString(Resource.String.dialogSendFeedBackTitle), GetString(Resource.String.dialogSendFeedBackContent), GetString(Resource.String.btnYes), GetString(Resource.String.btnNo), Constants.FeedbackDialog);
                                }
                            }
                            break;

                        // User is about give feedback
                        case Constants.FeedbackDialog:
                            if (state)
                            {
                                //OpenFeedback();
                                Toast.MakeText(this, "Not in use, put feedback url in Constants", ToastLength.Long).Show();
                            }
                            break;

                        // User has seen the compass dialog
                        case Constants.CompassDialog:
                            StaticStorage.IsCompassDialogShown = true;
                            break;
                    }
                    break;

                case (Constants.PopUpFragment):
                    if (state)
                    {
                        //Check that markerId is not null
                        if (string.IsNullOrEmpty(dataString))
                        {
                            throw new ArgumentException(GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));
                        }
                        SupportActionBar.SetTitle(Resource.String.appName);

                        //Show TrailsFragment
                        ShowMarkerFragment(dataString);
                    }
                    break;
                case (Constants.PopUpAdapter):
                    if (state)
                    {
                        //Check that markerId is not null
                        if (string.IsNullOrEmpty(dataString))
                        {
                            throw new ArgumentException(GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));
                        }

                        SupportActionBar.SetTitle(Resource.String.appName);

                        //Show TrailsFragment
                        ShowMarkerFragment(dataString);
                    }
                    break;
                case (Constants.RecyclerPopUpAdapter):
                    if (state)
                    {
                        //Check that markerId is not null
                        if (string.IsNullOrEmpty(dataString))
                            throw new ArgumentException(GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));
                        SupportActionBar.SetTitle(Resource.String.appName);
                        markerIdList.Remove(dataString);
                        popfrag.Dismiss();
                        if (markerIdList.Count == 0)
                        {
                            btnShowNotifications.ClearAnimation();
                            btnShowNotifications.Visibility = ViewStates.Invisible;
                        }
                        ShowMarkerFragment(dataString);
                    }
                    break;

                case (Constants.MarkerFragment):
                    //Make sure this activity is visible
                    ClearFragmentStacks();
                    //Show the map UI elements
                    ShowMapUIElements();
                    //Hide the Action Bar
                    SupportActionBar.Hide();
                    break;

                //ObjectiveThemeMarkerFragment invokes this callback when user returns without picking an answer
                case (Constants.ObjectiveThemeMarkerFragment):
                    Toast.MakeText(this, GetString(Resource.String.toastObjectiveVisitAgain), ToastLength.Short).Show();
                    //Make sure this activity is visible
                    ClearFragmentStacks();
                    //Show the map UI elements
                    ShowMapUIElements();
                    //Hide the Action Bar
                    SupportActionBar.Hide();
                    break;

                case (Constants.PopUpRecycleFragment):
                    //Make sure this activity is visible
                    ClearFragmentStacks();
                    //Show the map UI elements
                    ShowMapUIElements();
                    if (markerIdList.Count == 0)
                    {
                        btnShowNotifications.ClearAnimation();
                        btnShowNotifications.Visibility = ViewStates.Invisible;
                    }
                    //Hide the Action Bar
                    //SupportActionBar.Hide();
                    //SupportFragmentManager.BeginTransaction()
                    //    .Hide(popfrag)
                    //    .AddToBackStack(null)
                    //    .Commit();
                    break;

                default:
                    throw new NotImplementedException("An interface with OnDialogDismissed() is used in a fragment " + fragment + " but there is no case added for that fragment in this callback");
            }
        }
  
        public void OnDialogDismissed(string fragment, bool state, int dataInt, string dataString)
        {
            switch (fragment)
            {
                //ObjectiveThemeMarkerFragment invokes this callback when user returns after picking an answer
                case (Constants.ObjectiveThemeMarkerFragment):
                    //Check if returned Marker ID is not null or empty
                    if (string.IsNullOrEmpty(dataString))
                        Toast.MakeText(this, GetString(Resource.String.toastObjectiveVisitAgain), ToastLength.Short).Show();
                    //Check if an objectiveController has been initialized
                    else if (objectiveController != null)
                    {
                        //Check if the ObjectiveThemeMarker has been visited
                        if (!objectiveController.IsVisited(dataString))
                        {
                            //Add score from user's answer and flag marker as visited
                            objectiveController.AddScore(dataString, dataInt);

                            //Check if user has now visited all ObjectiveThemeMarkers in the Theme
                            if (objectiveController.AllMarkersVisited)
                                ShowAlertDialog(GetString(Resource.String.dialogAllObjectiveMarkersVisitedTitle), GetString(Resource.String.dialogAllObjectiveMarkersVisitedMessage), GetString(Resource.String.btnOk));
                        }
                        else
                        {
                            //Check if user has visited all ObjectiveThemeMarkers in the Theme
                            if (objectiveController.AllMarkersVisited)
                                ShowAlertDialog(GetString(Resource.String.dialogAllObjectiveMarkersVisitedTitle), GetString(Resource.String.dialogAllObjectiveMarkersVisitedMessage), GetString(Resource.String.btnOk));
                            else
                                //Marker was already visited
                                Toast.MakeText(this, GetString(Resource.String.toastObjectiveMarkerAlreadyVisited), ToastLength.Short).Show();
                        }
                    }
                    else
                        //No objective controller was initialized before visiting an objectivethememarker
                        ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.errorNoObjectiveController), GetString(Resource.String.btnOk));
                    //Make sure this activity is visible
                    ClearFragmentStacks();
                    //Show the map UI elements
                    ShowMapUIElements();
                    //Hide the Action Bar
                    SupportActionBar.Hide();
                    break;

                default:
                    throw new NotImplementedException("An interface with OnDialogDismissed() is used in a fragment " + fragment + " but there is no case added for that fragment in this callback");
            }

        }

        /// <summary>
        /// Callback when a list item is selected
        /// </summary>
        /// <param name="fragment">Simple name of the calling fragment</param>
        /// <param name="item">Name of the selected item</param>
        /// <param name="routeName">Name of the trail</param>
        public async void OnListItemSelected(string fragment, string item, string routeName)
        {
            switch (fragment)
            {
                case (Constants.LocationsFragment):
                    //Show TrailsFragment with Trails from the selected Location
                    try
                    {
                        if (string.IsNullOrEmpty(item))
                        {
                            throw new NullPointerException(GetString(Resource.String.dbErrorLocationIsNullOrEmpty));
                        }

                        //Set the actionbar title
                        SupportActionBar.SetTitle(Resource.String.drawerTrails);
                        locationName = item;
                        //Show TrailsFragment
                        ShowFragment(new TrailsFragment(), item);
                    }
                    catch (NullPointerException ex)
                    {
                        Log.Error(Class.SimpleName, ex.Message + "\n" + ex.StackTrace);
                        ShowAlertDialog(GetString(Resource.String.errorTitle), GetString(Resource.String.dbErrorLocationIsNullOrEmpty), GetString(Resource.String.btnClose));
                    }
                    break;

                case (Constants.TrailsFragment):
                    if (StaticStorage.IsTrailActive)
                        await FinishTrail();
                    if (!currentRoute.Contains(locationName))
                        currentRoute = locationName.ToString();
                    if (!currentRoute.Contains(routeName))
                        currentRoute = currentRoute + "\n" + routeName;
                    tvDistanceMeterTitle.Text = currentRoute;
                    tvDistanceMeter.Text = " ... ";
                    cardViewDistanceMeter.Visibility = ViewStates.Visible;
                    StartTrail(item);
                    SupportActionBar.Hide();
                    break;

                case "ListFragment":
                    ShowPopUpDialog(item);
                    SupportActionBar.Hide();
                    break;

                default:
                    throw new NotImplementedException("An interface with OnListItemSelected() is used in a fragment " + fragment + " but there is no case added for that fragment in this callback");
            }
        }

        /// <summary>
        /// Callback when a custom dialog is dismissed
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        /// <param name="type"></param>
        public void OnCustomDialogDismissed(bool state, string id)
        {
            switch (id)
            {
                case Constants.GpsDialog:
                    StaticStorage.IsGpsDialogShown = true;

                    if (state)
                    {
                        StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                    }

                    break;

                case Constants.NetworkDialog:
                    StaticStorage.IsNetworkDialogShown = true;

                    if (state)
                    {
                        StartActivity(new Intent(Android.Provider.Settings.ActionWifiSettings));
                    }
                    else if (!state && SharedPreferences.GetFirstTimeRan())
                    {
                        Finish();
                    }
                    break;
            }
        }

        /// <summary>
        /// Called when a broadcast is received
        /// </summary>
        /// <param name="intent">Received intent</param>
        public void OnBroadcastReceive(Intent intent)
        {
            switch (intent.Action)
            {
                case Constants.ServiceNewDialogAction:
                    switch (intent.GetStringExtra(Constants.BroadcastIntentAction))
                    {
                        case Constants.MarkerAction:
                            // Fetch marker ID
                            string markerId = intent.GetStringExtra(Constants.MarkerId);
                            // Check the id for content
                            if (!string.IsNullOrEmpty(markerId))
                            {
                                //Start a new marker popup
                                ShowPopUpDialog(markerId);
                            }
                            break;

                        case Constants.GPSDialogAction:
                            ShowWarningDialog(Constants.GPSDialogAction);
                            break;
                    }
                    break;

                case Constants.LocationUpdate:
                    var location = (Location)intent.Extras.GetParcelable("location");
                    CheckTrailGuide(location);
                    break;

                case Constants.ProviderStatusChanged:
                    if (!ServiceInformation.IsGpsEnabled())
                    {
                        SendBroadcast(new Intent(Constants.ServiceStopAction));
                        Toast.MakeText(this, GetString(Resource.String.errorProviderDisabled), ToastLength.Short).Show();
                    }
                    break;

                case Intent.ActionBatteryLow:
                    if (!SharedPreferences.GetIgnoreBatteryState() && StaticStorage.IsServiceRunning)
                    {
                        SendBroadcast(new Intent(Constants.ServiceStopAction));
                        NotificationProvider.CreateNotification(GetString(Resource.String.notificationBatteryLowTitle), GetString(Resource.String.notificationSeviceStoppedContent), null, 1, true);
                    }
                    break;

                case PowerManager.ActionPowerSaveModeChanged:
                    var powerManager = (PowerManager)GetSystemService(PowerService);

                    if (powerManager.IsPowerSaveMode)
                    {
                        if (!SharedPreferences.GetIgnoreBatteryState() && StaticStorage.IsServiceRunning)
                        {
                            SendBroadcast(new Intent(Constants.ServiceStopAction));
                            NotificationProvider.CreateNotification(GetString(Resource.String.notificationPowerSaverTitle), GetString(Resource.String.notificationSeviceStoppedContent), null, 1, true);
                        }
                    }

                    break;

                case Constants.ErrorDIalog:
                    ShowAlertDialog(GetString(Resource.String.errorTitle), intent.GetStringExtra(Constants.ErrorDIalog), GetString(Resource.String.btnOk));
                    break;

                case "":
                    Log.Error(Class.SimpleName, "intent.Action was empty");
                    break;

                case null:
                    Log.Error(Class.SimpleName, "intent.Action was null");
                    break;
            }
        }

#endregion
    }
}