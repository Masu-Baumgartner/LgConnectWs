using Newtonsoft.Json;

namespace LgConnectWs.Payloads;

public class CreateToastPayload
{
    [JsonProperty("message")]
    public string Message { get; set; }
}