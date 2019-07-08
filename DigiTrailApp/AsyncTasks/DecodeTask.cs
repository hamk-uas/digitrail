using Android.App;
using Com.Mapbox.Mapboxsdk.Geometry;
using DigiTrailApp.Azure;
using DigiTrailApp.AzureTables;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace DigiTrailApp.AsyncTasks
{
    class DecodeTask
    {
        /// <summary>
        /// Fetchs gpx file from device's memory matching provided id, extracts latitude, longitude and altitude values into LatLng list and returns list
        /// </summary>
        /// <param name="id">Identifier of the trail</param>
        /// <returns></returns>
        public List<LatLng> DecodeGpxById(string id)
        {
            // List containing the waypoints for the trail
            List<LatLng> list = new List<LatLng>();

            // Fetch a trail from DB
            Task<Trail> getTrail = AzureClient.GetTrail(id);
            getTrail.Wait();
            Trail trail = getTrail.Result;

            //Start figuring out the filename of the GPX file
            int pos = trail.File.LastIndexOf("/");
            //Define the path of the file in the filesystem
            string path = Application.Context.FilesDir.AbsolutePath + trail.File.Substring(pos, trail.File.Length - pos);

            //Check that the file exists in the given path
            if (File.Exists(path))
            {
                Stream stream = null;
                try
                {
                    //Open the existing file from memory as a stream
                    stream = File.OpenRead(path);

                    // Make sure that stream is not null
                    if (stream == null)
                    {
                        throw new ArgumentNullException("Provided stream cannot be null");
                    }

                    // Create new XmlReader object for content
                    XmlReader reader = XmlReader.Create(stream);
                    // Lists to store nodes from xml
                    XmlNodeList trackpoints;
                    XmlNodeList elevations;

                    try
                    {
                        // Create new empty XmlDocument object and load reader into it
                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader);

                        // Create new XmlNamespaceManager
                        XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
                        // Add namespace to collection
                        manager.AddNamespace("x", "http://www.topografix.com/GPX/1/1");
                        // Search the document for trkpt and ele nodes
                        trackpoints = doc.SelectNodes("//x:trkpt", manager);
                        elevations = doc.SelectNodes("//x:ele", manager);
                    }
                    finally
                    {
                        // Close reader after trying
                        reader.Close();
                    }

                    for (int i = 0; i < trackpoints.Count; i++)
                    {
                        // Create new LatLng object
                        LatLng latLng = new LatLng()
                        {
                            // Get values for Latitude and Longitude from trackpoints list
                            Latitude = float.Parse(trackpoints[i].Attributes["lat"].InnerText, CultureInfo.InvariantCulture),
                            Longitude = float.Parse(trackpoints[i].Attributes["lon"].InnerText, CultureInfo.InvariantCulture)
                        };

                        // Check if there is elevation value elevations list
                        if (elevations[i] != null)
                        {
                            latLng.Altitude = float.Parse(elevations[i].InnerText, CultureInfo.InvariantCulture);
                        }

                        // Add finalized LatLng object to list
                        list.Add(latLng);
                    }
                }
                finally
                {
                    if (stream != null)
                    {
                        // Remember to close the stream after trying
                        stream.Close();
                    }
                }
            }
            else
            {
                // If the GPX can't be found locally either, throw an error
                throw new IllegalStateException(Application.Context.GetString(Resource.String.errorLoadingTrail));
            }
            // Return LatLng list
            return list;
        }
    }
}