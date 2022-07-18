using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Session;

public record Session : ISession
{
    public int AccountID { get; set; }
    public SessionState State { get; set; }
}
