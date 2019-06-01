using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public abstract class AbstractFieldUserHandler : IGameHandler
    {
        public Task Handle(RecvPacketOperations operation, IPacket packet, GameSocket socket)
        {
            return socket.FieldUser == null
                ? Task.CompletedTask
                : Handle(operation, packet, socket.FieldUser);
        }

        public abstract Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user);
    }
}