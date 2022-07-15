using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Packets;

public interface IPacketHandlerManager<in TStageUser> where TStageUser : IStageUser
{
    Task Process(TStageUser user, IPacketReader reader);
}
