using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface ISessionUpdateServerRequest : IIdentifiable<int>
{
    public string ServerID { get; }
}
