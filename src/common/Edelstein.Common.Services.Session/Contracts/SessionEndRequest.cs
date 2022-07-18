using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Session.Contracts;

public record SessionEndRequest(int AccountID) : ISessionEndRequest;
