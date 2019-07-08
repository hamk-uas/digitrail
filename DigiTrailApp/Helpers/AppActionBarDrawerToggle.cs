using Android.Views;
using SupportDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// This class handles what happens when user is interacting with left navigation drawer
    /// </summary>
    public class AppActionBarDrawerToggle : SupportDrawerToggle
    {
        private AppCompatActivity mHostActivity;
        private int mOpenedResource;
        private int mClosedResource;

        public AppActionBarDrawerToggle(AppCompatActivity host,DrawerLayout drawerLayout, int openedResource, int closedResource) : base(host,drawerLayout,openedResource,closedResource)
        {
            mHostActivity = host;
            mOpenedResource = openedResource;
            mClosedResource = closedResource;
        }

        public override void OnDrawerOpened(View drawerView)
        {
            base.OnDrawerOpened(drawerView);
            //Setting actionbar title when drawer opened, strings is located on Values/Strings xml file.
            mHostActivity.SupportActionBar.SetTitle(mOpenedResource);
        }
        public override void OnDrawerClosed(View drawerView)
        {
            base.OnDrawerClosed(drawerView);
            //Setting actionbar title when drawer closed, strings is located on Values/Strings xml file.
            mHostActivity.SupportActionBar.SetTitle(mClosedResource);
        }
        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {          
            base.OnDrawerSlide(drawerView, slideOffset);
        }

    }
}