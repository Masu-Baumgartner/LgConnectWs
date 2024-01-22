## LgConnectWs
### A wrapper for the lg connect websocket api in c#

**Description:**

This little library helps you controlling your tv using the lg connect websocket api.
Some basic packets have already been implemented, if you want to perform custom operations,
you mostly just need to add custom payloads. You can take a look at the example project for more information

**Getting started:**

First, search and install the library via [nuget.org](https://www.nuget.org/packages/LgConnectWs)

The lg connect feature has to be enabled in the network settings in order to work. You will need the ip address of your tv.

Then start with creating a basic client and adding a connect event handler

````csharp
var lgConnect = new LgConnect();

lgConnect.Connected += async (_, _) =>
{
    Console.WriteLine("Connected");
    
    var paringRequest = JsonConvert.DeserializeObject<PairingRequest>(File.ReadAllText("pairing.json"))!;
    await lgConnect.Send("register", paringRequest);
};

lgConnect.Disconnected += (sender, eventArgs) =>
{
    Console.WriteLine("Disconnected");
};

await lgConnect.Connect("your ip address here");

await Task.Delay(-1);
````

You will need to provide a pairing request, i put a json file into the example project for you to use.
If you run this program now, you should get a prompt on your tv asking if you want to allow your program to connect.
You accept it and now your program and the tv are paired and requests could be sent. To execute code when pairing was successful,
add a new event handler like this:

````csharp
lgConnect.Paired += async (sender, s) =>
{
    Console.WriteLine($"Paired with client id: {s}");

    // Execute requests here
};
````

The string you get as a parameter is your client id. If you dont want to accept the prompt all the time,
save this client id and add it in every pairing request you want to make.

````csharp
var paringRequest = JsonConvert.DeserializeObject<PairingRequest>(File.ReadAllText("pairing.json"))!;

paringRequest.ClientKey = "81cc63d0de8da3117473925398d782ce";

await lgConnect.Send("register", paringRequest);
````

Now we want to switch the channel of your tv, as an example of sending requests.
Every request needs a uri which identifies the action you want to perform.
These urls are of course not documented ;) All i know of the requests is from [this repo](https://github.com/hobbyquaker/lgtv2)

A request to switch the current channel looks like this:

````csharp
Console.WriteLine($"Paired with client id: {s}");

await lgConnect.Request("ssap://tv/openChannel", new OpenChannelPayload()
{
   ChannelNumber = "1" // in the json its a string as well, dont ask me why :/
});
````

These requests should only be made if you are paired. Otherwise they will fail.

If you want to retrieve information an easy way is to subscribe to events. You can do this by providing a url and the payload model. The library will handle the rest for you.

A example to track which channel is currently playing:

````csharp
await lgConnect.Subscribe<CurrentChannelPayload>("ssap://tv/getCurrentChannel", arg =>
{
    Console.WriteLine($"Current channel: {arg.ChannelNumber}");
    return Task.CompletedTask;
});
````

The LgConnect object has some more event handlers to manually handle responses and see errors. Just take a look in the class to find out which events are available