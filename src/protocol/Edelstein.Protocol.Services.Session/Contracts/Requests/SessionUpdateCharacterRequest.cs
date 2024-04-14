using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts.Requests;

[DataContract]
public record SessionUpdateCharacterRequest
{
    [DataMember(Order = 1)] public required int CharacterID { get; init; }
    [DataMember(Order = 2)] public required int AccountID { get; init; }
    [DataMember(Order = 3)] public required long Key { get; init; }
}
