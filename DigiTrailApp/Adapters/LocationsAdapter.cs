using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Animation;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using DigiTrailApp.AzureTables;
using DigiTrailApp.Helpers;

namespace DigiTrailApp.Adapters
{
    /// <summary>
    /// Base Adapter for locations
    /// </summary>
    class LocationsAdapter : BaseAdapter
    {
        List<Location> locations;
        Context context;

        public LocationsAdapter(Context context, List<Location> locations)
        {
            this.context = context;
            this.locations = locations;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            LocationsAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as LocationsAdapterViewHolder;

            if (holder == null)
            {
                holder = new LocationsAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                view = inflater.Inflate(Resource.Layout.LocationsItemLayout, parent, false);
                holder.Title = view.FindViewById<TextView>(Resource.Id.tvLocationsItemTitle);
                holder.cardview = view.FindViewById<CardView>(Resource.Id.cardViewLocationContainer);
                holder.mainLayout = view.FindViewById<LinearLayout>(Resource.Id.locationsItemLayout);
                view.Tag = holder;
            }
            //fill in your items
            holder.Title.Text = locations[position].Name;
            return view;
        }       
        /// <summary>
        /// Count items
        /// </summary>
        public override int Count
        {
            get
            {
                return locations.Count;
            }
        }
        
    }
    
    class LocationsAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        public TextView Title { get; set; }
        public CardView cardview { get; set; }
        public LinearLayout mainLayout { get; set; }
    }

}