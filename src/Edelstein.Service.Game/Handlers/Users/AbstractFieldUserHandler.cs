using System.Threading.Tasks;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public abstract class AbstractFieldUserHandler : AbstractPacketHandler<GameServiceAdapter>
    {
        protected override async Task Handle(GameServiceAdapter adapter, RecvPacketOperations operation, IPacketDecoder packet)
        {
            if (adapter.User != null)
                await Handle(adapter.User, operation, packet);
        }

        protected abstract Task Handle(FieldUser user, RecvPacketOperations operation, IPacketDecoder packet);
    }
}