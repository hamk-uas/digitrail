using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class Language : BaseModel
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}