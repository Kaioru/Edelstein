namespace Edelstein.Protocol.Services.Server.Contracts;

public record ServerLogin(
    string ID, 
    string Host, 
    int Port
) : Server(ID, Host, Port), IServerLogin;
