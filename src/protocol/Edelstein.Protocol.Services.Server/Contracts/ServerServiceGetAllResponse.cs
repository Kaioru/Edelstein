using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public class ServerServiceGetAllResponse<TServerInfo> 
    where TServerInfo : class, IServerInfo
{
    [DataMember(Order = 1)] public required ServerServiceResult Result { get; init; }
    [DataMember(Order = 2)] public ICollection<TServerInfo>? Info { get; init; }
}
