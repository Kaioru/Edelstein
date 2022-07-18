using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Session.Contracts;

public record SessionStartResponse(SessionStartResult Result, ISession? Session = null) : ISessionStartResponse;
