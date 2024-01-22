using Newtonsoft.Json;

namespace LgConnectWs.SubscribePayloads;

public class CurrentChannelPayload
{
    [JsonProperty("channelNumber")]
    public string ChannelNumber { get; set; }
}