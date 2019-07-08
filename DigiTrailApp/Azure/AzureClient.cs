using Android.Content.Res;
using Android.Util;
using Com.Mapbox.Mapboxsdk.Geometry;
using DigiTrailApp.AzureTables;
using Java.Lang;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DigiTrailApp.Azure
{
    /// <summary>
    /// Client for communicating with the Azure Mobile Apps backend
    /// </summary>
    internal class AzureClient : MobileServiceClient
    {
        /// <summary>
        /// Indicates if the client has succesfully fetched data from the backend
        /// </summary>
        internal static bool IsDataFetched { get; private set; }

        /// <summary>
        /// Inidcates of the client is currently fetching data from the backend
        /// </summary>
        internal static bool IsFetchingData { get; private set; }

        /// <summary>
        /// MobileServiceClient used for communicating with Azure
        /// </summary>
        private static AzureClient Client = null;

        /// <summary>
        /// Returns the AzureClient
        /// </summary>
        /// <returns>AzureClient/returns>
        internal static AzureClient GetClient()
        {
            //Check if the client has been initialized
            if (Client == null)
                throw new IllegalStateException(Resources.System.GetString(Resource.String.dbErrorNullClient));

            return Client;
        }

        #region INITIALIZATION
        /// <summary>
        /// Private constructor for client
        /// </summary>
        /// <param name="mobileAppUri">URI of the backend</param>
        private AzureClient(string mobileAppUri) : base(mobileAppUri)
        {
        }

        /// <summary>
        /// Initialize the AzureClient and define local storage
        /// </summary>
        internal static async Task Initialize()
        {
            //Create a new Client
            Client = new AzureClient(Constants.BackendURL);
            //Create a new offline database object
            var store = new MobileServiceSQLiteStore(Constants.Database);
            //Define tables for the local database
            //Unused tables are commented
            //store.DefineTable<Difficulty>();
            //store.DefineTable<DifficultyTranslation>();
            //store.DefineTable<Feedback>();
            //store.DefineTable<Language>();
            store.DefineTable<Location>();
            store.DefineTable<Marker>();
            store.DefineTable<MarkerTranslation>();
            store.DefineTable<MarkerType>();
            //store.DefineTable<MarkerTypeIcon>();
            store.DefineTable<MarkerTypeTranslation>();
            store.DefineTable<ObjectiveThemeMarker>();
            store.DefineTable<ObjectiveThemeMarkerAnswer>();
            store.DefineTable<ObjectiveThemeMarkerAnswerTranslation>();
            store.DefineTable<Theme>();
            store.DefineTable<ThemeMarker>();
            store.DefineTable<ThemeMarkerPage>();
            store.DefineTable<ThemeMarkerPageTranslation>();
            store.DefineTable<ThemeObjective>();
            store.DefineTable<ThemeObjectiveFeedback>();
            store.DefineTable<ThemeObjectiveFeedbackTranslation>();
            store.DefineTable<ThemeTranslation>();
            store.DefineTable<Trail>();
            store.DefineTable<TrailMarker>();
            store.DefineTable<TrailTheme>();
            store.DefineTable<TrailTranslation>();
            store.DefineTable<Version>();
            //Initialize the local database with the definitions
            await Client.SyncContext.InitializeAsync(store);
        }

        /// <summary>
        /// Sync the local Version table against remote table in Azure
        /// </summary>
        internal static async Task RefreshVersion()
        {
            await SyncAsync<Version>();
        }

        /// <summary>
        /// Sync the local tables against remote tables in Azure
        /// </summary>
        internal static async Task RefreshTables()
        {
            //Refresh tables
            //Unused tables are commented
            //await SyncAsync<Difficulty>();
            //await SyncAsync<DifficultyTranslation>();
            //await SyncAsync<Feedback>();
            //await SyncAsync<Language>();
            await SyncAsync<Location>();
            await SyncAsync<Marker>();
            await SyncAsync<MarkerTranslation>();
            await SyncAsync<MarkerType>();
            //await SyncAsync<MarkerTypeIcon>();
            await SyncAsync<MarkerTypeTranslation>();
            await SyncAsync<ObjectiveThemeMarker>();
            await SyncAsync<ObjectiveThemeMarkerAnswer>();
            await SyncAsync<ObjectiveThemeMarkerAnswerTranslation>();
            await SyncAsync<Theme>();
            await SyncAsync<ThemeMarker>();
            await SyncAsync<ThemeMarkerPage>();
            await SyncAsync<ThemeMarkerPageTranslation>();
            await SyncAsync<ThemeObjective>();
            await SyncAsync<ThemeObjectiveFeedback>();
            await SyncAsync<ThemeObjectiveFeedbackTranslation>();
            await SyncAsync<ThemeTranslation>();
            await SyncAsync<Trail>();
            await SyncAsync<TrailMarker>();
            await SyncAsync<TrailTheme>();
            await SyncAsync<TrailTranslation>();
            IsDataFetched = true;
        }

        /// <summary>
        /// Deletes database file in default database path
        /// </summary>
        internal static void DropDatabase()
        {
            // Make sure that database file exists
            if (System.IO.File.Exists(DefaultDatabasePath + "/" + Constants.Database))
            {
                try
                {   // Remove database file
                    System.IO.File.Delete(DefaultDatabasePath + Constants.Database);
                }
                catch (System.IO.IOException ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new System.IO.FileNotFoundException("File not found: " + Constants.Database);
            }
        }

        /// <summary>
        /// Check if Client is already initialized
        /// </summary>
        /// <returns>True if initialized, false if not</returns>
        internal static bool IsInitialized()
        {
            //if (Client != null && Client.SyncContext != null && Client.SyncContext.IsInitialized)
            //    return true;
            //else
            //    return false;
            return Client != null;
        }

        internal static void ResetClient()
        {
            // Using Linq, get all boolean properties in this class
            var booleans = typeof(AzureClient).GetProperties().Where(p => p.PropertyType == typeof(bool));

            foreach (var boolean in booleans)
            {
                // Set value of boolean to false
                boolean.SetValue(boolean, false);
            }

            Client = null;
        }
        #endregion

        #region GENERALIZED QUERIES
        /// <summary>
        /// Fetch all items from the local table to a list
        /// </summary>
        /// <typeparam name="T">DTO class representing a table in Azure</typeparam>
        /// <returns>List containing all items in the local table</returns>
        private static async Task<List<T>> GetListAsync<T>()
        {
            return await Client.GetSyncTable<T>().ToListAsync();
        }

        /// <summary>
        /// Fetch matching item from the local table
        /// </summary>
        /// <typeparam name="T">DTO class representing a row in Azure</typeparam>
        /// <returns>Matching item in the local table</returns>
        private static async Task<T> GetItemAsync<T>(string id)
        {
            if (id == null)
                throw new IllegalStateException(Resources.System.GetString(Resource.String.errorNullParameter));

            return await Client.GetSyncTable<T>().LookupAsync(id);
        }

        /// <summary>
        /// Push all pending local operations to the remote table and pull items from the remote table to the local table.
        /// </summary>
        /// <typeparam name="T">DTO class representing a table in Azure</typeparam>
        /// <returns></returns>
        private static async Task SyncAsync<T>()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            var identifier = typeof(T).Name;
            try
            {
                IsFetchingData = true;
                await Client.SyncContext.PushAsync();
                await Client.GetSyncTable<T>().PullAsync($"all{identifier}", Client.GetSyncTable<T>().CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }
            finally
            {
                IsFetchingData = false;
            }

            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Log.Error(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
        #endregion

        #region LIST QUERIES
        /// <summary>
        /// Fetch all Locations from a local table to a List
        /// </summary>
        /// <returns>List containing all Locations in the local table</returns>
        internal static async Task<List<Location>> GetLocations()
        {
            return await GetListAsync<Location>();
        }

        /// <summary>
        /// Fetch all matching Trails from a local table to a List
        /// </summary>
        /// <returns>List containing all Trails in the local table</returns>
        internal static async Task<List<Trail>> GetTrails(string Location)
        {
            if (Location == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorLocationIsNullOrEmpty));

            return await Client.GetSyncTable<Trail>().Where(Trail => Trail.Location == Location).ToListAsync();
        }

        /// <summary>
        /// Fetch all matching Trails from a local table to a List
        /// </summary>
        /// <returns>List containing all Trails in the local table</returns>
        internal static async Task<List<Trail>> GetTrails()
        {
            return await Client.GetSyncTable<Trail>().ToListAsync();
        }

        internal static async Task<List<string>> GetTrailFiles()
        {
            return await Client.GetSyncTable<Trail>().Select(Trail => Trail.File).ToListAsync();
        }

        /// <summary>
        /// Fetch all Markers matching the Trail ID
        /// </summary>
        /// <param name="trailID">ID of Trail</param>
        /// <returns>List of Markers</returns>
        internal static async Task<List<Marker>> GetTrackedTrailMarkers(string trailID)
        {
            if (trailID == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorTrailIdNullOrEmpty));

            //Initialize a new list for Markers
            List<Marker> markers = new List<Marker>();
            //Fetch all TrailMarkers matching the trailID
            List<TrailMarker> trailMarkers = await Client.GetSyncTable<TrailMarker>().Where(TrailMarker => TrailMarker.Trail == trailID).ToListAsync();
            //Go through the list of Trailmarkers
            foreach (TrailMarker trailMarker in trailMarkers)
            {
                //Fetch a Marker matching the Marker ID in TrailMarker row, and add it to the List
                markers.Add(await GetMarker(trailMarker.Marker));
            }
            //Return the list
            return markers;
        }

        /// <summary>
        /// Fetch all Markers that are shown to the user matching the Trail ID 
        /// </summary>
        /// <param name="trailID">ID of Trail</param>
        /// <returns>List of Markers</returns>
        internal static async Task<List<Marker>> GetShownTrailMarkers(string trailID)
        {
            if (trailID == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorTrailIdNullOrEmpty));

            //Initialize a new list for Markers
            List<Marker> markers = new List<Marker>();
            //Fetch all TrailMarkers matching the trailID
            List<TrailMarker> trailMarkers = await Client.GetSyncTable<TrailMarker>().Where(TrailMarker => TrailMarker.Trail == trailID).ToListAsync();
            //Get a list of MarkerTypes where ShowToUser boolean is false
            List<MarkerType> markerTypes = await GetMarkerTypes(false);
            //Go through the list of Trailmarkers
            foreach (TrailMarker trailMarker in trailMarkers)
            {
                //Fetch a Marker matching the Marker ID in TrailMarker row
                Marker marker = await GetMarker(trailMarker.Marker);
                //Go through the fetched MarkerTypes
                foreach (MarkerType markerType in markerTypes)
                {
                    //If the MarkerType of the Marker matches with one in the list
                    if (markerType.Id != marker.MarkerType)
                        //Add the Marker to the list
                        markers.Add(marker);
                }

            }
            return markers;
        }

        /// <summary>
        /// Fetch all Markers matching the Theme ID
        /// </summary>
        /// <param name="themeID">ID of Theme</param>
        /// <returns>List of Markers</returns>
        internal static async Task<List<Marker>> GetTrackedThemeMarkers(string themeID)
        {
            if (themeID == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorThemeIdNullOrEmpty));

            //Initialize a new list for Markers
            List<Marker> markers = new List<Marker>();
            //Fetch all ThemeMarkers matching the Theme ID
            List<ThemeMarker> themeMarkers = await Client.GetSyncTable<ThemeMarker>().Where(ThemeMarker => ThemeMarker.Theme == themeID).ToListAsync();
            //Go through the list of ThemeMarkers
            foreach (ThemeMarker themeMarker in themeMarkers)
            {
                //Fetch a Marker matching the Marker ID in ThemeMarker row, and add it to the List
                markers.Add(await GetMarker(themeMarker.Marker));
            }
            return markers;
        }

        /// <summary>
        /// Fetch all ThemeMarkers that are shown to the user matching the Theme ID 
        /// </summary>
        /// <param name="themeID">ID of Theme</param>
        /// <returns>List of Markers</returns>
        internal static async Task<List<Marker>> GetShownThemeMarkers(string themeID)
        {
            if (themeID == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorThemeIdNullOrEmpty));

            //Initialize a new list for Markers
            List<Marker> markers = new List<Marker>();
            //Fetch all ThemeMarkers matching the Theme ID
            List<ThemeMarker> themeMarkers = await Client.GetSyncTable<ThemeMarker>().Where(ThemeMarker => ThemeMarker.Theme == themeID).ToListAsync();
            //Get a list of MarkerTypes where ShowToUser boolean is false
            List<MarkerType> markerTypes = await GetMarkerTypes(false);
            //Go through the list of ThemeMarkers
            foreach (ThemeMarker themeMarker in themeMarkers)
            {
                //Fetch a Marker matching the Marker ID in ThemeMarker row
                Marker marker = await GetMarker(themeMarker.Marker);
                //Go through the fetched MarkerTypes
                foreach (MarkerType markerType in markerTypes)
                {
                    //If the MarkerType of the Marker matches with one in the list
                    if (markerType.Id != marker.MarkerType)
                        //Add the Marker to the list
                        markers.Add(marker);
                }

            }
            return markers;
        }

        /// <summary>
        /// Fetch all ObjectiveThemeMarkers that are shown to the user matching the Theme ID
        /// </summary>
        /// <param name="themeID">ID of Theme</param>
        /// <returns>List of all matching Markers</returns>
        internal static async Task<List<Marker>> GetShownObjectiveThemeMarkers(string themeID)
        {
            if (themeID == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorThemeIdNullOrEmpty));

            //Initialize a new list for Markers
            List<Marker> markers = new List<Marker>();
            //Check that the given Theme has an Objective
            if(await ThemeHasObjective(themeID))
            {
                ThemeObjective themeObjective = await GetThemeObjective(themeID);
                //Fetch all ThemeMarkers matching the Theme ID
                List<ObjectiveThemeMarker> objectiveThemeMarkers = await Client.GetSyncTable<ObjectiveThemeMarker>().Where(ObjectiveThemeMarker => ObjectiveThemeMarker.ThemeObjective == themeObjective.Id).ToListAsync();
                //Get a list of MarkerTypes where ShowToUser boolean is false
                List<MarkerType> markerTypes = await GetMarkerTypes(false);
                //Go through the list of ThemeMarkers
                foreach (ObjectiveThemeMarker objectiveThemeMarker in objectiveThemeMarkers)
                {
                    //Fetch a Marker matching the Marker ID in ThemeMarker row
                    Marker marker = await GetMarker(objectiveThemeMarker.Marker);
                    //Go through the fetched MarkerTypes
                    foreach (MarkerType markerType in markerTypes)
                    {
                        //If the MarkerType of the Marker matches with one in the list
                        if (markerType.Id != marker.MarkerType)
                            //Add the Marker to the list
                            markers.Add(marker);
                    }
                }
            }

            return markers;
        }

        /// <summary>
        /// Fetch a list of MarkerTypes from the local DB
        /// </summary>
        /// <param name="showToUser">Column indicating if user is supposed to see the Markers of this type</param>
        /// <returns>List of matching MarkerTypes</returns>
        internal static async Task<List<MarkerType>> GetMarkerTypes(bool showToUser)
        {
            return await Client.GetSyncTable<MarkerType>().Where(MarkerType => MarkerType.ShowToUser == showToUser).ToListAsync();
        }

        /// <summary>
        /// Fetch all Markers matching the Theme ID
        /// </summary>
        /// <param name="themeID">ID of Theme</param>
        /// <returns>List of Markers</returns>
        internal static async Task<List<Marker>> GetThemeMarkers(string themeID)
        {
            if (themeID == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorThemeIdNullOrEmpty));

            //Initialize a new list for Markers
            List<Marker> markers = new List<Marker>();
            //Fetch all ThemeMarkers matching the Theme ID
            List<ThemeMarker> themeMarkers = await Client.GetSyncTable<ThemeMarker>().Where(ThemeMarker => ThemeMarker.Theme == themeID).ToListAsync();
            //Go through the list of ThemeMarkers
            foreach (ThemeMarker themeMarker in themeMarkers)
            {
                //Fetch a Marker matching the Marker ID in ThemeMarker row, and add it to the List
                markers.Add(await GetMarker(themeMarker.Marker));
            }
            return markers;
        }

        /// <summary>
        /// Fetch a List of ThemeMarkerPages
        /// </summary>
        /// <param name="themeMarkerID">ID of ThemeMarker</param>
        /// <returns>List of ThemeMarkerPages</returns>
        internal static async Task<List<ThemeMarkerPage>> GetThemeMarkerPagesByID(string themeMarkerID)
        {
            if (themeMarkerID == null)
            {
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorThemeIdNullOrEmpty));
            }

            return await Client.GetSyncTable<ThemeMarkerPage>().Where(ThemeMarkerPage => ThemeMarkerPage.ThemeMarker == themeMarkerID).OrderBy(ThemeMarkerPage => ThemeMarkerPage.OrderNumber).ToListAsync();
        }

        /// <summary>
        /// Fetch all Markers shown by default on the map from a local table to a List
        /// </summary>
        /// <returns>List of Markers</returns>
        internal static async Task<List<Marker>> GetDefaultShownMarkers()
        {
            //Fetch all Markers to lists
            List<Marker> markers = await GetListAsync<Marker>();
            List<ThemeMarker> themeMarkers = await GetListAsync<ThemeMarker>();
            List<TrailMarker> trailMarkers = await GetListAsync<TrailMarker>();
            List<ObjectiveThemeMarker> objectiveThemeMarkers = await GetListAsync<ObjectiveThemeMarker>();
            //Fetch all markertypes where ShowToUser is false
            List<MarkerType> notShown = await GetMarkerTypes(false);

            //List containing the duplicates
            List<Marker> duplicates = new List<Marker>();

            //Check that each list contains matches
            if (themeMarkers.Count > 0)
            {
                //Get an IEnumerable containing all Markers that exist in ThemeMarkers
                IEnumerable<Marker> matches = from th in themeMarkers
                                              from m in markers
                                              where (th.Marker == m.Id)
                                              select m;
                //Add the matches to the duplicates
                duplicates.AddRange(matches);
            }
            if (trailMarkers.Count > 0)
            {
                //Get an IEnumerable containing all Markers that exist in ThemeMarkers
                IEnumerable<Marker> matches = from tr in trailMarkers
                                              from m in markers
                                              where (tr.Marker == m.Id)
                                              select m;
                //Add the matches to the duplicates
                duplicates.AddRange(matches);
            }
            if(objectiveThemeMarkers.Count > 0)
            {
                //Get an IEnumerable containing all Markers that exist in ObjectiveThemeMarkers
                IEnumerable<Marker> matches = from otm in objectiveThemeMarkers
                                              from m in markers
                                              where (otm.Marker == m.Id)
                                              select m;
                //Add the matches to the duplicates
                duplicates.AddRange(matches);
            }
            if (notShown.Count > 0)
            {
                //Get an IEnumerable containing all Markers that exist in ThemeMarkers
                IEnumerable<Marker> matches = from mt in notShown
                                              from m in markers
                                              where (mt.Id == m.MarkerType)
                                              select m;
                //Add the matches to the duplicates
                duplicates.AddRange(matches);
            }

            //Return all Markers EXCEPT the duplicates
            return markers.Except(duplicates).ToList();
        }

        /// <summary>
        /// Fetch all Markers that RangeService needs to track
        /// </summary>
        /// <returns>List of Markers</returns>
        internal static async Task<List<Marker>> GetDefaultTrackedMarkers(LatLng position)
        {
            if (position == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorPositionNullOrEmpty));

            //Fetch all Markers within 1 square Km to list
            List<Marker> markers = await GetMarkersAroundPosition(position);
            //Fetch Theme, Trail and ObjectiveThemeMarkers to their own lists
            List<ThemeMarker> themeMarkers = await GetListAsync<ThemeMarker>();
            List<TrailMarker> trailMarkers = await GetListAsync<TrailMarker>();
            List<ObjectiveThemeMarker> objectiveThemeMarkers = await GetListAsync<ObjectiveThemeMarker>();

            //List containing the duplicates
            List<Marker> duplicates = new List<Marker>();

            //Check that each list contains matches
            if (themeMarkers.Count > 0)
            {
                //Get an IEnumerable containing all Markers that exist in ThemeMarkers
                IEnumerable<Marker> matches = from th in themeMarkers
                                              from m in markers
                                              where (th.Marker == m.Id)
                                              select m;
                //Add the matches to the duplicates
                duplicates.AddRange(matches);
            }
            if (trailMarkers.Count > 0)
            {
                //Get an IEnumerable containing all Markers that exist in TrailMarkers
                IEnumerable<Marker> matches = from tr in trailMarkers
                                              from m in markers
                                              where (tr.Marker == m.Id)
                                              select m;
                //Add the matches to the duplicates
                duplicates.AddRange(matches);
            }
            if (objectiveThemeMarkers.Count > 0)
            {
                //Get an IEnumerable containing all Markers that exist in ObjectiveThemeMarkers
                IEnumerable<Marker> matches = from otm in objectiveThemeMarkers
                                              from m in markers
                                              where (otm.Id == m.Id)
                                              select m;
                //Add the matches to the duplicates
                duplicates.AddRange(matches);
            }

            //Return all Markers EXCEPT the duplicates
            return markers.Except(duplicates).ToList();
        }

        /// <summary>
        /// Fetch Markers from local DB within 1 square Km of the given position
        /// </summary>
        /// <param name="position">Position for the query</param>
        /// <returns>List of Markers</returns>
        internal static async Task<List<Marker>> GetMarkersAroundPosition(LatLng position)
        {
            if (position == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorPositionNullOrEmpty));

            //Calculate the longitude at the current latitude
            double lonInKm = CalcLat(position.Latitude);
            //Fetch the markers from DB
            return await Client.GetSyncTable<Marker>().Where(Marker => Marker.Lat < position.Latitude + Constants.LatInKm
            && Marker.Lat > position.Latitude - Constants.LatInKm
            && Marker.Lon < position.Longitude + lonInKm
            && Marker.Lon > position.Longitude - lonInKm).ToListAsync();
        }

        /// <summary>
        /// Fetch TrailMarkers from local DB within 1 square Km of the given position
        /// </summary>
        /// <param name="trailID">ID of a Trail</param>
        /// <param name="position">Position for the query</param>
        /// <returns>List of Markers</returns>
        internal static async Task<List<Marker>> GetTrailMarkersAroundPosition(string trailID, LatLng position)
        {
            if (position == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorPositionNullOrEmpty));

            //Initialize a new list for Markers
            List<Marker> markers = new List<Marker>();
            //Fetch all ThemeMarkers matching the Theme ID
            List<TrailMarker> trailMarkers = await Client.GetSyncTable<TrailMarker>().Where(TrailMarker => TrailMarker.Trail == trailID).ToListAsync();
            //Go through the list of ThemeMarkers
            foreach (TrailMarker trailMarker in trailMarkers)
            {
                //Fetch a Marker matching the Marker ID in TrailMarker row
                Marker marker = await GetMarker(trailMarker.Marker);
                //Calculate the longitude at the current latitude
                double lonInKm = CalcLat(position.Latitude);
                //Check if the marker is within 1sqkm from the given position
                if (marker.Lat < position.Latitude + Constants.LatInKm
                    && marker.Lat > position.Latitude - Constants.LatInKm
                    && marker.Lon < position.Longitude + lonInKm
                    && marker.Lon > position.Longitude - lonInKm)
                    //Add it to the List
                    markers.Add(await GetMarker(trailMarker.Marker));
            }
            return markers;
        }

        /// <summary>
        /// Fetch ThemeMarkers from local DB within 1 square Km of the given position
        /// </summary>
        /// <param name="themeID">ID of a Theme</param>
        /// <param name="position">Position for the query</param>
        /// <returns>List of Markers</returns>
        internal static async Task<List<Marker>> GetThemeMarkersAroundPosition(string themeID, LatLng position)
        {            
            if (position == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorPositionNullOrEmpty));

            //Initialize a new list for Markers
            List<Marker> markers = new List<Marker>();
            //Fetch all ThemeMarkers matching the Theme ID
            List<ThemeMarker> themeMarkers = await Client.GetSyncTable<ThemeMarker>().Where(ThemeMarker => ThemeMarker.Theme == themeID).ToListAsync();
            //Go through the list of ThemeMarkers
            foreach (ThemeMarker themeMarker in themeMarkers)
            {
                //Fetch a Marker matching the Marker ID in ThemeMarker row
                Marker marker = await GetMarker(themeMarker.Marker);
                //Calculate the longitude at the current latitude
                double lonInKm = CalcLat(position.Latitude);
                //Check if the marker is within 1sqkm from the given position
                if (marker.Lat < position.Latitude + Constants.LatInKm
                    && marker.Lat > position.Latitude - Constants.LatInKm
                    && marker.Lon < position.Longitude + lonInKm
                    && marker.Lon > position.Longitude - lonInKm)
                    //Add it to the List
                    markers.Add(await GetMarker(themeMarker.Marker));
            }
            return markers;
        }

        /// <summary>
        /// Fetch ObjectiveThemeMarkers from local DB within 1 square Km of the given position
        /// </summary>
        /// <param name="themeID">ID of Theme</param>
        /// <param name="position">Position for the query</param>
        /// <returns>List of ObjectiveThemeMarkers</returns>
        internal static async Task<List<Marker>> GetObjectiveThemeMarkersAroundPosition(string themeID, LatLng position)
        {
            if (position == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorPositionNullOrEmpty));

            ThemeObjective themeObjective = await GetThemeObjective(themeID);
            //Initialize a new list for Markers
            List<Marker> markers = new List<Marker>();
            //Fetch all ThemeMarkers matching the Theme ID
            List<ObjectiveThemeMarker> ObjectiveThemeMarkers = await Client.GetSyncTable<ObjectiveThemeMarker>().Where(ObjectiveThemeMarker => ObjectiveThemeMarker.ThemeObjective == themeObjective.Id).ToListAsync();
            //Go through the list of ThemeMarkers
            foreach (ObjectiveThemeMarker objectiveThemeMarker in ObjectiveThemeMarkers)
            {
                //Fetch a Marker matching the Marker ID in ObjectiveThemeMarker row
                Marker marker = await GetMarker(objectiveThemeMarker.Marker);
                //Calculate the longitude at the current latitude
                double lonInKm = CalcLat(position.Latitude);
                //Check if the marker is within 1sqkm from the given position
                if (marker.Lat < position.Latitude + Constants.LatInKm
                    && marker.Lat > position.Latitude - Constants.LatInKm
                    && marker.Lon < position.Longitude + lonInKm
                    && marker.Lon > position.Longitude - lonInKm)
                    //Add it to the List
                    markers.Add(await GetMarker(objectiveThemeMarker.Marker));
            }
            return markers;
        }

        /// <summary>
        /// Fetch a list of ObjectiveThemeMarkerAnswers for the ObjectiveThemeMarker
        /// </summary>
        /// <param name="objetiveThemeMarkerID">ID of ObjectiveThemeMarker</param>
        /// <returns>List of ObjectiveThemeMarkerAnswers</returns>
        internal static async Task<List<ObjectiveThemeMarkerAnswer>> GetObjectiveThemeMarkerAnswers(string objetiveThemeMarkerID)
        {
            if (objetiveThemeMarkerID == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));

            return await Client.GetSyncTable<ObjectiveThemeMarkerAnswer>().Where(ObjectiveThemeMarkerAnswer => ObjectiveThemeMarkerAnswer.ObjectiveThemeMarker == objetiveThemeMarkerID).ToListAsync();
        }

        /// <summary>
        /// Fetch a list of ObjectiveThemeMarkers for the given theme
        /// </summary>
        /// <param name="themeID">ID of Theme</param>
        /// <returns>List of ObjectiveThemeMarkers</returns>
        internal static async Task<List<ObjectiveThemeMarker>> GetObjectiveThemeMarkers(string themeID)
        {
            if (themeID == null)
                throw new NullPointerException("Error in database query! Given ID is null!");

            ThemeObjective themeObjective = await GetThemeObjective(themeID);
            return await Client.GetSyncTable<ObjectiveThemeMarker>().Where(ObjectiveThemeMarker => ObjectiveThemeMarker.ThemeObjective == themeObjective.Id).ToListAsync();
        }
        #endregion

        #region INDIVIDUAL QUERIES
        /// <summary>
        /// Fetch a Marker from the local DB
        /// </summary>
        /// <param name="id">ID of Marker</param>
        /// <returns>Matching Marker</returns>
        internal static async Task<Marker> GetMarker(string id)
        {
            if (id == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));

            return await GetItemAsync<Marker>(id);
        }

        /// <summary>
        /// Fetch a MarkerType from the local DB
        /// </summary>
        /// <param name="id">ID of MarkerType</param>
        /// <returns>Matching MarkerType</returns>
        internal static async Task<MarkerType> GetMarkerType(string id)
        {
            if (id == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorMarkerTypeIdNullOrEmpty));

            return await GetItemAsync<MarkerType>(id);
        }

        /// <summary>
        /// Fetch a Trail from the local DB
        /// </summary>
        /// <param name="id">ID of Trail</param>
        /// <returns>Matching Trail</returns>
        internal static async Task<Trail> GetTrail(string id)
        {
            if (id == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorTrailIdNullOrEmpty));

            return await GetItemAsync<Trail>(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">DTO class representing a row in Azure. Target of search</typeparam>
        /// <param name="p0">Query value</param>
        /// <returns>True if a match was found</returns>
        internal static async Task<List<ThemeMarker>> GetThemeMarker(string p0)
        {
            if (p0 == null)
            {
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorTrailIdNullOrEmpty));
            }

            return await Client.GetSyncTable<ThemeMarker>().Where(ThemeMarker => ThemeMarker.Marker == p0).ToListAsync();
        }

        /// <summary>
        /// Returns the default theme for a trail
        /// </summary>
        /// <param name="trailId">ID of the trail or NULL if trail doesn't have a default theme</param>
        /// <returns></returns>
        internal static async Task<Theme> GetDefaultTheme(string trailId)
        {
            List<TrailTheme> trailThemeList = await Client.GetSyncTable<TrailTheme>().Where(TrailTheme => TrailTheme.Trail == trailId && TrailTheme.Default == true).ToListAsync();
            if (trailThemeList != null && trailThemeList.Count > 0)
                return await GetItemAsync<Theme>(trailThemeList.FirstOrDefault().Theme);
            else
                return null;
        }

        /// <summary>
        /// Check if a Theme has an Objective
        /// True indicates that a theme has an Objective
        /// </summary>
        /// <param name="themeId">ID of a Theme</param>
        /// <returns>True if a Theme has an Objective, otherwise false</returns>
        internal async static Task<bool> ThemeHasObjective(string themeId)
        {
            ThemeObjective themeObjective = await GetThemeObjective(themeId);
            return themeObjective != null;
        }

        /// <summary>
        /// Check if the particular marker is an ObjectiveThemeMarker.
        /// True if an ObjectiveThemeMarker is found with the Marker ID, otherwise false
        /// </summary>
        /// <param name="markerID">ID of Marker</param>
        /// <returns>True if an ObjectiveThemeMarker is found with the Marker ID, otherwise false</returns>
        internal async static Task<bool> IsObjectiveMarker(string markerID)
        {
            if (markerID == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));

            List<ObjectiveThemeMarker> result = await Client.GetSyncTable<ObjectiveThemeMarker>().Where(ObjectiveThemeMarker => ObjectiveThemeMarker.Marker == markerID).ToListAsync();

            return result != null && result.Count > 0;
        }

        /// <summary>
        /// Check if a particular <see cref="ThemeObjective"/> has a <see cref="ThemeObjectiveFeedback"/>.
        /// True if a <see cref="ThemeObjectiveFeedback"/> is found with <see cref="ThemeObjective"/> ID, otherwise false
        /// </summary>
        /// <param name="themeObjectiveID">ID of <see cref="ThemeObjective"/></param>
        /// <returns>True if a <see cref="ThemeObjectiveFeedback"/> is found with <see cref="ThemeObjective"/> ID, otherwise false</returns>
        internal async static Task<bool> ThemeObjectiveHasFeedback(string themeObjectiveID)
        {
            if (themeObjectiveID == null)
                throw new NullPointerException("ID is null!");

            List<ThemeObjectiveFeedback> result = await Client.GetSyncTable<ThemeObjectiveFeedback>().Where(ThemeObjectiveFeedback => ThemeObjectiveFeedback.ThemeObjective == themeObjectiveID).ToListAsync();

            return result != null && result.Count > 0;
        }

        /// <summary>
        /// Fetch an ObjectiveThemeMarker with the ID given as parameter
        /// </summary>
        /// <param name="markerID">ID of Marker</param>
        /// <returns>ObjectiveThemeMarker associated with given Marker ID</returns>
        internal async static Task<ObjectiveThemeMarker> GetObjectiveThemeMarker(string markerID)
        {
            if (markerID == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));

            List <ObjectiveThemeMarker> result = await Client.GetSyncTable<ObjectiveThemeMarker>().Where(ObjectiveThemeMarker => ObjectiveThemeMarker.Marker == markerID).ToListAsync();
            return result.FirstOrDefault();

        }

        /// <summary>
        /// Fetch latest version from from local db
        /// </summary>
        /// <returns>Object of latest version containing version name and code</returns>
        internal async static Task<Version> GetVersion()
        {
            List<Version> result = await Client.GetSyncTable<Version>().OrderBy(Version => Version.VersionCode).ToListAsync();
            return result.LastOrDefault();
        }

        /// <summary>
        /// Fetch a ThemeObjective for given Theme
        /// </summary>
        /// <param name="themeId">ID of Theme</param>
        /// <returns>ThemeObjective or NULL if no Objective is found</returns>
        internal async static Task<ThemeObjective> GetThemeObjective(string themeId)
        {
            if (themeId == null)
                throw new NullPointerException("Error in database query! Given ID is null!");

            List<ThemeObjective> result = await Client.GetSyncTable<ThemeObjective>().Where(ThemeObjective => ThemeObjective.Theme == themeId).ToListAsync();
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Fetch a ThemeObjectiveFeedback for given Theme
        /// </summary>
        /// <param name="themeID">ID of Theme</param>
        /// <param name="userScore">Score of the user</param>
        /// <returns>ThemeObjective or NULL if no Objective is found</returns>
        internal async static Task<ThemeObjectiveFeedback> GetThemeObjectiveFeedback(string themeID, int userScore)
        {
            if (themeID == null)
                throw new NullPointerException("Error in database query! Given ID is null!");

            ThemeObjective objective = await GetThemeObjective(themeID);
            List<ThemeObjectiveFeedback> result = await Client.GetSyncTable<ThemeObjectiveFeedback>().Where(ThemeObjectiveFeedback => userScore >= ThemeObjectiveFeedback.ScoreMin && userScore <= ThemeObjectiveFeedback.ScoreMax && ThemeObjectiveFeedback.ThemeObjective == objective.Id).ToListAsync();
            return result.FirstOrDefault();
        }
        #endregion

        #region TRANSLATION QUERIES
        //TODO: Can these queries be generalized?
        /// <summary>
        /// Fetch a MarkerTranslation for a Marker from the local db
        /// </summary>
        /// <param name="id">ID of Marker</param>
        /// <param name="language">Language code for translation</param>
        /// <returns>MarkerTranslation of matching language, MarkerTranslation of default language</returns>
        internal async static Task<MarkerTranslation> GetMarkerTranslation(string id, string language)
        {
            if (id == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));

            if (language == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorLanguageNullOrEmpty));

            //Fetch a list of MarkerTranslation(s) with given language
            List<MarkerTranslation> markerTranslations = await Client.GetSyncTable<MarkerTranslation>().Where(MarkerTranslation => MarkerTranslation.Marker == id && MarkerTranslation.Language == language).OrderBy(MarkerTranslation => MarkerTranslation.UpdatedAt).ToListAsync();
            //Check if we get a result with the given language
            if (markerTranslations.Count > 0)
                return markerTranslations.First();
            else
            {
                //Fetch default language
                //TODO: Could we fetch the default column value for Language column instead of this query?
                markerTranslations = await Client.GetSyncTable<MarkerTranslation>().Where(MarkerTranslation => MarkerTranslation.Marker == id && MarkerTranslation.Language == Constants.DefaultLang).OrderBy(MarkerTranslation => MarkerTranslation.UpdatedAt).ToListAsync();
                if (markerTranslations.Count > 0)
                    return markerTranslations.First();
            }

            //Return MarkerTranslation with default hardcoded values
            return new MarkerTranslation();
        }

        /// <summary>
        /// Fetch a TrailTranslation for a Trail from the local db
        /// </summary>
        /// <param name="id">ID of Trail</param>
        /// <param name="language">Language code for translation</param>
        /// <returns>TrailTranslation of matching language or TrailTranslation of default language</returns>
        internal async static Task<TrailTranslation> GetTrailTranslation(string id, string language)
        {
            if (id == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorMarkerIdNullOrEmpty));

            if (language == null)
                throw new NullPointerException(Resources.System.GetString(Resource.String.dbErrorLanguageNullOrEmpty));

            //Fetch a list of TrailTranslation(s) with given language
            List<TrailTranslation> result = await Client.GetSyncTable<TrailTranslation>().Where(TrailTranslation => TrailTranslation.Trail == id && TrailTranslation.Language == language).OrderBy(TrailTranslation => TrailTranslation.UpdatedAt).ToListAsync();
            //Check if we get a result with the given language
            if (result.Count > 0)
                return result.First();
            else
            {
                //Fetch default language
                //TODO: Could we fetch the default column value for Language column instead of this query?
                result = await Client.GetSyncTable<TrailTranslation>().Where(TrailTranslation => TrailTranslation.Trail == id && TrailTranslation.Language == Constants.DefaultLang).OrderBy(TrailTranslation => TrailTranslation.UpdatedAt).ToListAsync();
                if (result.Count > 0)
                    return result.First();
            }

            //Return TrailTranslation with default hardcoded values
            return new TrailTranslation();
        }

        /// <summary>
        /// Fetch a ObjectiveThemeMarkerAnswerTranslation for an ObjectiveThemeMarkerAnswer from the local DB
        /// </summary>
        /// <param name="objectiveThemeMarkerAnswerID">ID of ObjectiveThemeMarkerAnswer</param>
        /// <param name="language">Language code for translation</param>
        /// <returns>ObjectiveThemeMarkerAnswerTranslation of matching language or ObjectiveThemeMarkerAnswerTranslation of default language</returns>
        internal async static Task<ObjectiveThemeMarkerAnswerTranslation> GetObjectiveThemeMarkerAnswerTranslation(string objectiveThemeMarkerAnswerID, string language)
        {
            if (objectiveThemeMarkerAnswerID == null)
                throw new NullPointerException("ID is null!");

            if (language == null)
                throw new NullPointerException("Given language is null!");

            //Fetch a list of ObjectiveThemeMarkerAnswerTranslation(s) with given language
            List<ObjectiveThemeMarkerAnswerTranslation> result = await Client.GetSyncTable<ObjectiveThemeMarkerAnswerTranslation>().Where(ObjectiveThemeMarkerAnswerTranslation => ObjectiveThemeMarkerAnswerTranslation.ObjectiveThemeMarkerAnswer == objectiveThemeMarkerAnswerID && ObjectiveThemeMarkerAnswerTranslation.Language == language).OrderBy(ObjectiveThemeMarkerAnswerTranslation => ObjectiveThemeMarkerAnswerTranslation.UpdatedAt).ToListAsync();
            //Check if we get a result with the given language
            if (result.Count > 0)
                return result.First();
            else
            {
                //Fetch default language
                //TODO: Could we fetch the default column value for Language column instead of this query?
                result = await Client.GetSyncTable<ObjectiveThemeMarkerAnswerTranslation>().Where(ObjectiveThemeMarkerAnswerTranslation => ObjectiveThemeMarkerAnswerTranslation.ObjectiveThemeMarkerAnswer == objectiveThemeMarkerAnswerID && ObjectiveThemeMarkerAnswerTranslation.Language == Constants.DefaultLang).OrderBy(ObjectiveThemeMarkerAnswerTranslation => ObjectiveThemeMarkerAnswerTranslation.UpdatedAt).ToListAsync();
                if (result.Count > 0)
                    return result.First();
            }

            //Return ObjectiveThemeMarkerAnswerTranslation with default hardcoded values
            return new ObjectiveThemeMarkerAnswerTranslation();
        }

        /// <summary>
        /// Fetch a <see cref="ThemeMarkerPageTranslation"/> for an <see cref="ThemeMarkerPage"/> from the local DB
        /// </summary>
        /// <param name="pageNumber">ID of <see cref="ThemeMarkerPage"/></param>
        /// <param name="language"><see cref="Language"/> code for translation</param>
        /// <returns><see cref="ThemeMarkerPageTranslation"/> of matching <see cref="Language"/> or <see cref="ThemeMarkerPageTranslation"/> of default <see cref="Language"/></returns>
        internal static async Task<ThemeMarkerPageTranslation> GetThemeMarkerPageTranlation(string pageNumber, string language)
        {
            if (pageNumber == null)
                throw new NullPointerException("ID is null!");

            if (language == null)
                throw new NullPointerException("Given language is null!");

            List<ThemeMarkerPageTranslation> result = await Client.GetSyncTable<ThemeMarkerPageTranslation>().Where(ThemeMarkerPageTranslation => ThemeMarkerPageTranslation.ThemeMarkerPage == pageNumber && ThemeMarkerPageTranslation.Language == language).OrderBy(ThemeMarkerPageTranslation => ThemeMarkerPageTranslation.UpdatedAt).ToListAsync();
            if (result.Count > 0)
                return result.First();
            else
            {
                //Fetch default language
                //TODO: Could we fetch the default column value for Language column instead of this query?
                result = await Client.GetSyncTable<ThemeMarkerPageTranslation>().Where(ThemeMarkerPageTranslation => ThemeMarkerPageTranslation.ThemeMarkerPage == pageNumber && ThemeMarkerPageTranslation.Language == Constants.DefaultLang).OrderBy(ThemeMarkerPageTranslation => ThemeMarkerPageTranslation.UpdatedAt).ToListAsync();
                if (result.Count > 0)
                    return result.First();
            }

            return new ThemeMarkerPageTranslation();
        }

        /// <summary>
        /// Fetch a <see cref="ThemeObjectiveFeedbackTranslation"/> for an <see cref="ThemeObjectiveFeedback"/> from the local DB
        /// </summary>
        /// <param name="themeObjectiveFeedbackID">ID of <see cref="ThemeObjectiveFeedback"/></param>
        /// <param name="language"><see cref="Language"/> code for translation</param>
        /// <returns><see cref="ThemeObjectiveFeedbackTranslation"/> of matching <see cref="Language"/> or <see cref="ThemeObjectiveFeedbackTranslation"/> of default <see cref="Language"/></returns>
        internal static async Task<ThemeObjectiveFeedbackTranslation> GetThemeObjectiveFeedbackTranslation(string themeObjectiveFeedbackID, string language)
        {
            if (themeObjectiveFeedbackID == null)
                throw new NullPointerException("ID is null!");

            if (language == null)
                throw new NullPointerException("Given language is null!");

            List<ThemeObjectiveFeedbackTranslation> result = await Client.GetSyncTable<ThemeObjectiveFeedbackTranslation>().Where(ThemeObjectiveFeedbackTranslation => ThemeObjectiveFeedbackTranslation.ThemeObjectiveFeedback == themeObjectiveFeedbackID && ThemeObjectiveFeedbackTranslation.Language == language).OrderBy(ThemeObjectiveFeedbackTranslation => ThemeObjectiveFeedbackTranslation.UpdatedAt).ToListAsync();
            if (result.Count > 0)
                return result.First();
            else
            {
                result = await Client.GetSyncTable<ThemeObjectiveFeedbackTranslation>().Where(ThemeObjectiveFeedbackTranslation => ThemeObjectiveFeedbackTranslation.ThemeObjectiveFeedback == themeObjectiveFeedbackID && ThemeObjectiveFeedbackTranslation.Language == Constants.DefaultLang).OrderBy(ThemeObjectiveFeedbackTranslation => ThemeObjectiveFeedbackTranslation.UpdatedAt).ToListAsync();
                if (result.Count > 0)
                    return result.First();
            }

            return new ThemeObjectiveFeedbackTranslation();
        }
        #endregion

        #region  INTERNAL FUNCTIONS
        /// <summary>
        /// Calculates one kilometer of longitude in decimals at the given latitude
        /// </summary>
        /// <param name="latitude">Degrees of latitude</param>
        /// <returns>One kilometer of longitude in decimals</returns>
        private static double CalcLat(double latitude)
        {
            return 360 / (Math.Cos(Math.ToRadians(latitude)) * 40075);
        }
        #endregion
    }
}