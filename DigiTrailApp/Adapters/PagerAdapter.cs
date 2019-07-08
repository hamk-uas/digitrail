using Android.App;
using Android.Graphics;
using Android.Support.V4.App;
using DigiTrailApp.AzureTables;
using DigiTrailApp.Helpers;
using Java.Lang;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;


namespace DigiTrailApp
{
    /// <summary>
    /// A simple pager adapter that represents <see cref="SlideFragment"/> objects, in sequence.
    /// </summary>
    class PagerAdapter : FragmentStatePagerAdapter
    {
        private readonly List<Fragment> fragmentsList;
        private int pageCount = 1;

        /// <summary>
        /// Constructor of the PagerAdapter class
        /// </summary>
        /// <param name="markerId">ID of the marker in DB</param>
        /// <param name="fm">Android.Support.V4.App.FragmentManager</param>
        public PagerAdapter(string markerId, FragmentManager fm) : base(fm)
        {
            fragmentsList = Task.Run(async () => await GetFragments(markerId)).Result;
        }

        // Returns the number of pages (wizard steps) to show.
        public override int Count { get { return pageCount; } }

        // Returns item from fragmentsList 
        public override Fragment GetItem(int position)
        {
            return fragmentsList[position];
        }

        private async Task<List<Fragment>> GetFragments(string markerId)
        {
            List<Fragment> fragments = new List<Fragment>();
            string title, content, link;
            Bitmap image = null;

            try
            {
                // markerId cannot be null or empty
                if (string.IsNullOrEmpty(markerId))
                {
                    throw new NullPointerException(Application.Context.GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));
                }

                // Fetch Marker from the DB that matche the markerId
                Marker marker = await Azure.AzureClient.GetMarker(markerId);
                // Using markerId query DB for matching ThemeMarker
                List<ThemeMarker> themeMarker = await Azure.AzureClient.GetThemeMarker(markerId);
                List<ThemeMarkerPage> pages = new List<ThemeMarkerPage>();
                if(themeMarker!=null && themeMarker.Count != 0)
                    pages = await Azure.AzureClient.GetThemeMarkerPagesByID(themeMarker[0].Id);
                // Matching ThemeMarker was found
                if (pages.Count > 0)
                {
                    // How many pages need to be shown
                    //List<ThemeMarkerPage> pages = await Azure.AzureClient.GetThemeMarkerPagesByID(themeMarker[0].Id);
                    pageCount = pages.Count;

                    for (int i = 0; i < pages.Count; i++)
                    {

                        // Fetch translation from the DB
                        ThemeMarkerPageTranslation translation = await Azure.AzureClient.GetThemeMarkerPageTranlation(pages[i].Id, LocaleManager.GetLocale());

                        title =  !string.IsNullOrEmpty(translation.Title) ? translation.Title : null;
                        content = !string.IsNullOrEmpty(translation.Text) ? translation.Text : null;
                        link = string.IsNullOrEmpty(translation.Link) ? translation.Link : null;

                        // Check if the Marker has the optional image
                        if (!string.IsNullOrEmpty(pages[i].Image))
                        {
                            if (ServiceInformation.IsConnectionActive())
                            {
                                // Fetch the image as byte array and convert it into a bitmap
                                byte[] vs = await new HttpRequest().GetImage(pages[i].Image);
                                image = await BitmapFactory.DecodeByteArrayAsync(vs, 0, vs.Length);
                            }
                        }

                        // Create a new page - SlideFragment - and add it to the fragments list
                        fragments.Add(new SlideFragment(title, content, link, image, (pageCount > 1 ? (i + 1) + "/" + pageCount : null)));
                    }
                }
                // No ThemeMarker found
                else
                {
                    MarkerTranslation markerTranslation = await Azure.AzureClient.GetMarkerTranslation(markerId, LocaleManager.GetLocale());

                    title = (!string.IsNullOrEmpty(markerTranslation.Title) ? markerTranslation.Title : null);
                    content = (!string.IsNullOrEmpty(markerTranslation.Description) ? markerTranslation.Description : null);
                    link = (!string.IsNullOrEmpty(markerTranslation.Link) ? markerTranslation.Link : null);

                    // Check if the Marker has the optional image
                    if (!string.IsNullOrEmpty(marker.Image))
                    {
                        if (ServiceInformation.IsConnectionActive())
                        {
                            // Fetch the image as byte array and convert it into a bitmap
                            byte[] vs = await new HttpRequest().GetImage(marker.Image);
                            image = await BitmapFactory.DecodeByteArrayAsync(vs, 0, vs.Length);
                        }
                    }
                    fragments.Add(new SlideFragment(title, content, link, image, null));
                }
            }
            catch (HttpRequestException e)
            {
                Android.Util.Log.Error("PagerAdapter", e.ToString());
            }
            catch (ArgumentNullException e)
            {
                Android.Util.Log.Error("PagerAdapter", e.ToString());
            }
            catch (NullPointerException e) { 
                Android.Util.Log.Error("PagerAdapter", e.ToString());
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Android.Util.Log.Error("PagerAdapter", e.ToString());
            }
            catch (UriFormatException e)
            {
                Android.Util.Log.Error("PagerAdapter", e.ToString());
            }
            catch (IllegalStateException e)
            {
                Android.Util.Log.Error("PagerAdapter", e.ToString());
            }
            catch (ArgumentException e)
            {
                Android.Util.Log.Error("PagerAdapter", e.ToString());
            }
            return fragments;
        }
    }
}