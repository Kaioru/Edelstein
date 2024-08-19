using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public class ServerServiceResponse
{
    [DataMember(Order = 1)] public required ServerServiceResult Result { get; init; }
}
