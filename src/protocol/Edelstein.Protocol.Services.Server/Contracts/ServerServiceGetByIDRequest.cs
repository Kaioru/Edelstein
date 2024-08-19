using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public class ServerServiceGetByIDRequest
{
    [DataMember(Order = 1)] public required int ID { get; init; }
}
