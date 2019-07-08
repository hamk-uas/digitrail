using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class ThemeMarkerPage : BaseModel
    {
        [JsonProperty(PropertyName = "orderNumber")]
        public byte OrderNumber { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "themeMarker")]
        public string ThemeMarker { get; set; }
    }
}