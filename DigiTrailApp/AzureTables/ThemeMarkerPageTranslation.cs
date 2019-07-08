using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class ThemeMarkerPageTranslation : BaseModel
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } = "Title";

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; } = "Text";
               
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        [JsonProperty(PropertyName = "themeMarkerPage")]
        public string ThemeMarkerPage { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; } = Constants.DefaultLang;
    }
}