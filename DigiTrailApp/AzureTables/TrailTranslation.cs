using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class TrailTranslation : BaseModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "trail")]
        public string Trail { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; } = Constants.DefaultLang;
    }
}