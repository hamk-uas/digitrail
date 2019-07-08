using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// Just A holder for popupadapter
    /// </summary>
    public class PopUpViewHolder : RecyclerView.ViewHolder
    {
        public Button btnOk { get; private set; }
        public Button btnCancel { get; private set; }
        public TextView popupTitle { get; private set; }
        public TextView popupText { get; private set; }
        //public ImageView popupImage { get; private set; }
        public ImageView markerImage { get; private set; }
        public LottieAnimationView animationview { get; private set; }
        public RelativeLayout rlImageHolderLayout { get; private set; }
        //public CardView CardView { get; private set; }
        public TextView tvPopUpNoImage { get; private set; }
        public ImageView ivPlaceHolder { get; private set; }

        public PopUpViewHolder(View itemView):base(itemView)
        {
            btnOk = itemView.FindViewById<Button>(Resource.Id.btnPopupOpen);
            btnCancel = itemView.FindViewById<Button>(Resource.Id.btnPopupClose);
            popupTitle = itemView.FindViewById<TextView>(Resource.Id.tvAttractionTitle);
            popupText = itemView.FindViewById<TextView>(Resource.Id.tvAttractionAdapterContent);
            animationview = itemView.FindViewById<LottieAnimationView>(Resource.Id.AnimationViewPopUp);
            markerImage = itemView.FindViewById<ImageView>(Resource.Id.IvPopUpMarkerImage);
            rlImageHolderLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.rlPopUpImageHolder);
            animationview.ImageAssetsFolder = "Images";
            tvPopUpNoImage = itemView.FindViewById<TextView>(Resource.Id.tvPopUpNoImage);            
            ivPlaceHolder = itemView.FindViewById<ImageView>(Resource.Id.ivImagePlaceholder);
        }
    }
}