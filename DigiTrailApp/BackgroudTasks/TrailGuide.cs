using System;
using System.Collections.Generic;
using System.Linq;
using Android.Locations;
using DigiTrailApp.Helpers;
using Android.Util;
using Com.Mapbox.Mapboxsdk.Geometry;

namespace DigiTrailApp.BackgroudTasks {
    /// <summary>
    /// This class will calculate and return trail information, distanse to end and start etc...
    /// Check <see cref="TrailResult"/> to see which parameter it contains.
    /// </summary>
    public class TrailGuide {

        // Variables for trail information provided when an object of this class is created
        Location userLocation;
        List<LatLng> trailPointList;
        int distanceToReact;
        int dificulty;

        // Variables to store distances the start and end points of the trail
        LatLng startPoint;
        LatLng endPoint;

        // Variables to store indexes of the 'trailPoints' list
        int middlePoint;
        int nearestPoint;
        int lastPoint;

        // Counters to be used when checking if usr is over halway of the trail or when user is going back 
        int forwardCounter;
        int backCounter;

        // Variables to store distances
        float[] distance;
        float tempDist;
        float trailLenght;

        // Get trail and user location information
        public TrailGuide(Location userLocation, List<LatLng> trailPointList, int distanceToReact, int dificulty) {
            this.userLocation = userLocation;
            this.trailPointList = trailPointList;
            this.distanceToReact = distanceToReact;
            this.dificulty = dificulty;
        }

        /// <summary>
        /// This is so called driver method. It calls other mehods of <see cref="TrailGuide"/> class which provide information about current trail.
        /// The resulted values of each method are stored to variables in <see cref="TrailResult"/> class.
        /// </summary>
        /// <returns>trailResult</returns>
        public void CheckTrailResult() {

            SetUp();
            CalculateNearestPoint();
            UserDirection();
            UserOnRange();
            SetLastPoint();
            CalculateTrailLenght();
            CalculateDistanceToEnd();
            CalculateDistanceToStart();
            SetTrailStatus();
            //DebugDump(); USE BREAKPOINTS FOR CRYING OUT LOUD
            //CalculateTime();
            //CalculateElapsedTime();
        }

        /// <summary>
        /// Sets up required variables like start and end points, distance array ect.
        /// </summary>
        private void SetUp() {

            // Distanse variable is used on Location.DistanceBetween class
            distance = new float[2];
            trailLenght = 0;

            // Startpoint of the trail
            startPoint = trailPointList.FirstOrDefault();

            // Calculate distance from user location to start point of the trail and store result to distance[0]. Round result and set values of 
            // DistanceToStartPoint and tempDist equal to distance[0]
            Location.DistanceBetween(userLocation.Latitude, userLocation.Longitude, startPoint.Latitude, startPoint.Longitude, distance);
            TrailResult.DistanceToStartPoint = Convert.ToInt32(Math.Round(distance.FirstOrDefault()));
            tempDist = distance.FirstOrDefault();

            // Endpoint of the trail
            endPoint = trailPointList.LastOrDefault();

            // Calculate distance from user location to end point of the trail and store result to distance[0]. Round result and set value of
            // DistanceToEndPoint equal to distance[0]
            Location.DistanceBetween(userLocation.Latitude, userLocation.Longitude, endPoint.Latitude, endPoint.Longitude, distance);
            TrailResult.DistanceToEndPoint = Convert.ToInt32(Math.Round(distance.FirstOrDefault()));

            // Middle point of the trail
            middlePoint = TrailResult.MiddlePointIndex = trailPointList.Count / 2 - 10; // TODO: REMOVE '-10'
            lastPoint = TrailResult.LastTrailPointIndex;

            forwardCounter = TrailResult.ForwardCounter;
            backCounter = TrailResult.BackCounter;
        }

