using System;
using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class ThemeObjectiveFeedback : BaseModel
    {
        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; } = "Image";

        [JsonProperty(PropertyName = "scoreMin")]
        public byte ScoreMin { get; set; } = 0;

        [JsonProperty(PropertyName = "scoreMax")]
        public byte ScoreMax { get; set; } = 0;

        [JsonProperty(PropertyName = "themeObjective")]
        public string ThemeObjective { get; set; } = "ThemeObjective";
    }
}