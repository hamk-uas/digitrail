using System;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using System.IO;
using Com.Mapbox.Mapboxsdk.Geometry;
using DigiTrailApp.Models;

namespace DigiTrailApp.Helpers
{
    class GpxHandler
    {
        /// <summary>
        /// Receives gpx file and returns list of LatLng object created from each node
        /// </summary>
        /// <param name="content">Content of GPX file in string format</param>
        /// <returns>List of LatLng objects containing  latitude, longitude and altitude if available</returns>
        public static List<LatLng> Decode(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("Provided content cannot be null");
            }

            // Create new XmlReader object for content
            XmlReader reader = XmlReader.Create(new StringReader(content));
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

            // List to store LatLng objects
            List<LatLng> list = new List<LatLng>();

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

                list.Add(latLng);
            }

            return list;
        }

        /// <summary>
        /// Receives gpx file and returns list of LatLng object created from each node.
        /// </summary>
        /// <param name="stream">Stream containing GPX file</param>
        /// <returns>List of LatLng objects containing  latitude, longitude and altitude if available</returns>
        public static List<LatLng> Decode(Stream stream)
        {
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

            // List to store LatLng objects
            List<LatLng> list = new List<LatLng>();

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

                list.Add(latLng);
            }

            return list;
        }
    }
}