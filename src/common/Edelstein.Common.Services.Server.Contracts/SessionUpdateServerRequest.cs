using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record SessionUpdateServerRequest(
    int ID,
    string ServerID
) : ISessionUpdateServerRequest;
