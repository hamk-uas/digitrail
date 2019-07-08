using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class Trail : BaseModel
    {
        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "file")]
        public string File { get; set; }

        [JsonProperty(PropertyName = "ratingCount")]
        public int RatingCount { get; set; }

        [JsonProperty(PropertyName = "ratingSum")]
        public int RatingSum { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "difficulty")]
        public string Difficulty { get; set; }
    }
}