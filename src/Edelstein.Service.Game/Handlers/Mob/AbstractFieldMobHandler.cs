using System.Threading.Tasks;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Game.Fields.Objects.Mob;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Handlers.Users;

namespace Edelstein.Service.Game.Handlers.Mob
{
    public abstract class AbstractFieldMobHandler : AbstractFieldUserHandler
    {
        protected override Task Handle(FieldUser user, RecvPacketOperations operation, IPacketDecoder packet)
        {
            var mob = user.Field.GetControlledObject<FieldMob>(user, packet.DecodeInt());
            return mob == null
                ? Task.CompletedTask
                : Handle(user, mob, operation, packet);
        }

        protected abstract Task Handle(FieldUser user, FieldMob mob, RecvPacketOperations operation, IPacketDecoder packet);
    }
}