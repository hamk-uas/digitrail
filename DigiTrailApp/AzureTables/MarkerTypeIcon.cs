using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class MarkerTypeIcon : BaseModel
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }
    }
}