using Newtonsoft.Json;

namespace LgConnectWs.Payloads;

public class LauncherOpenPayload
{
    [JsonProperty("target")]
    public string Target { get; set; }
}