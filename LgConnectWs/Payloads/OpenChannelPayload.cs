using Newtonsoft.Json;

namespace LgConnectWs.Payloads;

public class OpenChannelPayload
{
    [JsonProperty("channelNumber")]
    public string ChannelNumber { get; set; } 
}