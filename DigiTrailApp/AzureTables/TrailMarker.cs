using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class TrailMarker : BaseModel
    {
        [JsonProperty(PropertyName = "marker")]
        public string Marker { get; set; }

        [JsonProperty(PropertyName = "trail")]
        public string Trail { get; set; }
    }
}