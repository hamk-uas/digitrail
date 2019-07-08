using System;
using System.Collections.Generic;
using Android.OS;
using DigiTrailApp.AzureTables;
using DigiTrailApp.Models;
using Java.Lang;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// Class for handling score count and visited markers for Themes with Objectives
    /// </summary>
    internal class ObjectiveController
    {
        /// <summary>
        /// Key for storing and restoring user score from Bundle
        /// </summary>
        internal const string OBJECTIVE_SCORE = "OBJECTIVE_SCORE";

        /// <summary>
        /// Key for storing ObjectiveController data as a Bundle
        /// </summary>
        internal const string OBJECTIVE_BUNDLE = "OBJECTIVE_BUNDLE";

        /// <summary>
        /// List containing ObjectiveThemeMarkers with additional "Visited" field
        /// </summary>
        internal List<TrackedObjectiveThemeMarker> TrackedMarkers { get; private set; } = new List<TrackedObjectiveThemeMarker>();

        /// <summary>
        /// Points added up from answered markers
        /// </summary>
        internal int Score { get; private set; } = 0;

        /// <summary>
        /// Indicates whether all ObjectiveThemeMarkers have been visited or not
        /// </summary>
        internal bool AllMarkersVisited;

        /// <summary>
        /// Constructor for ObjectiveController
        /// </summary>
        /// <param name="markers">List of ObjectiveThemeMarkers to be tracked</param>
        internal ObjectiveController(List<ObjectiveThemeMarker> markers)
        {
            //Go through the list given as parameter
            foreach (ObjectiveThemeMarker marker in markers)
            {
                //Create a new tracked item and add it to local list
                TrackedMarkers.Add(new TrackedObjectiveThemeMarker(marker));
            }
        }

        /// <summary>
        /// Constructor for ObjectiveController
        /// </summary>
        /// <param name="markers">List of ObjectiveThemeMarkers to be tracked</param>
        /// <param name="bundle">Bundle for restoring progress after OnSaveInstanceState is called in Activity</param>
        internal ObjectiveController(List<ObjectiveThemeMarker> markers, Bundle bundle)
        {
            //Go through the list given as parameter
            foreach (ObjectiveThemeMarker marker in markers)
            {
                //Create a new tracked item with it's Visited state fetched from the Bundle and add it to local list
                TrackedMarkers.Add(new TrackedObjectiveThemeMarker(marker, bundle.GetBoolean(marker.Id)));
            }
            //Restore the user score
            Score = bundle.GetInt(OBJECTIVE_SCORE);
        }

        /// <summary>
        /// Constructor for ObjectiveController
        /// </summary>
        /// <param name="trackedMarkers">List of TrackedObjectiveThemeMarkers</param>
        internal ObjectiveController(List<TrackedObjectiveThemeMarker> trackedMarkers)
        {
            TrackedMarkers = trackedMarkers;
        }

        /// <summary>
        /// Check if a Marker is already visited. True if marker is visited, otherwise false
        /// </summary>
        /// <param name="markerID">ID of Marker</param>
        /// <returns>True if marker is visited, otherwise false</returns>
        internal bool IsVisited(string markerID)
        {
            //Go through the local list of Markers
            foreach(TrackedObjectiveThemeMarker tracked in TrackedMarkers)
                //If the ID matches and the marker is flagged as visited
                if (tracked.Marker == markerID && tracked.Visited)
                    //Return true
                    return true;

            //The Marker was either already visited or couldn't be found in the list
            return false;
        }

        /// <summary>
        /// Add points to the score of the user
        /// </summary>
        /// <param name="markerID">ID of Marker that was visited</param>
        /// <param name="points">Points for the answer picked by the user</param>
        internal void AddScore(string markerID, int points)
        {
            if (markerID == null)
                throw new ArgumentNullException("Given argument is null");

            //Check that the marker isn't already visited, and flag it as visited
            if (!FlagAsVisited(markerID))
                throw new IllegalStateException("Marker is already visited");

            //Check if all ObjectiveThemeMarkers have been visited
            if (GetTrackedMarkersList(true).Count == TrackedMarkers.Count)
                //Indicate that all ObjectiveThemeMarkers have been visited
                AllMarkersVisited = true;

            //Add up the points
            Score += points;
        }

        /// <summary>
        /// Flag a Marker as visited. True if Marker was found and unvisited, otherwise false.
        /// </summary>
        /// <param name="markerID">ID of Marker</param>
        /// <returns>True if Marker was found and unvisited, otherwise false.</returns>
        internal bool FlagAsVisited(string markerID)
        {
            //Go through the list of tracked ObjectiveThemeMarkers
            foreach (TrackedObjectiveThemeMarker tracked in TrackedMarkers)
                //If the ID of the Marker match and it's not yet visited
                if (tracked.Marker == markerID && !tracked.Visited)
                    //Flag the Marker as visited and return True
                    return tracked.Visited = true;

            //We could not find a Marker in the list or it has been already visited, return false
            return false;
        }

        /// <summary>
        /// Get a list of TrackedObjectiveMarkers from the Controller that have their Visited flag set to <paramref name="visited"/>
        /// </summary>
        /// <param name="visited">State of Visited flag</param>
        /// <returns>List of TrackedObjectiveMarkers that have their Visited flag set to <paramref name="visited"/></returns>
        internal List<TrackedObjectiveThemeMarker> GetTrackedMarkersList(bool visited)
        {
            //Create new List object
            List<TrackedObjectiveThemeMarker> result = new List<TrackedObjectiveThemeMarker>();

            //Go through the list of tracked markers
            foreach (TrackedObjectiveThemeMarker tracked in TrackedMarkers)
                //If the flag of the marker matches the given parameter
                if (tracked.Visited == visited)
                    //Add the marker to the list
                    result.Add(tracked);

            //Return the lsit of TrackedObjectiveMarkers
            return result;
        }

        /// <summary>
        /// Call to finish the Objective for the Theme
        /// </summary>
        /// <returns>Returns <see cref="ObjectiveResult"/></returns>
        internal ObjectiveResult FinishObjective()
        {
            //Return user score, number of ObjectiveThemeMarkers that were visited, and total number of markers
            return new ObjectiveResult(Score, GetTrackedMarkersList(true).Count, TrackedMarkers.Count);
        }

        /// <summary>
        /// Generate a Bundle containing user score and state of Visited flags of markers for restoring progress after OnSaveInstanceState is called in Activity
        /// </summary>
        /// <returns>Bundle for restoring progress after OnSaveInstanceState is called in Activity</returns>
        internal Bundle GetBundle()
        {
            //Initialize new Bundle
            Bundle bundle = new Bundle();
            //Store user score to Bundle
            bundle.PutInt(OBJECTIVE_SCORE, Score);
            //Go through each TrackedObjectiveThemeMarker
            foreach (TrackedObjectiveThemeMarker tracked in TrackedMarkers)
            {
                //Store Visited flag state to Bundle
                bundle.PutBoolean(tracked.Id, tracked.Visited);
            }

            //Return created Bundle
            return bundle;
        }
    }
}