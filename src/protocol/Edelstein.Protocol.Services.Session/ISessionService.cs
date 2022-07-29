using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Protocol.Services.Session;

public interface ISessionService
{
    Task<ISessionStartResponse> Start(ISessionStartRequest request);
    Task<ISessionUpdateResponse> Update(ISessionUpdateRequest request);
    Task<ISessionEndResponse> End(ISessionEndRequest request);

    Task<ISessionGetOneResponse> GetByActiveAccount(ISessionGetByActiveAccountRequest request);
    Task<ISessionGetOneResponse> GetByActiveCharacter(ISessionGetByActiveCharacterRequest request);
}
