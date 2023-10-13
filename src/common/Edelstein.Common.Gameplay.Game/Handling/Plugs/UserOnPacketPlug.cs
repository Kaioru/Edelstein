using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Game;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class UserOnPacketPlug : AbstractUserOnPacketPlug<IGameStageUser>
{
    public UserOnPacketPlug(IPacketHandlerManager<IGameStageUser> handler) : base(handler)
    {
    }
}
