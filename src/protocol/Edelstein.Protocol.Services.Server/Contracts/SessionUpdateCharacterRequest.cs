namespace Edelstein.Protocol.Services.Server.Contracts;

public record SessionUpdateCharacterRequest(
    int ID,
    int? CharacterID = null
);