        /// <summary>
        /// Calculates nearest point from user's location by distance. The index of nearest point is stored to <see cref="TrailResult.NearestTrailPointIndex"/>
        /// </summary>
        private void CalculateNearestPoint() {

            int buffer = 5;

            // If user has gone past the middle point, start checking distances from middle poin forward to the end of the list 
            // Else, start from first point of the list to the middle point of the list  
            if (TrailResult.IsOverHalfway) {
                for (int i = middlePoint - buffer; i < trailPointList.Count(); i++) {

                    // Calculate distance
                    Location.DistanceBetween(userLocation.Latitude, userLocation.Longitude, trailPointList[i].Latitude, trailPointList[i].Longitude, distance);

                    // Check if calculated distance is closer than previously calculated distance 
                    if (tempDist > distance.FirstOrDefault()) {

                        tempDist = distance.FirstOrDefault();
                        TrailResult.NearestTrailPointIndex = nearestPoint = i;
                        TrailResult.DistanceToNearestTrailPoint = Convert.ToInt32(tempDist);
                    }
                }
            } else {
                for (int i = 0; i <= middlePoint + buffer; i++) {

                    Location.DistanceBetween(userLocation.Latitude, userLocation.Longitude, trailPointList[i].Latitude, trailPointList[i].Longitude, distance);

                    // +5 is buffer used with checking if user is over halfway
                    if (tempDist > distance.FirstOrDefault()) {

                        tempDist = distance.FirstOrDefault();
                        TrailResult.NearestTrailPointIndex = nearestPoint = i;
                        TrailResult.DistanceToNearestTrailPoint = Convert.ToInt32(tempDist);
                    }
                }
            }
        }

        /// <summary>
        ///  Checks if user has gone past middle point or is going back 
        /// </summary>
        private void UserDirection() {

            // Check if user is going back. If so, add one to counter
            if (nearestPoint < lastPoint && backCounter < 5) {
                backCounter++;
                TrailResult.BackCounter = backCounter;

                if (forwardCounter > 0) {
                    forwardCounter--;
                    TrailResult.ForwardCounter = forwardCounter;
                }

            } else if (backCounter == 5) {
                TrailResult.GoingBack = true;

                if (nearestPoint < middlePoint && TrailResult.IsOverHalfway == true) {
                    TrailResult.IsOverHalfway = false;
                }

                // Check if user is walking past middle point of the trail. If so, add one to counter
            } else if (nearestPoint > lastPoint && forwardCounter < 5) {
                forwardCounter++;
                TrailResult.ForwardCounter = forwardCounter;

                if (backCounter > 0) {
                    backCounter--;
                    TrailResult.BackCounter = backCounter;
                }

            } else if (forwardCounter == 5) {
                TrailResult.GoingBack = false;

                if (nearestPoint > middlePoint && TrailResult.IsOverHalfway == false) {
                    TrailResult.IsOverHalfway = true;
                }
            }
        }

