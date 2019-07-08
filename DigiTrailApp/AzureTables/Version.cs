using Newtonsoft.Json;

namespace DigiTrailApp.AzureTables
{
    class Version : BaseModel
    {
        [JsonProperty(PropertyName = "versionCode")]
        public int VersionCode { get; set; }

        [JsonProperty(PropertyName = "versionName")]
        public string VersionName { get; set; }
    }
}