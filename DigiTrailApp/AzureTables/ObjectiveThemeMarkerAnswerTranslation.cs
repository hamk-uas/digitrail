using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class ObjectiveThemeMarkerAnswerTranslation : BaseModel
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; } = "Text";

        [JsonProperty(PropertyName = "objectiveThemeMarkerAnswer")]
        public string ObjectiveThemeMarkerAnswer { get; set; } = "Answer";

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; } = Constants.DefaultLang;
    }
}