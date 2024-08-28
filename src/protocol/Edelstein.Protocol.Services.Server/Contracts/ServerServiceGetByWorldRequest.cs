using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public class ServerServiceGetByWorldRequest
{
    [DataMember(Order = 1)] public required int WorldID { get; init; }
}
