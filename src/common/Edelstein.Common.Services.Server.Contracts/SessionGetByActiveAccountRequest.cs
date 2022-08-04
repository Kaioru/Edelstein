using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record SessionGetByActiveAccountRequest(int AccountID) : ISessionGetByActiveAccountRequest;
