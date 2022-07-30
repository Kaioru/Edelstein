using Edelstein.Protocol.Services.Server.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record ServerGetByIDRequest(string ID) : IServerGetByIDRequest;
