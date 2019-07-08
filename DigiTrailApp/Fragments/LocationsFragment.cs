using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using DigiTrailApp.AzureTables;
using Android.Content;
using DigiTrailApp.Adapters;
using Java.Lang;
using Microsoft.WindowsAzure.MobileServices;
using System;
using DigiTrailApp.Helpers;
using Android.Animation;
using Android.Views.Animations;
using DigiTrailApp.Interfaces;

namespace DigiTrailApp.Fragments
{
    /// <summary>
    /// Fragment for locations
    /// </summary>
    public class LocationsFragment : Android.Support.V4.App.Fragment
    {
        List<Location> locationNames;
        ListView lvLocations;

        private IOnListItemSelectedListener onListItemSelectedListener;       

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);

            // Make sure that interface is implemented in host activity
            try
            {
                onListItemSelectedListener = (IOnListItemSelectedListener)context;
            }
            catch (ClassCastException)
            {
                throw new ClassCastException(context.ToString() + " must implement ILocationSelectedListener");
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.LocationsLayout, container, false);
            LinearLayout layout = view.FindViewById<LinearLayout>(Resource.Id.locationsItemLayout);
            lvLocations = view.FindViewById<ListView>(Resource.Id.lvLocations);
            lvLocations.ItemClick += LvLocations_ItemClick;

            //Listviw animation
            Animation animation = AnimationUtils.LoadAnimation(Context, Resource.Animation.lefttoright);
            LayoutAnimationController animController = new LayoutAnimationController(animation);
            animController.GetAnimationForView(view);
            lvLocations.LayoutAnimation = animController;
            lvLocations.StartLayoutAnimation();

            return view;
        }

        public async override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            await GetLocationNames();
        }

        private async void LvLocations_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Show TrailsFragment with Trails from the selected Location
            var fadeanimation = ObjectAnimator.OfFloat(e.View, "translationX", 0f, e.View.Width);
            fadeanimation.SetDuration(200);
            fadeanimation.SetInterpolator(new AccelerateInterpolator(2f));
            await fadeanimation.StartAsync();
            onListItemSelectedListener.OnListItemSelected(Class.SimpleName, locationNames[e.Position].Name, null);
        }

        private async Task GetLocationNames()
        {
            try
            {
                //Get locations from db
                locationNames = await Azure.AzureClient.GetLocations();
                locationNames.Sort((x, y) => string.Compare(x.Name, y.Name));
                lvLocations.Adapter = new LocationsAdapter(Activity, locationNames);
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
            catch (NullReferenceException e)
            {
                ErrorBroadcaster.BroadcastError(Activity, e);
            }
        }
    }
}