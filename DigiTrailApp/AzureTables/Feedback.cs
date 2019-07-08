using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class Feedback : BaseModel
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public float Lat { get; set; }

        [JsonProperty(PropertyName = "Lon")]
        public float Lon { get; set; }
    }
}