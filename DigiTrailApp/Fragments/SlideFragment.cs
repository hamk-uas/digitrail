using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;

namespace DigiTrailApp
{
    public class SlideFragment : Android.Support.V4.App.Fragment
    {
        private readonly string title, content, link, position;
        private readonly Android.Graphics.Bitmap bitmap;

        private TextView titleView, contentView, linkView, pageView;
        private ImageView imageView;

        /// <summary>
        /// Constructor of the SlideFragment class
        /// </summary>
        /// <param name="title">The title to be shown</param>
        /// <param name="content">The textual content to be shown</param>
        /// <param name="link">Link of a website</param>
        /// <param name="bitmap">Bitmap image</param>
        /// <param name="position">Page / order number</param>
        public SlideFragment(string title, string content, string link, Android.Graphics.Bitmap bitmap, string position)
        {
            this.position = position;
            this.title = title;
            this.content = content;
            this.link = link;
            this.bitmap = bitmap;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            ViewGroup rootView = (ViewGroup)inflater.Inflate(Resource.Layout.SlideFragmentLayout, container, false);

            titleView = (TextView)rootView.FindViewById(Resource.Id.title);
            titleView.Text = (title != null ? title : GetString(Resource.String.dialogDefaultTitle));

            imageView = (ImageView)rootView.FindViewById(Resource.Id.image);
            if (bitmap != null)
            {
                imageView.SetImageBitmap(bitmap);
            }
            else
            {
                imageView.Visibility = ViewStates.Gone;
            }

            contentView = (TextView)rootView.FindViewById(Resource.Id.content);
            contentView.Text = (content != null ? content : GetString(Resource.String.dialogDefaultContent));

            linkView = (TextView)rootView.FindViewById(Resource.Id.link);
            linkView.Text = (link != null ? link : null);

            pageView = (TextView)rootView.FindViewById(Resource.Id.page);
            pageView.Text = (pageView != null ? position  : null);

            Button closeFragment = (Button)rootView.FindViewById(Resource.Id.closeFragment);
            closeFragment.Click += CloseFragment_Click;

            return rootView;
        }

        public void SetContent(string title, string content, string link, Android.Graphics.Bitmap bitmap, string position)
        {
            if (bitmap != null)
            {
                imageView.SetImageBitmap(bitmap);
            }
            else
            {
                imageView.Visibility = ViewStates.Gone;
            }            
            if(title!=null)
                titleView.Text = (title != null ? title : GetString(Resource.String.dialogDefaultTitle));
            if (content != null)
                contentView.Text = (content != null ? content : GetString(Resource.String.dialogDefaultContent));
            if(link != null)
                linkView.Text = (link != null ? link : null);
            if(position != null)
                pageView.Text = (pageView != null ? position : null);
        }

        private void CloseFragment_Click(object sender, System.EventArgs e)
        {
            Activity.OnBackPressed();
        }

        protected override void Dispose(bool disposing)
        {
            System.Threading.Tasks.Task.Delay(200);
            base.Dispose(disposing);
        }
    }
}