using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class TrailTheme : BaseModel
    {
        [JsonProperty(PropertyName = "default")]
        public bool Default { get; set; }

        [JsonProperty(PropertyName = "theme")]
        public string Theme { get; set; }

        [JsonProperty(PropertyName = "trail")]
        public string Trail { get; set; }
    }
}