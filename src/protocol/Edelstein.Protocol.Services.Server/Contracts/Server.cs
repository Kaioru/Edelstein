namespace Edelstein.Protocol.Services.Server.Contracts;

public record Server(
    string ID,
    string Host,
    int Port
) : IServer;
