using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class Location : BaseModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}