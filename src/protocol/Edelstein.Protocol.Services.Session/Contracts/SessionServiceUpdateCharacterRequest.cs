using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionServiceUpdateCharacterRequest
{
    [DataMember(Order = 1)] public required int AccountID { get; init; }
    [DataMember(Order = 2)] public required int? CharacterID { get; init; }
}
