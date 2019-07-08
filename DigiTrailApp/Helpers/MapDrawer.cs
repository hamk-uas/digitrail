using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Support.Design.Animation;
using Android.Util;
using Com.Mapbox.Mapboxsdk.Annotations;
using Com.Mapbox.Mapboxsdk.Camera;
using Com.Mapbox.Mapboxsdk.Geometry;
using Com.Mapbox.Mapboxsdk.Maps;
using DigiTrailApp.Azure;
using DigiTrailApp.AzureTables;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// MapDrawer draws gpx route on the map and move the camera
    /// </summary>
    public class MapDrawer : Activity
    {
        MainActivity mainactivity;
        public MapDrawer(MainActivity mainActivity)
        {
            this.mainactivity = mainActivity;          
        }
        /// <summary>
        /// Draws polyline between latitude and longitude pairs (latLng)
        /// </summary>
        /// <param name="latLngPoints">List of LatLng points</param>
        /// <param name="map">MapboxMap object</param>
        /// <param name="width">Width of polyline</param>
        /// <param name="color">Color of polyline</param>
        /// <returns>Created Polyline or null</returns>
        public Polyline DrawPolyLine(PolylineOptions options, List<LatLng> latLngPoints, MapboxMap map,string traiID)
        {
            //Create new camera position
            CameraPosition.Builder position = new CameraPosition.Builder();
            position.Target(latLngPoints[2]) // Sets the new camera position
                .Zoom(15)
                .Bearing(0)
                .Tilt(0);
            map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(position.Build()), 3000);
            if (latLngPoints.Count <= 0)
                throw new ArgumentException("List cannot be empty");
            int current = 0;
            Polyline polyline = null;
            //List<LatLng> currentrunpoints;
            int movecameraEvery = 1;
            int multiplyer = latLngPoints.Count / 100;
            double camZoom = 12;
            if (latLngPoints.Count > 600)
                camZoom = 11;
            ValueAnimator animator = ValueAnimator.OfInt(0, 100);
            animator.SetInterpolator(AnimationUtils.FastOutLinearInInterpolator);
            animator.SetDuration(latLngPoints.Count * 10);
            animator.StartDelay = 3000;
            animator.Start();
            animator.Update += (sender, e) =>
            {           
                // Goes through location list and forms new LatLng objects from latitude and longitude pairs
                // Adds map markers to position of first and last index
                //There is a bug, can't start at 0 point, start adding after there is couple of points
                if (current == 2)
                {
                    foreach (Polyline poly in map.Polylines)
                    {
                        map.RemovePolyline(poly);
                    }
                    map.AddPolyline(options);
                }
                else
                {                    
                    map.UpdatePolyline(options.Polyline);
                }

                if (current < latLngPoints.Count && latLngPoints.Count > (current * multiplyer))
                {
                    options.Add(latLngPoints[current * multiplyer]);
                    position = new CameraPosition.Builder();
                    position.Target(latLngPoints[current * multiplyer]) // Sets the new camera position
                        .Zoom(camZoom)
                        .Bearing(0)
                        .Tilt(0);
                    map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(position.Build()), 1000);
                    movecameraEvery = movecameraEvery + movecameraEvery;
                }
                else
                {
                    int index = multiplyer * (current-1);
                    int countToEnd = latLngPoints.Count - (multiplyer * (current-1));
                    foreach (LatLng pos in latLngPoints.GetRange(index,countToEnd))
                    {
                        options.Add(pos);
                    }
                    animator.Cancel();
                }             
                current++;
            };
            polyline = options.Polyline;
            animator.AnimationEnd += async (sender, e) =>
            {
                var addedpointscount = current * multiplyer;
                if (addedpointscount != latLngPoints.Count)
                {
                    PolylineOptions optionsClear = new PolylineOptions().InvokeWidth(5).InvokeColor(Resource.Color.mapbox_blue);

                    foreach (Polyline poly in map.Polylines)
                    {
                        map.RemovePolyline(poly);
                    }
                    for (int i = 0; i < latLngPoints.Count; i++)
                    {
                        optionsClear.Add(latLngPoints[i]);
                    }
                    map.AddPolyline(optionsClear);

                    polyline = optionsClear.Polyline;                  
                }
                //Create new camera position
                CameraPosition.Builder position2 = new CameraPosition.Builder();
                position2.Target(latLngPoints[2]) // Sets the new camera position
                    .Zoom(14)
                    .Bearing(0)
                    .Tilt(0);
                map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(position2.Build()), 3000);
            };
            return polyline;
        }
    }
}