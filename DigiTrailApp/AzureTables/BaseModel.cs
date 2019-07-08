using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    /// <summary>
    /// Class containing the default columns for all Azure Mobile Apps tables
    /// </summary>
    public class BaseModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = "ID";

        [JsonProperty(PropertyName = "createdAt")]
        public string CreatedAt { get; set; } = "CreatedAt";

        [JsonProperty(PropertyName = "updatedAt")]
        public string UpdatedAt { get; set; } = "UpdatedAt";

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; } = "Version";

        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted { get; set; } = false;
    }
}