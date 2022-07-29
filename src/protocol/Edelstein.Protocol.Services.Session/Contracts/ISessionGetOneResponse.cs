using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface ISessionGetOneResponse
{
    SessionGetResult Result { get; }
    ISession? Session { get; }
}
