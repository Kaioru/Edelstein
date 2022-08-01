using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class GameStageUserInitializer : IAdapterInitializer
{
    private readonly IGameContext _context;

    public GameStageUserInitializer(IGameContext context) => _context = context;

    public IAdapter Initialize(ISocket socket) => new GameStageUser(socket, _context);
}
