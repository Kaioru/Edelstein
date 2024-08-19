using System.Runtime.Serialization;
using Edelstein.Protocol.Services.Session.Entities;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionServiceGetOneResponse
{
    [DataMember(Order = 1)] public required SessionServiceResult Result { get; init; }
    [DataMember(Order = 2)] public SessionServiceSessionInfo? Info { get; init; }
}
