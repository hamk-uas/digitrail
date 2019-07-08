using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class MarkerType : BaseModel
    {
        [JsonProperty(PropertyName = "showToUser")]
        public bool ShowToUser { get; set; }

        [JsonProperty(PropertyName = "markerTypeIcon")]
        public string MarkerTypeIcon { get; set; }
    }
}