namespace Edelstein.Protocol.Services.Server.Contracts;

public record ServerGame(
    string ID,
    string Host,
    int Port,
    int WorldID,
    int ChannelID,
    bool IsAdultChannel = false
) : Server(ID, Host, Port), IServerGame;
