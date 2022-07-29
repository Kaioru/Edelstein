using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Protocol.Services.Session;

public interface ISessionService
{
    Task<ISessionResponse> Start(ISessionStartRequest request);
    Task<ISessionResponse> Update(ISessionUpdateRequest request);
    Task<ISessionResponse> End(ISessionEndRequest request);

    Task<ISessionGetOneResponse> GetByActiveAccount(ISessionGetByActiveAccountRequest request);
    Task<ISessionGetOneResponse> GetByActiveCharacter(ISessionGetByActiveCharacterRequest request);
}
