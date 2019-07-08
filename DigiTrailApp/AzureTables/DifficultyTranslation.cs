using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class DifficultyTranslation : BaseModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "Name";

        [JsonProperty(PropertyName = "difficulty")]
        public string Difficulty { get; set; } = "Difficulty";

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; } = Constants.DefaultLang;
    }
}