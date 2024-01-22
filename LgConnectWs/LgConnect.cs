using LgConnectWs.Packets;
using LgConnectWs.Packets.ServerBound;
using Newtonsoft.Json;
using WebSocketSharp;

namespace LgConnectWs;

public class LgConnect : IDisposable
{
    private readonly string IdPrefix = "5d3ed79";
    private readonly List<LgConnectSubscribeListener> Subscribers = new();

    private WebSocket WebSocket;
    private int PacketCounter = 1;

    public EventHandler Connected { get; set; }
    public EventHandler Disconnected { get; set; }
    public EventHandler<string> Paired { get; set; }
    public EventHandler<string> RawPacket { get; set; }
    public EventHandler<string> Error { get; set; }

    public bool IsConnected => WebSocket.IsAlive;

    public Task Connect(string host, int port = 3000)
    {
        WebSocket = new WebSocket($"ws://{host}:{port}");

        WebSocket.OnMessage += HandleMessage;

        WebSocket.OnOpen += (sender, args) => Connected?.Invoke(sender, args);
        WebSocket.OnClose += (sender, args) => Disconnected?.Invoke(sender, args);
        WebSocket.OnError += (sender, args) => { Error.Invoke(this, $"Ws: {args.Message}: {args.Exception}"); };

        WebSocket.Connect();

        return Task.CompletedTask;
    }

    public Task Send(string type, object packet)
    {
        var basePacket = new BasePacket()
        {
            Id = GetId(),
            Type = type,
            Payload = packet
        };

        var json = JsonConvert.SerializeObject(basePacket);

        WebSocket.Send(json);

        return Task.CompletedTask;
    }

    public Task Send(BasePacket basePacket)
    {
        basePacket.Id = GetId();

        var json = JsonConvert.SerializeObject(basePacket);

        WebSocket.Send(json);

        return Task.CompletedTask;
    }

    public async Task Request(string uri, object? payload = null)
    {
        var requestPacket = new BaseRequestPacket()
        {
            Uri = uri,
            Payload = payload,
            Type = "request"
        };

        await Send(requestPacket);
    }

    public Task Subscribe<T>(string uri, Func<T, Task> handler)
    {
        var id = GetId();

        lock (Subscribers)
        {
            Subscribers.Add(new()
            {
                Id = id,
                Handler = handler,
                PayloadType = typeof(T)
            });
        }

        var subscribePacket = new BaseSubscribePacket()
        {
            Uri = uri,
            Id = id,
            Type = "subscribe"
        };

        var json = JsonConvert.SerializeObject(subscribePacket);
        WebSocket.Send(json);

        return Task.CompletedTask;
    }

    private void HandleMessage(object? sender, MessageEventArgs e)
    {
        if (e.IsBinary || e.IsPing)
            return;
        
        RawPacket?.Invoke(this, e.Data);

        var basePacket = JsonConvert.DeserializeObject<BasePacket>(e.Data)!;

        switch (basePacket.Type)
        {
            case "registered":
                var pairingResponse = JsonConvert.DeserializeObject<TypedBasePacket<PairingResponse>>(e.Data)!;
                Paired?.Invoke(this, pairingResponse.Payload.ClientKey);
                return;
        }

        // Unknown type? Lets check if its a subscribe packet

        LgConnectSubscribeListener? listener = null;

        lock (Subscribers)
            listener = Subscribers.FirstOrDefault(x => x.Id == basePacket.Id);

        if (listener != null)
        {
            try
            {
                var typedPacketType = typeof(TypedBasePacket<>).MakeGenericType(listener.PayloadType);
                var packet = JsonConvert.DeserializeObject(e.Data, typedPacketType);
                var data = packet.GetType().GetProperty("Payload").GetValue(packet);

                Task.Run(async () =>
                {
                    try
                    {
                        var taskO = listener.Handler
                            .GetType()
                            .GetMethod("Invoke")!
                            .Invoke(listener.Handler, new[] { data });

                        var task = taskO as Task;

                        await task!;
                    }
                    catch (Exception exception)
                    {
                        Error.Invoke(this, $"A subscribe handler threw an exception: {exception}");
                    }
                });
            }
            catch (Exception exception)
            {
                Error.Invoke(this, $"An unhandled error occured while processing subscription: {exception}");
            }
        }
    }

    private string GetId()
    {
        var id = IdPrefix + IntToStringWithLeadingZeros(PacketCounter, 5);

        PacketCounter++;

        return id;
    }

    private string IntToStringWithLeadingZeros(int number, int n)
    {
        string result = number.ToString();
        int length = result.Length;

        for (int i = length; i < n; i++)
        {
            result = "0" + result;
        }

        return result;
    }

    public void Dispose()
    {
    }
}