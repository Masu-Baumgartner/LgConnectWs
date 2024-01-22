using Newtonsoft.Json;

namespace LgConnectWs.Packets;

public class BasePacket
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("payload")]
    public object Payload { get; set; }
}