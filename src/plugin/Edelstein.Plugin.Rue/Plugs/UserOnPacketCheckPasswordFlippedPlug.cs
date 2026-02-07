using Edelstein.Plugin.Rue.Configs;
using Edelstein.Plugin.Rue.Contracts;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Plugin.Rue.Plugs;

public class UserOnPacketCheckPasswordFlippedPlug(ILogger? logger, IOptions<RueConfigLogin> options, LoginContext context) : IPipelinePlug<UserOnPacketCheckPassword>
{
    private readonly ILogger? _logger = logger;
    private readonly RueConfigLogin _config = options.Value;
    private readonly LoginContext _context = context;

    public async Task Handle(IPipelineContext ctx, UserOnPacketCheckPassword message)
    {
        if (_config.IsFlippedUsername && message is not UserOnPacketCheckPasswordFlipped)
        {
            await _context.Pipelines.UserOnPacketCheckPassword.Process(new UserOnPacketCheckPasswordFlipped(
                message.User,
                message.Password,
                message.Username
            ));
            ctx.Cancel();
        }
    }
}
