using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionServiceResponse
{
    [DataMember(Order = 1)] public required SessionServiceResult Result { get; init; }
}
