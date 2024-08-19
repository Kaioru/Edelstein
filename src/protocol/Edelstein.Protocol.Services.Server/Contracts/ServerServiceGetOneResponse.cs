using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public class ServerServiceGetOneResponse<TServerInfo> 
    where TServerInfo : class, IServerInfo
{
    [DataMember(Order = 1)] public required ServerServiceResult Result { get; init; }
    [DataMember(Order = 2)] public TServerInfo? Info { get; init; }
}
