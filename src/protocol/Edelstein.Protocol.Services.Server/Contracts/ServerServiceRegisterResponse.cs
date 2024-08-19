using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public class ServerServiceRegisterResponse
{
    [DataMember(Order = 1)] public required ServerServiceResult Result { get; init; }
    [DataMember(Order = 2)] public long? Secret { get; init; }
}
