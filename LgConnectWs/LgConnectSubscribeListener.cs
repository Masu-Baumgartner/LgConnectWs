namespace LgConnectWs;

public class LgConnectSubscribeListener
{
    public string Id { get; set; }
    public object Handler { get; set; }
    public Type PayloadType { get; set; }
}