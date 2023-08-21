namespace Edelstein.Protocol.Services.Session.Contracts;

public record SessionUpdateCharacterRequest(
    int ID,
    int? CharacterID = null
);
