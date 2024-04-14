using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts.Requests;

[DataContract]
public record ServerGetByIDRequest
{
    [DataMember(Order = 1)] public required string ServerID { get; init; }
}
