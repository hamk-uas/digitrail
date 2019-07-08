using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class ObjectiveThemeMarker : BaseModel
    {
        [JsonProperty(PropertyName = "marker")]
        public string Marker { get; set; }

        [JsonProperty(PropertyName = "themeObjective")]
        public string ThemeObjective { get; set; }
    }
}