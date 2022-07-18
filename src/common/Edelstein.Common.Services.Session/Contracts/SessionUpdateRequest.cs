using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Session.Contracts;

public record SessionUpdateRequest(int AccountID, SessionState? State = null) : ISessionUpdateRequest;
