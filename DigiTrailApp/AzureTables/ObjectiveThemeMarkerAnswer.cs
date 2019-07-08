using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class ObjectiveThemeMarkerAnswer : BaseModel
    {
        [JsonProperty(PropertyName = "correct")]
        public bool Correct { get; set; }

        [JsonProperty(PropertyName = "points")]
        public byte Points { get; set; }

        [JsonProperty(PropertyName = "objectiveThemeMarker")]
        public string ObjectiveThemeMarker { get; set; }
    }
}