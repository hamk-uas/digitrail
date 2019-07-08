using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DigiTrailApp.Models;

namespace DigiTrailApp.Adapters
{
    /// <summary>
    /// BaseAdapter for list with 3 textviews, for DEV purposes
    /// </summary>
    class ListItemsAdapter : BaseAdapter
    {
        List<ListFragmentItem> items;
        Context context;

        public ListItemsAdapter(Context context, List<ListFragmentItem> items)
        {
            this.context = context;
            this.items = items;
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
            ListFragmentAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as ListFragmentAdapterViewHolder;

            if (holder == null)
            {
                holder = new ListFragmentAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                view = inflater.Inflate(Resource.Layout.ListFragmentItemLayout, parent, false);
                holder.LargeText = view.FindViewById<TextView>(Resource.Id.tvListFragmentItemLarge);
                holder.MediumText = view.FindViewById<TextView>(Resource.Id.tvListFragmentItemMedium);
                holder.SmallText = view.FindViewById<TextView>(Resource.Id.tvListFragmentItemSmall);
                view.Tag = holder;
            }


            //fill in your items
            if (!string.IsNullOrEmpty(items[position].LargeText))
            {
                holder.LargeText.Text = items[position].LargeText;
                holder.LargeText.Visibility = ViewStates.Visible;

            }
            else
                holder.LargeText.Visibility = ViewStates.Gone;

            if (!string.IsNullOrEmpty(items[position].MediumText))
            {
                holder.MediumText.Text = items[position].MediumText;
                holder.MediumText.Visibility = ViewStates.Visible;
                
            }
            else
                holder.MediumText.Visibility = ViewStates.Gone;

            if (!string.IsNullOrEmpty(items[position].SmallText))
            {
                holder.SmallText.Text = items[position].SmallText;
                holder.SmallText.Visibility = ViewStates.Visible;
            }
            else
                holder.SmallText.Visibility = ViewStates.Gone;

            return view;
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

    }

    class ListFragmentAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        public TextView LargeText { get; set; }
        public TextView MediumText { get; set; }
        public TextView SmallText { get; set; }
    }
}