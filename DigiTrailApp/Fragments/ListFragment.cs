using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using DigiTrailApp.Adapters;
using DigiTrailApp.Models;
using Android.Content;
using Java.Lang;
using DigiTrailApp.Interfaces;

namespace DigiTrailApp.Fragments
{
    /// <summary>
    /// Show DEV listview, btn DEV clicks
    /// </summary>
    public class ListFragment : Android.Support.V4.App.Fragment
    {
        //List containing the trails
        List<ListFragmentItem> items;
        //ListView that contains the trails
        ListView lvItems;
        private IOnListItemSelectedListener onListItemSelectedListener;

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);

            try
            {
                onListItemSelectedListener = (IOnListItemSelectedListener)context;
            }
            catch (ClassCastException)
            {
                throw new ClassCastException(context.ToString() + " must implement IOnListItemSelectedListener");
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Bundle bundle = Arguments;

            //GetTrailNames(bundle.GetString(Class.SimpleName));

            View view = inflater.Inflate(Resource.Layout.ListFragmentLayout, container, false);
            lvItems = view.FindViewById<ListView>(Resource.Id.lvListFragmentItems);
            lvItems.ItemClick += LvTrails_ItemClick;
            lvItems.Adapter = new ListItemsAdapter(Activity, items);
            
            return view;
        }

        public ListFragment(List<ListFragmentItem> items)
        {
            this.items = items;
        }

        private void LvTrails_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            onListItemSelectedListener.OnListItemSelected(Class.SimpleName, items[e.Position].SmallText,null);
        }
    }
}