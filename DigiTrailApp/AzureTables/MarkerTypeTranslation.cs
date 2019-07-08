using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class MarkerTypeTranslation : BaseModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "Name";

        [JsonProperty(PropertyName = "markerType")]
        public string MarkerType { get; set; } = "MarkerType";

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; } = Constants.DefaultLang;
    }
}