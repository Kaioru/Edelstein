using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Summoned;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers.User;

namespace Edelstein.Service.Game.Services.Handlers.Summoned
{
    public abstract class AbstractFieldSummonedHandler : AbstractFieldUserHandler
    {
        public override Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var id = packet.Decode<int>();
            var summoned = user.Owned
                .OfType<FieldSummoned>()
                .FirstOrDefault(s => s.ID == id);
            return summoned == null
                ? Task.CompletedTask
                : Handle(operation, packet, summoned);
        }

        public abstract Task Handle(RecvPacketOperations operation, IPacket packet, FieldSummoned summoned);
    }
}