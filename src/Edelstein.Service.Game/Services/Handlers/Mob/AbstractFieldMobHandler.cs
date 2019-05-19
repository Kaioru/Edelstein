using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Mob;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers.User;

namespace Edelstein.Service.Game.Services.Handlers.Mob
{
    public abstract class AbstractFieldMobHandler : AbstractFieldUserHandler
    {
        public override Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var mob = user.Field.GetControlledObject<FieldMob>(user, packet.Decode<int>());
            return mob == null
                ? Task.CompletedTask
                : Handle(operation, packet, mob);
        }

        public abstract Task Handle(RecvPacketOperations operation, IPacket packet, FieldMob mob);
    }
}