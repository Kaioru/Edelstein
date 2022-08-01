using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Actions;
using Edelstein.Protocol.Gameplay.Stages.Game;

namespace Edelstein.Common.Gameplay.Stages.Game.Plugs;

public class SocketOnPacketPlug : AbstractSocketOnPacketPlug<IGameStageUser>
{
    public SocketOnPacketPlug(IPacketHandlerManager<IGameStageUser> handler) : base(handler)
    {
    }
}
