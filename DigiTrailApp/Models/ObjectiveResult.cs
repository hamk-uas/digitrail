namespace DigiTrailApp.Models
{
    /// <summary>
    /// model for objectiveResult
    /// </summary>
    internal class ObjectiveResult
    {
        internal int Score { get; private set; } = 0;
        internal int VisitedMarkers { get; private set; } = 0;
        internal int TotalMarkers { get; private set; } = 0;

        internal ObjectiveResult(int score, int visitedMarkers, int totalMarkers)
        {
            Score = score;
            VisitedMarkers = visitedMarkers;
            TotalMarkers = totalMarkers;
        }
    }
}