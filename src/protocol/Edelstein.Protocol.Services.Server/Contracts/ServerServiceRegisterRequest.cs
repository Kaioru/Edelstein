using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public record ServerServiceRegisterRequest<TServerInfo> 
    where TServerInfo : class, IServerInfo
{
    [DataMember(Order = 1)] public required TServerInfo Info { get; init; }
}
