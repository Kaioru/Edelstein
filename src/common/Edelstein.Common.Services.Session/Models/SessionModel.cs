using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Services.Session.Models;

public record SessionModel : IIdentifiable<int>, ISession
{
    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }

    public int ID { get; set; }
    public int AccountID => ID;

    public SessionState State { get; set; }
}
