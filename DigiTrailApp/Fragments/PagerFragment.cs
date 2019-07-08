using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Com.Airbnb.Lottie;

namespace DigiTrailApp.Fragments
{
    /// <summary>
    /// Fragment that uses layout with <see cref="ViewPager"/> to show one or more <see cref="SlideFragment"/> fragments
    /// </summary>
    class PagerFragment : Android.Support.V4.App.Fragment
    {
        private readonly string markerId;

        /// <summary>
        /// Consturctor of the PagerFragment
        /// </summary>
        /// <param name="markerId">ID of the marker in DB</param>
        public PagerFragment(string markerId)
        {
            this.markerId = markerId;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            // Get root view of the layout containing android.support.v4.view.ViewPager widget
            ViewGroup rootView = (ViewGroup)inflater.Inflate(Resource.Layout.PagerFragmentLayout, container, false);            
            // Use root view to find pager view to instantiate a ViewPager
            ViewPager viewPager = (ViewPager)rootView.FindViewById(Resource.Id.pager);
            // The pager adapter provides the pages to the view pager widget
            viewPager.Adapter = new PagerAdapter(markerId, FragmentManager);
            ((MainActivity)this.Activity).loadingCardview.Visibility = ViewStates.Gone;
            return viewPager;
        }

    }
}