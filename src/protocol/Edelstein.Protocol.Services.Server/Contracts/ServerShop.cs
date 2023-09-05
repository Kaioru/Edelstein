namespace Edelstein.Protocol.Services.Server.Contracts;

public record ServerShop(
    string ID,
    string Host,
    int Port,
    int WorldID
) : Server(ID, Host, Port), IServerShop;
