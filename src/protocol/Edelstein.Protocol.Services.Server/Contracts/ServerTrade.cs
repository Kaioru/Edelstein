namespace Edelstein.Protocol.Services.Server.Contracts;

public record ServerTrade(
    string ID,
    string Host,
    int Port,
    int WorldID
) : Server(ID, Host, Port), IServerTrade;
