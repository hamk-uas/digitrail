using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using DigiTrailApp.Adapters;
using DigiTrailApp.Models;
using DigiTrailApp.AzureTables;
using Java.Lang;
using Microsoft.WindowsAzure.MobileServices;
using DigiTrailApp.Helpers;
using Android.Views.InputMethods;
using Android.Animation;
using Android.Views.Animations;
using Android.Support.V7.Widget;
using System.Threading.Tasks;

namespace DigiTrailApp.Fragments
{
    /// <summary>
    /// Fragment for locations
    /// </summary>
    public class TrailsFragment : Android.Support.V4.App.Fragment
    {
        //List containing the trails
        List<TrailList> trailList;
        List<TrailList> nonFilteredTrailList;
        //ListView that contains the trails
        RecyclerView lvTrails;
        //SearchView for filttering trails
        Android.Support.V7.Widget.SearchView _searchView;
        //Adapter for listview
        TrailAdapterRecyclerView _adapter;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Bundle bundle = Arguments;
            View view = inflater.Inflate(Resource.Layout.TrailsLayout, container, false);
            lvTrails = view.FindViewById<RecyclerView>(Resource.Id.lvTrails);

            _searchView = view.FindViewById<Android.Support.V7.Widget.SearchView>(Resource.Id.sv);
            if (_searchView != null)
            {
                _searchView.QueryHint = this.GetString(Resource.String.SearchHint);
                //listen inputs on search field
                _searchView.QueryTextChange += SearchView_QueryTextChange;
                //setting search view open at start                        
                _searchView.SetIconifiedByDefault(false);
                _searchView.Iconified = false;
                _searchView.ClearFocus();
                _searchView.RequestFocusFromTouch();
            }
            lvTrails.SetLayoutManager(new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false));
            return view;
        }
        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            //anim
            Bundle bundle = Arguments;
            await GetTrailNames(bundle.GetString(Class.SimpleName));
            Animation animation = AnimationUtils.LoadAnimation(Context, Resource.Animation.lefttoright);
            LayoutAnimationController animController = new LayoutAnimationController(animation);
            animController.GetAnimationForView(view);
            lvTrails.LayoutAnimation = animController;
            _adapter.NotifyDataSetChanged();
            lvTrails.ScheduleLayoutAnimation();
        }
        public override void OnPause()
        {
            if (_searchView.HasFocus)
                _searchView.ClearFocus();
            base.OnPause();
        }
        public override void OnStop()
        {
            if (_searchView.HasFocus)
                _searchView.ClearFocus();
            base.OnStop();
        }
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
        }
        private void SearchView_QueryTextChange(object sender, Android.Support.V7.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            //Call filter, magic happens on TrailsAdapter class
            _adapter.Filter.InvokeFilter(e.NewText);
        }
        /// <summary>
        /// Get trail names from db
        /// </summary>
        /// <param name="selectedLocation"></param>
        /// <returns>true ok, false error</returns>
        private async Task<bool> GetTrailNames(string selectedLocation)
        {
            try
            {
                trailList = new List<TrailList>();
                List<Trail> trails = await Azure.AzureClient.GetTrails(selectedLocation);
                nonFilteredTrailList = new List<TrailList>();
                foreach (Trail trail in trails)
                {
                    TrailTranslation trailTranslations = await Azure.AzureClient.GetTrailTranslation(trail.Id, LocaleManager.GetLocale());
                    TrailList listItem = new TrailList(trail.Id, trailTranslations.Name, trailTranslations.Description);
                    trailList.Add(listItem);
                    nonFilteredTrailList.Add(listItem);
                }

                trailList.Sort((x, y) => string.Compare(x.Name, y.Name));
                //_adapter = new TrailsAdapter(Activity, trailList);
                _adapter = new TrailAdapterRecyclerView(Activity, trailList);
                //apply adapter               
                //lvTrails.Adapter = _adapter;
                _adapter.HasStableIds = false;
                lvTrails.SetAdapter(_adapter);
                return true;
            }
            catch (MobileServiceInvalidOperationException e)
            {
                ErrorBroadcaster.BroadcastError(Activity, e);
                return false;
            }
        }

        private class SearchViewExpandListener : Object, IMenuItemOnActionExpandListener
        {
            private readonly IFilterable _adapter;

            public SearchViewExpandListener(IFilterable adapter)
            {
                _adapter = adapter;
            }

            public bool OnMenuItemActionCollapse(IMenuItem item)
            {
                _adapter.Filter.InvokeFilter("");
                return true;
            }

            public bool OnMenuItemActionExpand(IMenuItem item)
            {
                return true;
            }
        }
    }
}