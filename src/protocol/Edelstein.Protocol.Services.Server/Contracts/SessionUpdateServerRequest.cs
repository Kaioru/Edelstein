namespace Edelstein.Protocol.Services.Server.Contracts;

public record SessionUpdateServerRequest(
    int ID,
    string ServerID
);
