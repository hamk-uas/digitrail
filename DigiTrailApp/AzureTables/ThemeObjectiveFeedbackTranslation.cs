using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class ThemeObjectiveFeedbackTranslation : BaseModel
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } = "Title";

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; } = "Text";

        [JsonProperty(PropertyName = "themeObjectiveFeedback")]
        public string ThemeObjectiveFeedback { get; set; } = "ThemeObjectiveFeedback";

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; } = Constants.DefaultLang;
    }
}