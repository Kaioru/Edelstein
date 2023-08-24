namespace Edelstein.Protocol.Services.Session.Contracts;

public record SessionUpdateServerRequest(
    int ID,
    string ServerID
);
