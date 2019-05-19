using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.NPC;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers.User;

namespace Edelstein.Service.Game.Services.Handlers.NPC
{
    public abstract class AbstractFieldNPCHandler : AbstractFieldUserHandler
    {
        public override Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var npc = user.Field.GetControlledObject<FieldNPC>(user, packet.Decode<int>());
            return npc == null
                ? Task.CompletedTask
                : Handle(operation, packet, npc);
        }

        public abstract Task Handle(RecvPacketOperations operation, IPacket packet, FieldNPC npc);
    }
}