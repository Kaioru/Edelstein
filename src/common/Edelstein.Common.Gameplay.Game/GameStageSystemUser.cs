using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Game;

public class GameStageSystemUser(
    ISocket socket, 
    IGameStageSystem system
) : IGameStageSystemUser
{
    public ISocket Socket { get; } = socket;
    public IGameStageSystem System { get; } = system;
    
    public Account? Account { get; set; }
    public AccountWorldData? AccountWorldData { get; set; }
    public Character? Character { get; set; }

    public bool IsMigrating { get; set; }
    public long Key { get; set; }
}
