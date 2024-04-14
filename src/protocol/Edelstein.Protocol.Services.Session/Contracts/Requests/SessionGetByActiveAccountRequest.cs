using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts.Requests;

[DataContract]
public record SessionGetByActiveAccountRequest
{
    [DataMember(Order = 1)] public required int AccountID { get; init; }
}
