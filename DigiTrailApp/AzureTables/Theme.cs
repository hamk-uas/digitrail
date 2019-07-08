using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class Theme : BaseModel
    {
        [JsonProperty(PropertyName = "ratingCount")]
        public int RatingCount { get; set; }

        [JsonProperty(PropertyName = "ratingSum")]
        public int RatingSum { get; set; }
    }
}