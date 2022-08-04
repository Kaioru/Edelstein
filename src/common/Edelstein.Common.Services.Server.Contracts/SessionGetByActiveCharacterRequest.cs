using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record SessionGetByActiveCharacterRequest(int CharacterID) : ISessionGetByActiveCharacterRequest;
