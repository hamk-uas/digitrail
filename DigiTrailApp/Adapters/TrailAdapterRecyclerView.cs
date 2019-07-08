using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Borjabravo.Readmoretextview;
using Com.Mapbox.Mapboxsdk.Geometry;
using DigiTrailApp.AsyncTasks;
using DigiTrailApp.Helpers;
using DigiTrailApp.Interfaces;
using DigiTrailApp.Models;
using Java.Lang;
using Microsoft.WindowsAzure.MobileServices;

namespace DigiTrailApp.Adapters
{
    /// <summary>
    /// Recycler adapter for trails
    /// </summary>
    public class TrailAdapterRecyclerView : RecyclerView.Adapter, IFilterable
    {
        private List<TrailList> trails;
        private readonly Context context;
        //items is a filered list of trails
        private List<TrailList> _items;
        private IOnListItemSelectedListener onListItemSelectedListener;
        string trailTitle = null;
        AlphaAnimation fadein;
        AlphaAnimation fadeout;
        /// <summary>
        /// Trail adapter constructor
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="trails">List of trail lists</param>
        public TrailAdapterRecyclerView(Context context, List<TrailList> trails)
        {
            _items = trails.OrderBy(s => s.Name).ToList();
            this.context = context;
            this.trails = trails;
            try
            {
                onListItemSelectedListener = (IOnListItemSelectedListener)context;
            }
            catch (ClassCastException)
            {
                throw new ClassCastException(context.ToString() + " must implement IMarkerDismissedListener");
            }
        }
        /// <summary>
        /// Item count
        /// </summary>
        public override int ItemCount
        {
            get
            {
                return _items.Count;
            }
        }
        /// <summary>
        /// On bind view holder
        /// </summary>
        /// <param name="holder">ViewHolder</param>
        /// <param name="position">int</param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TrailsAdapterRecyclerViewHolder vh = holder as TrailsAdapterRecyclerViewHolder;
            vh.Description.Text = _items[position].Description;
            vh.BtnRouteGuide.Text = context.Resources.GetString(Resource.String.gMapsRouteGuide);
            vh.BtnStartTrail.Text = _items[position].Name;
            if (!vh.BtnRouteGuide.HasOnClickListeners)
            {
                //Start phone default navigation
                vh.BtnRouteGuide.Click += delegate
                 {
                     vh.BtnRouteGuide.StartAnimation(fadein);
                     BtnRouteGuideClick(_items[position].Id);
                     vh.BtnRouteGuide.StartAnimation(fadeout);

                 };
            }
            if (!vh.BtnStartTrail.HasOnClickListeners)
            {
                //Start trail click, pass values to item selected listener
                vh.BtnStartTrail.Click += async delegate
                 {
                     var fadeanimation = ObjectAnimator.OfFloat(vh.CardView, "translationX", 0f, vh.CardView.Width);
                     fadeanimation.SetDuration(200);
                     fadeanimation.SetInterpolator(new AccelerateInterpolator(2f));
                     await fadeanimation.StartAsync();
                     trailTitle = _items[vh.AdapterPosition].Name;
                     var trailItem = trails.Find(x => x.Name == _items[vh.AdapterPosition].Name);
                     onListItemSelectedListener.OnListItemSelected(Constants.TrailsFragment, trailItem.Id, trailTitle);
                 };
            }
        }
        /// <summary>
        /// On Create ViewHolder
        /// </summary>
        /// <param name="parent">ViewGroup</param>
        /// <param name="viewType">int</param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(Resource.Layout.TrailsItemLayout, parent, false);
            TrailsAdapterRecyclerViewHolder vh = new TrailsAdapterRecyclerViewHolder(itemView);
            Filter = new TrailsFilter(this);
            fadeout = new AlphaAnimation(1, 0);
            fadein = new AlphaAnimation(0, 1);
            fadeout.Duration = 1500;
            fadein.Duration = 1000;
            return vh;
        }

        /// <summary>
        /// starts phone default navigation
        /// </summary>
        /// <param name="trailID">trail id for getting lat lang</param>
        private void BtnRouteGuideClick(string trailID)
        {
            try
            {
                Task<List<LatLng>> getLatLngs = Task.Run(() => new DecodeTask().DecodeGpxById(trailID));
                getLatLngs.Wait();
                List<LatLng> list = getLatLngs.Result;

                if (list[0] == null)
                {
                    Toast.MakeText(context, context.GetString(Resource.String.errorLoadingTrail), ToastLength.Long).Show();
                }
                double latdouble = list[0].Latitude;
                double lngdouble = list[0].Longitude;
                //start google navigation to given coordinates
                string lat = Regex.Replace(latdouble.ToString(), @"\,+", ".");
                string lng = Regex.Replace(lngdouble.ToString(), @"\,+", ".");
                var geoUri = Android.Net.Uri.Parse("geo:" + lat + "," + lng + "?q=" + lat + "," + lng + "(" + context.GetString(Resource.String.gMapsStartingPoint) + ")");
                try
                {
                    //Try to launch a navigation application
                    context.StartActivity(new Intent(Intent.ActionView).SetData(geoUri));
                }
                catch (ActivityNotFoundException)
                {
                    //If an application is not found, launch browser to Google Maps with parameters origin = user location and destination = trail start point
                    context.StartActivity(new Intent(Intent.ActionView).SetData(Android.Net.Uri.Parse("https://www.google.com/maps/dir/?api=1&destination=" + lat + "," + lng)));
                }

            }
            catch (HttpRequestException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ErrorBroadcaster.BroadcastError((Activity)context, e);
            }
            catch (ArgumentNullException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ErrorBroadcaster.BroadcastError((Activity)context, e);
            }
            catch (NullPointerException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ErrorBroadcaster.BroadcastError((Activity)context, e);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ErrorBroadcaster.BroadcastError((Activity)context, e);
            }
            catch (UriFormatException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ErrorBroadcaster.BroadcastError((Activity)context, e);
            }
            catch (IllegalStateException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ErrorBroadcaster.BroadcastError((Activity)context, e);
            }
            catch (ArgumentException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ErrorBroadcaster.BroadcastError((Activity)context, e);
            }
            catch (NullReferenceException e)
            {
                Log.Error(Class.SimpleName, e.Message + "\n" + e.StackTrace);
                ErrorBroadcaster.BroadcastError((Activity)context, e);
            }
        }

        /// <summary>
        /// Filter for search field
        /// </summary>
        public Filter Filter { get; private set; }
        /// <summary>
        /// Filter takes searchview text and compares it to trails list
        /// </summary>
        public class TrailsFilter : Filter
        {
            readonly TrailAdapterRecyclerView _adapter;

            public TrailsFilter(TrailAdapterRecyclerView adapter)
            {
                _adapter = adapter;
            }
            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();
                var results = new List<TrailList>();
                if (_adapter.trails == null)
                {
                    _adapter.trails = _adapter._items;
                }
                if (constraint == null) return returnObj;

                if (_adapter.trails != null && _adapter.trails.Any())
                {
                    // Compare constraint to all names lowercased. 
                    // It they are contained they are added to results.
                    results.AddRange(
                        _adapter.trails.Where(
                            trail => trail.Description.ToLower().Contains(constraint.ToString())));
                }

                // Nasty piece of .NET to Java wrapping, be careful with this!
                // This uses Models -> ObjextExtensions class
                returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                returnObj.Count = results.Count;
                constraint.Dispose();
                return returnObj;
            }
            /// <summary>
            /// Result
            /// </summary>
            /// <param name="constraint">ICharSequense</param>
            /// <param name="results">FilterResult</param>
            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    _adapter._items = values.ToArray<Java.Lang.Object>()
                        .Select(r => r.ToNetObject<TrailList>()).ToList();

                _adapter.NotifyDataSetChanged();
                // Don't do this and see GREF counts rising
                constraint.Dispose();
                results.Dispose();
            }
        }
        /// <summary>
        /// ViewHolder constructor
        /// </summary>
        class TrailsAdapterRecyclerViewHolder : RecyclerView.ViewHolder
        {
            //Your adapter views to re-use
            public ReadMoreTextView Description { get; set; }
            public Button BtnRouteGuide { get; set; }
            public Button BtnStartTrail { get; set; }
            public CardView CardView { get; set; }
            public TrailsAdapterRecyclerViewHolder(View itemView) : base(itemView)
            {
                Description = itemView.FindViewById<ReadMoreTextView>(Resource.Id.tvTrailsItemDescription);
                BtnRouteGuide = itemView.FindViewById<Button>(Resource.Id.btnRouteGuide);
                BtnStartTrail = itemView.FindViewById<Button>(Resource.Id.btnStartTrail);
                CardView = itemView.FindViewById<CardView>(Resource.Id.rlTrailsItemContainer);

            }
        }

    }
}