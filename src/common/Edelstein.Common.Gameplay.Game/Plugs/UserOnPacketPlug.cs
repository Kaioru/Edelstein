using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Game;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class UserOnPacketPlug : AbstractUserOnPacketPlug<IGameStageUser>
{
    public UserOnPacketPlug(IPacketHandlerManager<IGameStageUser> handler) : base(handler)
    {
    }
}
