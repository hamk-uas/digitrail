using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    /// <summary>
    /// MarkerTranslation Base model
    /// </summary>
    public class MarkerTranslation : BaseModel
    {

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } = "Title";

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = "Description";

        [JsonProperty(PropertyName = "popupDescription")]
        public string PopupDescription { get; set; } = "PopupDescription";

        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        [JsonProperty(PropertyName = "marker")]
        public string Marker { get; set; } = "Marker";

        [JsonProperty(PropertyName = "Language")]
        public string Language { get; set; } = Constants.DefaultLang;
    }
}