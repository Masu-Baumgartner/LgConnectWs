using Newtonsoft.Json;

namespace LgConnectWs.Packets.ClientBound;

public partial class PairingRequest
{
    [JsonProperty("forcePairing")] public bool ForcePairing { get; set; }

    [JsonProperty("pairingType")] public string PairingType { get; set; }

    [JsonProperty("manifest")] public Manifest Manifest { get; set; }

    [JsonProperty("client-key")] public string ClientKey { get; set; }
}

public partial class Manifest
{
    [JsonProperty("manifestVersion")] public long ManifestVersion { get; set; }

    [JsonProperty("appVersion")] public string AppVersion { get; set; }

    [JsonProperty("signed")] public Signed Signed { get; set; }

    [JsonProperty("permissions")] public string[] Permissions { get; set; }

    [JsonProperty("signatures")] public Signature[] Signatures { get; set; }
}

public partial class Signature
{
    [JsonProperty("signatureVersion")] public long SignatureVersion { get; set; }

    [JsonProperty("signature")] public string SignatureSignature { get; set; }
}

public partial class Signed
{
    [JsonProperty("created")]
    public long Created { get; set; }

    [JsonProperty("appId")] public string AppId { get; set; }

    [JsonProperty("vendorId")] public string VendorId { get; set; }

    [JsonProperty("localizedAppNames")] public LocalizedAppNames LocalizedAppNames { get; set; }

    [JsonProperty("localizedVendorNames")] public LocalizedVendorNames LocalizedVendorNames { get; set; }

    [JsonProperty("permissions")] public string[] Permissions { get; set; }

    [JsonProperty("serial")] public string Serial { get; set; }
}

public partial class LocalizedAppNames
{
    [JsonProperty("")]
    public string Empty { get; set; }

    [JsonProperty("ko-KR")]
    public string KoKr { get; set; }

    [JsonProperty("zxx-XX")]
    public string ZxxXx { get; set; }
}

public partial class LocalizedVendorNames
{
    [JsonProperty("")]
    public string Empty { get; set; }
}