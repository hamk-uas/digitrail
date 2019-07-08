using DigiTrailApp.AzureTables;

namespace DigiTrailApp.Models
{
    /// <summary>
    /// Model for TrackedObjectiveThemeMarker
    /// </summary>
    internal class TrackedObjectiveThemeMarker : ObjectiveThemeMarker
    {
        internal bool Visited { get; set; } = false;

        internal TrackedObjectiveThemeMarker(ObjectiveThemeMarker objectiveThemeMarker)
        {
            Id = objectiveThemeMarker.Id;
            CreatedAt = objectiveThemeMarker.CreatedAt;
            UpdatedAt = objectiveThemeMarker.UpdatedAt;
            Deleted = objectiveThemeMarker.Deleted;
            Version = objectiveThemeMarker.Version;
            Marker = objectiveThemeMarker.Marker;
            ThemeObjective = objectiveThemeMarker.ThemeObjective;
        }
        internal TrackedObjectiveThemeMarker(ObjectiveThemeMarker objectiveThemeMarker, bool visited)
        {
            Id = objectiveThemeMarker.Id;
            CreatedAt = objectiveThemeMarker.CreatedAt;
            UpdatedAt = objectiveThemeMarker.UpdatedAt;
            Deleted = objectiveThemeMarker.Deleted;
            Version = objectiveThemeMarker.Version;
            Marker = objectiveThemeMarker.Marker;
            ThemeObjective = objectiveThemeMarker.ThemeObjective;
            Visited = visited;
        }
    }
}