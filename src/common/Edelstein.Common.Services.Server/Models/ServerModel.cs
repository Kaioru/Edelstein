using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Services.Server.Models;

public record ServerModel : IIdentifiable<string>
{
    public string Host { get; set; }
    public int Port { get; set; }

    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }

    public string ID { get; set; }
}
