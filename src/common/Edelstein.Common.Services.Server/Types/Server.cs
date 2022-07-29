using Edelstein.Protocol.Services.Server.Types;

namespace Edelstein.Common.Services.Server.Types;

public record Server : IServer
{
    public string ID { get; set; }

    public string Host { get; set; }
    public int Port { get; set; }
}
