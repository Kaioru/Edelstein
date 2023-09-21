using Edelstein.Protocol.Services.Server.Contracts;

namespace Edelstein.Protocol.Services.Server;

public interface ISessionService
{
    Task<SessionResponse> Start(SessionStartRequest request);
    Task<SessionResponse> End(SessionEndRequest request);

    Task<SessionResponse> UpdateServer(SessionUpdateServerRequest request);
    Task<SessionResponse> UpdateCharacter(SessionUpdateCharacterRequest request);

    Task<SessionGetOneResponse> GetByActiveAccount(SessionGetByActiveAccountRequest request);
    Task<SessionGetOneResponse> GetByActiveCharacter(SessionGetByActiveCharacterRequest request);
}
