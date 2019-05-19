using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Services
{
    public interface IGameHandler
    {
        Task Handle(RecvPacketOperations operation, IPacket packet, GameSocket socket);
    }
}