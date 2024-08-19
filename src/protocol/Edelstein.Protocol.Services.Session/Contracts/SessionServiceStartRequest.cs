using System.Runtime.Serialization;
using Edelstein.Protocol.Services.Session.Entities;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionServiceStartRequest
{
    [DataMember(Order = 1)] public required SessionServiceSessionInfo Info { get; init; }
}
