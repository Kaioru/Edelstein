using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class AliveAckHandler : IGameHandler
    {
        public Task Handle(RecvPacketOperations operation, IPacket packet, GameSocket socket)
            => socket.TryProcessHeartbeat(socket.Account, socket.Character);
    }
}