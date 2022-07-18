namespace Edelstein.Protocol.Services.Session.Contracts;

public interface ISessionUpdateRequest
{
    int AccountID { get; }
    SessionState? State { get; }
}
