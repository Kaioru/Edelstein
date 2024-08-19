using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionServiceGetByActiveCharacterRequest
{
    [DataMember(Order = 1)] public required int CharacterID { get; init; }
}

