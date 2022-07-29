using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface ISessionUpdateRequest
{
    ISession Session { get; }
}
