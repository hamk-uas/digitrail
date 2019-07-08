using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using DigiTrailApp.Adapters;
using DigiTrailApp.Azure;
using DigiTrailApp.AzureTables;
using DigiTrailApp.Helpers;
using DigiTrailApp.Interfaces;
using Java.Lang;

namespace DigiTrailApp.Fragments
{
    /// <summary>
    /// Fragment to handle popups
    /// </summary>
    public class PopUpRecycleFragment : Android.Support.V4.App.DialogFragment
    {
        private TextView tvPopUpTitleCounter;
        private List<string> trailIdList;
        private List<Marker> markerList;
        private List<MarkerTranslation> markerTranslationList;
        private RecyclerView recyclerView;
        private Context context;
        private Button btnCloseFragment;
        private ImageView ivArrowLeft;
        private ImageView ivArrowRight;
        RecyclerView.LayoutManager mLayoutManager;
        RecyclerPopUpAdapter mAdapter;
        SnapHelper snapHelper;
        Marker lastAddedMarker;
        MarkerTranslation lastAddedMarkerTrasnaltion;
        int itemPosition;

        private IOnDialogDismissedListener onDialogDismissedListener;

        /// <summary>
        /// Popup fragment
        /// </summary>
        /// <param name="contex">context</param>
        /// <param name="trailIdList">Trail ID list</param>
        /// <param name="ItemPosition">Item position</param>
        public PopUpRecycleFragment(Context contex, List<string> trailIdList,int ItemPosition)
        {
            this.trailIdList = trailIdList;
            this.context = contex;
            itemPosition = ItemPosition;
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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.PopUpListView, container, false);            
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.lvPopUps);            
            tvPopUpTitleCounter = view.FindViewById<TextView>(Resource.Id.tvPopUpListTitle);
            btnCloseFragment = view.FindViewById<Button>(Resource.Id.btnPopUpRecycleClose);
            ivArrowLeft = view.FindViewById<ImageView>(Resource.Id.ivRecyclerViewLeftArrow);
            ivArrowRight = view.FindViewById<ImageView>(Resource.Id.ivRecyclerViewRightArrow);              
            return view;
        }
        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            await GetData();
            // Plug in the linear layout manager:
            mLayoutManager = new LinearLayoutManager(context, LinearLayoutManager.Horizontal, false);
            recyclerView.SetLayoutManager(mLayoutManager);
            recyclerView.CanScrollHorizontally(0);            
            snapHelper = new PagerSnapHelper();
            snapHelper.AttachToRecyclerView(recyclerView);
            //attach adapter
            mAdapter = new RecyclerPopUpAdapter(context, markerList, markerTranslationList);
            mAdapter.HasStableIds = false;
            recyclerView.SetAdapter(mAdapter);
            btnCloseFragment.Click += BtnCloseFragment_Click;
            ivArrowRight.Visibility = ViewStates.Invisible;
            ivArrowLeft.Visibility = ViewStates.Invisible;
            btnCloseFragment.Visibility = ViewStates.Visible;
            tvPopUpTitleCounter.Visibility = ViewStates.Invisible;
            if (markerList.Count > 1)
            {
                tvPopUpTitleCounter.Text = markerList.Count + " " + Resources.GetString(Resource.String.popuptitle);
                tvPopUpTitleCounter.Visibility = ViewStates.Visible;
                ivArrowRight.Visibility = ViewStates.Visible;
                ivArrowLeft.Visibility = ViewStates.Visible;
            }
            else
            {
                tvPopUpTitleCounter.Visibility = ViewStates.Invisible;
                ivArrowRight.Visibility = ViewStates.Invisible;
                ivArrowLeft.Visibility = ViewStates.Invisible;
            }
            SmoothScrollToItem(itemPosition);
        }
        private void BtnCloseFragment_Click(object sender, EventArgs e)
        {
            onDialogDismissedListener.OnDialogDismissed(Class.SimpleName, true, null);
            SharedPreferences.SetMarkerId(null);
            Dismiss();
        }
        /// <summary>
        /// Get Markers and translations
        /// </summary>
        /// <returns></returns>
        public async Task GetData()
        {
            try
            {
                if (trailIdList != null)
                {
                    if (markerList == null)
                        markerList = new List<Marker>();
                    if (markerTranslationList == null)
                        markerTranslationList = new List<MarkerTranslation>();
                    markerList.Clear();
                    markerTranslationList.Clear();
                    foreach (string item in trailIdList)
                    {
                        //Fetch a Marker from the local table based on the ID
                        Marker marker = await AzureClient.GetMarker(item);
                        markerList.Add(marker);
                        //Fetch a translation for the Marker based on ID and current Language (will fetch English by default)
                        MarkerTranslation translation = await AzureClient.GetMarkerTranslation(item, LocaleManager.GetLocale());
                        markerTranslationList.Add(translation);
                    }
                }
                if (trailIdList.Count == 1)
                {
                    tvPopUpTitleCounter.Visibility = ViewStates.Invisible;
                }
                else
                {
                    tvPopUpTitleCounter.Visibility = ViewStates.Visible;
                }
            }
            catch(System.Exception ex)
            {
                Log.Debug(Class.SimpleName, "Error (List was modified during foreach? : " + ex);
            }
        }
        /// <summary>
        /// Get one marker item
        /// </summary>
        /// <param name="markerId">Id of the marker</param>
        /// <returns></returns>
        public async Task GetOneMarkerItem(string markerId)
        {
            lastAddedMarker = new Marker();
            lastAddedMarkerTrasnaltion = new MarkerTranslation();
             //Fetch a Marker from the local table based on the ID
            lastAddedMarker = await AzureClient.GetMarker(markerId);
            markerList.Add(lastAddedMarker);
            //Fetch a translation for the Marker based on ID and current Language (will fetch English by default)
            lastAddedMarkerTrasnaltion = await AzureClient.GetMarkerTranslation(markerId, LocaleManager.GetLocale());
            markerTranslationList.Add(lastAddedMarkerTrasnaltion);            
        }        
        /// <summary>
        /// Refresh List and scroll to added item.
        /// </summary>
        /// <param name="markerId"></param>
        /// <returns></returns>
        public async Task RefreshList(string markerId)
        {
            await GetOneMarkerItem(markerId);                        
            mAdapter.NotifyItemInserted(markerList.Count);
            mAdapter.NotifyItemChanged(markerList.Count);
            SetPopUpTitleCount(markerList.Count);           
            var bounceAnimation = ObjectAnimator.OfFloat(tvPopUpTitleCounter, "translationX", 0f, 0,80,0);
            bounceAnimation.SetDuration(500);
            bounceAnimation.SetInterpolator(new BounceInterpolator());
            await bounceAnimation.StartAsync();
            SmoothScrollToItem(markerList.Count);
        }
        public void SmoothScrollToItem(int itempos)
        {
            recyclerView.SmoothScrollToPosition(itempos);
        }
        
        public void SetPopUpTitleCount(int count)
        {
            if (count > 1)
            {
                tvPopUpTitleCounter.Text = count + " " + Resources.GetString(Resource.String.popuptitle);
                tvPopUpTitleCounter.Visibility = ViewStates.Visible;
                ivArrowRight.Visibility = ViewStates.Visible;
                ivArrowLeft.Visibility = ViewStates.Visible;
            }
            else
            {
                tvPopUpTitleCounter.Visibility = ViewStates.Invisible;
                ivArrowRight.Visibility = ViewStates.Invisible;
                ivArrowLeft.Visibility = ViewStates.Invisible;
            }
            
        }
        /// <summary>
        /// Remove item from listview
        /// </summary>
        /// <param name="position">item position</param>
        /// <param name="itemcount">item count for the textview title</param>
        public async void RemoveViewAtPosition(int position, int itemcount)
        {
            SetPopUpTitleCount(itemcount);
            var bounceAnimation = ObjectAnimator.OfFloat(tvPopUpTitleCounter, "translationX", 0f, 0,50,0);
            bounceAnimation.SetDuration(200);
            bounceAnimation.SetInterpolator(new BounceInterpolator());
            await bounceAnimation.StartAsync();
            if (position == 0)
                recyclerView.SmoothScrollToPosition(position + 1);
            else
                recyclerView.SmoothScrollToPosition(position - 1);       
        }
        
    }
}