using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Protocol.Services.Session;

public interface ISession
{
    int AccountID { get; }

    SessionState State { get; }
}
