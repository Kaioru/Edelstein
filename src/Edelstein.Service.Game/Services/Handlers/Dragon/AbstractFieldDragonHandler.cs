using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Dragon;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers.User;

namespace Edelstein.Service.Game.Services.Handlers.Dragon
{
    public abstract class AbstractFieldDragonHandler : AbstractFieldUserHandler
    {
        public override Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var summoned = user.Owned
                .OfType<FieldDragon>()
                .FirstOrDefault();
            return summoned == null
                ? Task.CompletedTask
                : Handle(operation, packet, summoned);
        }

        public abstract Task Handle(RecvPacketOperations operation, IPacket packet, FieldDragon dragon);
    }
}