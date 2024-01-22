using Newtonsoft.Json;

namespace LgConnectWs.Payloads;

public class LauncherLaunchPayload
{
    [JsonProperty("id")]
    public string Id { get; set; }
}