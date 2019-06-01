using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Drop;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public abstract class AbstractFieldDropHandler : AbstractFieldUserHandler
    {
        public override Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<byte>();
            packet.Decode<int>();
            packet.Decode<short>();
            packet.Decode<short>();
            var objectID = packet.Decode<int>();
            packet.Decode<int>();

            return Handle(operation, packet, user, user.Field.GetObject<AbstractFieldDrop>(objectID));
        }

        public abstract Task Handle(
            RecvPacketOperations operation,
            IPacket packet,
            FieldUser user,
            AbstractFieldDrop drop
        );
    }
}