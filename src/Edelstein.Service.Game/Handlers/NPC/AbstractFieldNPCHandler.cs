using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.NPC;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Handlers.Users;

namespace Edelstein.Service.Game.Handlers.NPC
{
    public abstract class AbstractFieldNPCHandler : AbstractFieldUserHandler
    {
        protected override Task Handle(FieldUser user, RecvPacketOperations operation, IPacketDecoder packet)
        {
            var npc = user.Field.GetControlledObject<FieldNPC>(user, packet.DecodeInt());
            return npc == null
                ? Task.CompletedTask
                : Handle(npc, operation, packet);
        }

        protected abstract Task Handle(FieldNPC npc, RecvPacketOperations operation, IPacketDecoder packet);
    }
}