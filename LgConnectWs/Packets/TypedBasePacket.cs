using Newtonsoft.Json;

namespace LgConnectWs.Packets;

public class TypedBasePacket<T>
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("payload")]
    public T Payload { get; set; }
}