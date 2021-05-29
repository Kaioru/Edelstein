using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IFieldDispatcher : IPacketDispatcher
    {
        Task Dispatch(IFieldObj source, IPacket packet);
    }
}
