using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts.Requests;

[DataContract]
public record SessionGetByActiveCharacterRequest
{
    [DataMember(Order = 1)] public required int CharacterID { get; init; }
}
