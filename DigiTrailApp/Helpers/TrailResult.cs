using System;

namespace DigiTrailApp.Helpers {
    /// <summary>
    /// Trail results model
    /// </summary>
    public class TrailResult {

        /// <summary>Is user on the trail
        /// <para>True or False</para>        
        /// </summary>
        public static bool UserOnRange { get; set; }

        /// <summary>
        /// Is user going back
        /// <para>True or False</para>
        /// </summary>
        public static bool GoingBack { get; set; }

        /// <summary>Has user received dialog about going back
        /// <para>True or False</para>        
        /// </summary>
        public static bool GoingBackDialogSent { get; set; }

        /// <summary>Has user received dialog not to get lost
        /// <para>True or False</para>        
        /// </summary>
        public static bool LostDialogSent { get; set; }

        /// <summary>Has user walked pass the middle point
        /// <para>True or False</para>        
        /// </summary>
        public static bool IsOverHalfway { get; set; }

        /// <summary>Has user started the trail
        /// <para>True or False</para>        
        /// </summary>
        public static bool IsTrailStarted { get; set; }

        /// <summary>
        /// Counter used to check if user have gone past the middle point of the trail
        /// </summary>
        public static int ForwardCounter { get; set; }

        /// <summary>
        /// Counter used to check if user starts to go back the trail
        /// </summary>
        public static int BackCounter { get; set; }

        /// <summary>
        /// Has user finished the trail
        /// <para>True or False</para>
        /// </summary>
        public static bool IsTrailFinished { get; set; }

        /// <summary>What is nearest LatLng point from user
        /// <para>Integer</para>        
        /// </summary>
        public static int NearestTrailPointIndex { get; set; }

        /// <summary>What was the nearest point last time
        /// <para>Integer</para>        
        /// </summary>
        public static int LastTrailPointIndex { get; set; }

        /// <summary>Is user on the trail
        /// <para>Integer</para>        
        /// </summary>
        public static int MiddlePointIndex { get; set; }

        /// <summary>Dictance to start point (as the crow flies)
        /// <para>returns meters</para>        
        /// </summary>
        public static int DistanceToStartPoint { get; set; }

        /// <summary>Dictance to end point (as the crow flies)
        /// <para>returns meters</para>        
        /// </summary>
        public static int DistanceToEndPoint { get; set; }

        /// <summary>Dictance to middle point (as the crow flies)
        /// <para>returns meters</para>        
        /// </summary>
        public static int DistanceToMidPoint { get; set; }

        /// <summary>Calculated trail lenght
        /// <para>returns meters</para>        
        /// </summary>
        public static float TrailLenght { get; set; }

        /// <summary>Dictance to startpoint (traillenght - user location + trail lenght)
        /// <para>returns meters</para>        
        /// </summary>
        public static int CalculatedDistanceToStart { get; set; }

        /// <summary>Dictance to startpoint (traillenght + user location - trail lenght)
        /// <para>returns meters</para>        
        /// </summary>
        public static int CalculatedDistanceToEnd { get; set; }

        /// <summary>Dictance to startpoint (traillenght + user location - trail lenght)
        /// <para>returns meters</para>        
        /// </summary>
        public static int CalculatedDistanceToMid { get; set; }

        /// <summary>Elapsed time to finish the trip (based on dificultu level)
        /// <para>returns minutes</para>        
        /// </summary>
        public static int ElapsedTime { get; set; }

        /// <summary>Elapsed time to finish the trip (based on UserSpeed)
        /// <para>returns datetime</para>        
        /// </summary>
        public static DateTime ElapsedDateTime { get; set; }

        /// <summary> Calculate time to finish trip or back to start (if user has a speed)
        /// <para>returns minutes</para>        
        /// </summary>
        public static int TimeInMinutes { get; set; }

        /// <summary> user distance to nearest point on the trail
        /// <para>returns meters</para>        
        /// </summary>
        public static int DistanceToNearestTrailPoint { get; set; }

        /// <summary>
        /// Function to reset all variables in <see cref="TrailResult"/>
        /// </summary>
        public static void ResetResult()
        {
            BackCounter = 0;
            CalculatedDistanceToEnd = 0;
            CalculatedDistanceToMid = 0;
            CalculatedDistanceToStart = 0;
            DistanceToEndPoint = 0;
            DistanceToMidPoint = 0;
            DistanceToNearestTrailPoint = 0;
            DistanceToStartPoint = 0;
            ElapsedTime = 0;
            ElapsedDateTime = new DateTime();
            ForwardCounter = 0;
            GoingBack = false;
            GoingBackDialogSent = false;
            IsOverHalfway = false;
            IsTrailStarted = false;
            IsTrailFinished = false;
            LastTrailPointIndex = 0;
            LostDialogSent = false;
            MiddlePointIndex = 0;
            NearestTrailPointIndex = 0;
            TimeInMinutes = 0;
            TrailLenght = 0;
            UserOnRange = false;
        }
    }
}