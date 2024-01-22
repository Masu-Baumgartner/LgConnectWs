using Newtonsoft.Json;

namespace LgConnectWs.Packets;

public class BaseSubscribePacket : BasePacket
{
    [JsonProperty("uri")]
    public string Uri { get; set; }
}