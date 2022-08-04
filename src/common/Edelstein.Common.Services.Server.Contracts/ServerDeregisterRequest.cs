using Edelstein.Protocol.Services.Server.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record ServerDeregisterRequest(string ID) : IServerDeregisterRequest;
