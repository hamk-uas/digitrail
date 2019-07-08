using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class ThemeMarker : BaseModel
    {
        [JsonProperty(PropertyName = "marker")]
        public string Marker { get; set; }

        [JsonProperty(PropertyName = "theme")]
        public string Theme { get; set; }
    }
}