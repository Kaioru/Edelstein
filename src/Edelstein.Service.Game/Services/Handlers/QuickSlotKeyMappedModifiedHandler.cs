using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class QuickSlotKeyMappedModifiedHandler : AbstractFieldUserHandler
    {
        public override Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            for (var i = 0; i < 8; i++)
            {
                user.Character.QuickSlotKeys[i] = packet.Decode<int>();
            }
            
            return Task.CompletedTask;
        }
    }
}