using Edelstein.Plugin.Rue.Configs;
using Edelstein.Plugin.Rue.Contracts;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.Plugs;

public class UserOnPacketCheckPasswordFlippedPlug : IPipelinePlug<UserOnPacketCheckPassword>
{
    private readonly ILogger? _logger;
    private readonly RueConfigLogin? _config;
    private readonly LoginContext _context;

    public UserOnPacketCheckPasswordFlippedPlug(ILogger? logger, RueConfigLogin? config, LoginContext context)
    {
        _logger = logger;
        _config = config;
        _context = context;
    }
    
    public async Task Handle(IPipelineContext ctx, UserOnPacketCheckPassword message)
    {
        if ((_config?.IsFlippedUsername ?? false) && message is not UserOnPacketCheckPasswordFlipped)
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
