using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class ThemeTranslation : BaseModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "Name";

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = "Description";

        [JsonProperty(PropertyName = "theme")]
        public string Theme { get; set; } = "Theme";

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; } = Constants.DefaultLang;
    }
}