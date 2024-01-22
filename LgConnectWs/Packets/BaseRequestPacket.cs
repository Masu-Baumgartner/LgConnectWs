using Newtonsoft.Json;

namespace LgConnectWs.Packets;

public class BaseRequestPacket : BasePacket
{
    [JsonProperty("uri")]
    public string Uri { get; set; }
}