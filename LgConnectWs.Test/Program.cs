// See https://aka.ms/new-console-template for more information

using LgConnectWs;
using LgConnectWs.Packets.ClientBound;
using LgConnectWs.Payloads;
using LgConnectWs.SubscribePayloads;
using Newtonsoft.Json;

var lgConnect = new LgConnect();

lgConnect.Connected += async (_, _) =>
{
    Console.WriteLine("Connected");
    
    var paringRequest = JsonConvert.DeserializeObject<PairingRequest>(File.ReadAllText("pairing.json"))!;

    paringRequest.ClientKey = "81cc63d0de8da3117473925398d782ce";

    await lgConnect.Send("register", paringRequest);
};

lgConnect.Paired += async (sender, s) =>
{
    Console.WriteLine($"Paired with client id: {s}");

    await lgConnect.Request("ssap://tv/openChannel", new OpenChannelPayload()
    {
        ChannelNumber = "1"
    });

    await lgConnect.Subscribe<CurrentChannelPayload>("ssap://tv/getCurrentChannel", arg =>
    {
        Console.WriteLine($"Current channel: {arg.ChannelNumber}");
        return Task.CompletedTask;
    });
};

lgConnect.Disconnected += (sender, eventArgs) =>
{
    Console.WriteLine("Disconnected");
};

await lgConnect.Connect("172.27.64.182");

await Task.Delay(-1);