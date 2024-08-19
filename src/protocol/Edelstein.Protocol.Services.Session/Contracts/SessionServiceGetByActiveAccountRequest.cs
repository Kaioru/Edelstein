using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionServiceGetByActiveAccountRequest
{
    [DataMember(Order = 1)] public required int AccountID { get; init; }
}
