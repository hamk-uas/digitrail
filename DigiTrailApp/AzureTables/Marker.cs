using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    /// <summary>
    /// Marker base model
    /// </summary>
    public class Marker : BaseModel
    {
        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "popupImage")]
        public string PopupImage { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public float Lat { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public float Lon { get; set; }

        [JsonProperty(PropertyName = "impactrange")]
        public int Impactrange { get; set; }

        [JsonProperty(PropertyName = "markerType")]
        public string MarkerType { get; set; }
    }
}