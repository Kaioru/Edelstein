using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Session.Contracts;

public record SessionUpdateResponse(SessionUpdateResult Result, ISession? Session = null) : ISessionUpdateResponse;
