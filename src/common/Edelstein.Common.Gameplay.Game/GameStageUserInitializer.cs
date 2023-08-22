using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Game;

public class GameStageUserInitializer: IAdapterInitializer
{
    private readonly GameContext _context;

    public GameStageUserInitializer(GameContext context) => _context = context;

    public IAdapter Initialize(ISocket socket) => new GameStageUser(socket, _context);
}
