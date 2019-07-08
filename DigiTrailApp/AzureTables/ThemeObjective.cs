using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class ThemeObjective : BaseModel
    {
        [JsonProperty(PropertyName = "theme")]
        public string Theme { get; set; }
    }
}