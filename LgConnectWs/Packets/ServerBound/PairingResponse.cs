using Newtonsoft.Json;

namespace LgConnectWs.Packets.ServerBound;

public class PairingResponse
{
    [JsonProperty("client-key")] public string ClientKey { get; set; }
}