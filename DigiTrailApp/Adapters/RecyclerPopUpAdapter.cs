using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Mapbox.Mapboxsdk.Annotations;
using DigiTrailApp.Azure;
using DigiTrailApp.AzureTables;
using DigiTrailApp.Helpers;
using DigiTrailApp.Interfaces;
using Java.IO;
using Java.Lang;

namespace DigiTrailApp.Adapters
{
    /// <summary>
    /// Adapter for popup fragment
    /// </summary>
    public class RecyclerPopUpAdapter : RecyclerView.Adapter
    {
        List<AzureTables.Marker> markerList;
        List<MarkerTranslation> markerTranslationList;
        Context context;
        private IOnItemRemovedListener onItemRemovedListener;
        
        private IOnDialogDismissedListener onDialogDismissedListener;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="markerList">List of Markers</param>
        /// <param name="markerTranslationList">List of MarkerTanslations</param>
        public RecyclerPopUpAdapter(Context context, List<AzureTables.Marker> markerList, List<MarkerTranslation> markerTranslationList)
        {
            this.markerList = markerList;
            this.markerTranslationList = markerTranslationList;
            this.context = context;
            // Make sure that interface is implemented in host activity
            try
            {
                onDialogDismissedListener = (IOnDialogDismissedListener)context;
                onItemRemovedListener = (IOnItemRemovedListener)context;
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
            get { return markerList.Count; }
        }
        /// <summary>
        /// Bind view holder
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PopUpViewHolder vh = holder as PopUpViewHolder;
            //needs to be set null, will cause wrong image on holder else #life
            vh.ivPlaceHolder.SetImageDrawable(null);
            vh.markerImage.SetImageDrawable(null);
            vh.IsRecyclable = false;
            vh.popupTitle.Text = markerTranslationList[position].Title;
            vh.popupText.Text = markerTranslationList[position].PopupDescription;
            vh.btnCancel.Visibility = ViewStates.Visible;
            vh.popupText.Visibility = ViewStates.Invisible;
            vh.animationview.ImageAssetsFolder = "Images";
            vh.animationview.Visibility = ViewStates.Visible;
            vh.ivPlaceHolder.Visibility = ViewStates.Invisible;
            vh.animationview.Loop(true);
            vh.animationview.PlayAnimation();
            MarkerType markerType = await AzureClient.GetMarkerType(markerList[position].MarkerType);
            //Get bitmap for icons and set it to view
            Bitmap icon = GetIconBitmap(markerType.MarkerTypeIcon);
            vh.markerImage.SetImageBitmap(icon);
            //Check the title
            if (!string.IsNullOrEmpty(markerTranslationList[position].Title))
                vh.popupTitle.Text = markerTranslationList[position].Title;

            //Check the description, hide if null or empty
            if (!string.IsNullOrEmpty(markerTranslationList[position].PopupDescription))
            {
                vh.popupText.Text = markerTranslationList[position].PopupDescription;
                vh.popupText.Visibility = ViewStates.Visible;
            }
            else
            {
                vh.popupText.Visibility = ViewStates.Invisible;
            }

            //If the Marker has no full lenght description or image, don't show the button to open the full marker details
            if (string.IsNullOrEmpty(markerTranslationList[position].Description) && string.IsNullOrEmpty(markerList[position].Image))
            {
                vh.btnOk.Visibility = ViewStates.Invisible;
            }
            else
            {
                vh.btnOk.Visibility = ViewStates.Visible;
            }

            //Handle clicks
            //Skip this item, remove from view
            if (!vh.btnCancel.HasOnClickListeners)
            {
                //RouteGuide button click
                vh.btnCancel.Click += delegate
                {
                    var id = markerList[vh.AdapterPosition].Id;
                    markerList.Remove(markerList[vh.AdapterPosition]);
                    markerTranslationList.Remove(markerTranslationList[vh.AdapterPosition]);
                    vh.ivPlaceHolder.SetImageDrawable(null);
                    vh.btnCancel.Visibility = ViewStates.Invisible;
                    vh.btnOk.Visibility = ViewStates.Invisible;
                    onItemRemovedListener.OnItemRemoved(Class.SimpleName, true, vh.AdapterPosition, id);
                    NotifyItemRemoved(vh.AdapterPosition);
                    NotifyItemRangeChanged(vh.AdapterPosition, markerList.Count);
                };
            }
            //this make sure that button has only one clicklistener
            if (!vh.btnOk.HasOnClickListeners)
            {
                vh.btnOk.Click += async delegate
                {
                    var id = markerList[vh.AdapterPosition].Id;
                    onDialogDismissedListener.OnDialogDismissed(Class.SimpleName, true, id);
                };
            }
           
            Bitmap bitmapImage = null;
            //if popupimage not null or empty set popupimage else default image
            if (!string.IsNullOrEmpty(markerList[position].PopupImage))
            {
                if (ServiceInformation.IsConnectionActive())
                {
                    try
                    {
                        string filePath = System.IO.Path.Combine(context.CacheDir.AbsolutePath, markerList[position].Id+".jpg");
                        if (!System.IO.File.Exists(filePath))
                        {
                            byte[] bytes = await new HttpRequest().GetImage(markerList[position].PopupImage);
                            File image = new File(context.CacheDir, markerList[position].Id + ".jpg");
                            Java.IO.FileOutputStream bos = new Java.IO.FileOutputStream(image);
                            bos.Write(bytes);
                            bos.Flush();
                            bos.Close();
                            bitmapImage = await BitmapFactory.DecodeByteArrayAsync(bytes, 0, bytes.Length);
                        }
                        else
                        {
                            bitmapImage = await BitmapFactory.DecodeFileAsync(filePath);
                        }
                        vh.tvPopUpNoImage.Visibility = ViewStates.Invisible;
                        
                    }
                    catch (System.Exception ex)
                    {
                        bitmapImage = await BitmapFactory.DecodeResourceAsync(context.Resources, Resource.Drawable.ImagePlaceHolder);
                        vh.tvPopUpNoImage.Text = context.Resources.GetString(Resource.String.dialogNoImageCauseInternet);
                        vh.tvPopUpNoImage.Visibility = ViewStates.Visible;
                    }
                }
                else
                {
                    bitmapImage = await BitmapFactory.DecodeResourceAsync(context.Resources, Resource.Drawable.ImagePlaceHolder);
                    vh.tvPopUpNoImage.Visibility = ViewStates.Visible;
                }

            }
            else
            {
                bitmapImage = await BitmapFactory.DecodeResourceAsync(context.Resources, Resource.Drawable.ImagePlaceHolder);
            }
            vh.animationview.Loop(false);
            vh.animationview.PauseAnimation();
            vh.animationview.Visibility = ViewStates.Invisible;
            vh.ivPlaceHolder.RefreshDrawableState();
            vh.ivPlaceHolder.SetImageBitmap(bitmapImage);
            vh.ivPlaceHolder.Visibility = ViewStates.Visible;
        }
        /// <summary>
        /// What happens on Detached
        /// </summary>
        /// <param name="recyclerView"></param>
        public override void OnDetachedFromRecyclerView(RecyclerView recyclerView)
        {
            base.OnDetachedFromRecyclerView(recyclerView);
            recyclerView.ClearDisappearingChildren();
        }
        /// <summary>
        /// Get icon bitmap 
        /// </summary>
        /// <param name="filename">name of file</param>
        /// <returns></returns>
        private Bitmap GetIconBitmap(string filename)
        {
            //Variable for marker icon
            Bitmap markerIcon = null;
            //Iconfactory for fetching an icon from resources
            IconFactory iconFactory = IconFactory.GetInstance(context);
            //Get the resource int of the icon
            int resource = (int)typeof(Resource.Drawable).GetField(filename).GetValue(null);
            //Check that we get a valid int
            if (resource >= 0)
                //Fetch an icon from drawables
                return markerIcon = BitmapFactory.DecodeResource(context.Resources, (int)typeof(Resource.Drawable).GetField(filename).GetValue(null));
            //If we couldn't find an icon in drawables matching the MarkerTypeIcon field...
            if (markerIcon == null)
            {
                //Assign a the default icon
                return BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.markerDefault);
            }
            return markerIcon;
        }
        /// <summary>
        /// View holder for popup adapter
        /// </summary>
        /// <param name="parent">ViewGroup</param>
        /// <param name="viewType">int</param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                    Inflate(Resource.Layout.PopUpLayout, parent, false);
            PopUpViewHolder vh = new PopUpViewHolder(itemView);
            vh.ivPlaceHolder.Visibility = ViewStates.Invisible;
            return vh;
        }
    }
}