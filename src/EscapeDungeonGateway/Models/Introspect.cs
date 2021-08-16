using Newtonsoft.Json;

namespace EscapeDungeonGateway.Models
{
    public class Introspect
    {
        [JsonProperty("active")]
        public bool Active { get; set; }
        [JsonProperty("sub")]
        public string Sub { get; set; }
    }
}