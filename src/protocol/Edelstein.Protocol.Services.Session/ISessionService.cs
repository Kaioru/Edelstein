using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Protocol.Services.Session;

public interface ISessionService
{
    Task<SessionResponse> Start(SessionStartRequest request);
    Task<SessionResponse> End(SessionEndRequest request);

    Task<SessionResponse> UpdateServer(SessionUpdateServerRequest request);
    Task<SessionResponse> UpdateCharacter(SessionUpdateCharacterRequest request);

    Task<SessionGetOneResponse> GetByActiveAccount(SessionGetByActiveAccountRequest request);
    Task<SessionGetOneResponse> GetByActiveCharacter(SessionGetByActiveCharacterRequest request);
}
