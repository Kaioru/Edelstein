using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record SessionUpdateCharacterRequest(
    int ID,
    int? CharacterID = null
) : ISessionUpdateCharacterRequest;