        /// <summary>
        /// Checks if user is on trail and sets <see cref="TrailResult.UserOnRange"/> true or false
        /// </summary>
        private void UserOnRange() {

            for (int i = 0; i < trailPointList.Count(); i++) {
                Location.DistanceBetween(userLocation.Latitude, userLocation.Longitude, trailPointList[i].Latitude, trailPointList[i].Longitude, distance);

                if (distance.FirstOrDefault() > distanceToReact) {
                    TrailResult.UserOnRange = false;
                } else {
                    TrailResult.UserOnRange = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Tracks what is the current status of the selected trail (started, finished or none). 
        /// </summary>
        private void SetTrailStatus() {

            // If the distance to the end is under 10 meters and the nearest trailpoint is the last point, the user has reached the end of the trail
            if (TrailResult.CalculatedDistanceToEnd < 10 && TrailResult.IsTrailStarted && TrailResult.NearestTrailPointIndex == TrailResult.LastTrailPointIndex) {
                TrailResult.IsTrailFinished = true;
                TrailResult.IsTrailStarted = false;

                // trail is started at first time the user is on range
            } else if (TrailResult.UserOnRange == true && !TrailResult.IsTrailFinished && !TrailResult.IsTrailStarted) {
                TrailResult.IsTrailStarted = true;

                // Set dialog booleans to false
                TrailResult.LostDialogSent = false;
                TrailResult.GoingBackDialogSent = false;
            }
        }

        /// <summary>
        /// Sets current nearest point as lastpoint
        /// </summary>
        private void SetLastPoint() {
            TrailResult.LastTrailPointIndex = lastPoint = nearestPoint;
        }

        /// <summary>
        /// Gets the trail lenght, add ups every trail points
        /// </summary>
        private void CalculateTrailLenght() {

            //Getting the trail lenght, add ups every trail points
            for (int i = 0; i < trailPointList.Count - 1; i++) {
                Location.DistanceBetween(trailPointList[i].Latitude, trailPointList[i].Longitude, trailPointList[i + 1].Latitude, trailPointList[i + 1].Longitude, distance);
                trailLenght = trailLenght + distance.FirstOrDefault();
            }
            TrailResult.TrailLenght = trailLenght;
        }

        /// <summary>
        /// Calculates trail lenght from user location to end
        /// </summary>
        private void CalculateDistanceToEnd() {

            trailLenght = 0;
            if (nearestPoint != 0) {
                for (int i = nearestPoint; i < trailPointList.Count - 1; i++) {
                    Location.DistanceBetween(trailPointList[i].Latitude, trailPointList[i].Longitude, trailPointList[i + 1].Latitude, trailPointList[i + 1].Longitude, distance);
                    trailLenght = trailLenght + distance.FirstOrDefault();
                }
            }
            TrailResult.CalculatedDistanceToEnd = Convert.ToInt32(trailLenght);
        }

        /// <summary>
        /// Calculates trail lenght from user location to start
        /// </summary>
        private void CalculateDistanceToStart() {

            trailLenght = 0;
            if (nearestPoint != 0) {
                for (int i = nearestPoint; i > 0; i--) {
                    Location.DistanceBetween(trailPointList[i].Latitude, trailPointList[i].Longitude, trailPointList[i - 1].Latitude, trailPointList[i - 1].Longitude, distance);
                    trailLenght = trailLenght + distance.FirstOrDefault();
                }
            }
            TrailResult.CalculatedDistanceToStart = Convert.ToInt32(trailLenght);
        }

        /// <summary>
        /// Calculates elapsed time to end or start (depending if user decides to go back same way he/she came) with 'userSpeed' variable,
        ///  which value comes from <see cref="userLocation"/>.speed and it is calculated based on what speed has user travelled earlier
        /// </summary>
        private void CalculateTime() {

            double userSpeed = 0;
            int timeInMinutes;

            if (userLocation.Speed != 0) {

                userSpeed = userLocation.Speed;
            } else {

                TrailResult.TimeInMinutes = 0;
            }

            if (TrailResult.GoingBack) {

                // Time to start in minutes
                timeInMinutes = Convert.ToInt32(Math.Round((TrailResult.CalculatedDistanceToStart / userSpeed) / 60));
                TrailResult.TimeInMinutes = timeInMinutes;
            } else {

                // Time to end in minutes
                timeInMinutes = Convert.ToInt32(Math.Round((TrailResult.CalculatedDistanceToEnd / userSpeed) / 60));
                TrailResult.TimeInMinutes = timeInMinutes;
            }
        }

        /// <summary>
        /// Calculates elapsed <see cref="DateTime"/> by adding <see cref="TrailResult.TimeInMinutes"/>
        /// Lastly stores value of sum to <see cref="TrailResult.ElapsedDateTime"/>
        /// </summary>
        private void CalculateElapsedTime() {

            DateTime now = DateTime.Now;
            DateTime elapsedTime = now.AddMinutes(TrailResult.TimeInMinutes);

            TrailResult.ElapsedDateTime = elapsedTime;
        }
    }
}