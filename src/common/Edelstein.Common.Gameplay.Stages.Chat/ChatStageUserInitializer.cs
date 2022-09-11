using Edelstein.Protocol.Gameplay.Stages.Chat.Contexts;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Chat;

public class ChatStageUserInitializer : IAdapterInitializer
{
    private readonly IChatContext _context;

    public ChatStageUserInitializer(IChatContext context) => _context = context;

    public IAdapter Initialize(ISocket socket) => new ChatStageUser(socket, _context);
}
