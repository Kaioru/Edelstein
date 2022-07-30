using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Common.Services.Server.Contracts;

public record SessionGetOneResponse(
    SessionResult Result,
    ISession? Session = null
) : ISessionGetOneResponse;
