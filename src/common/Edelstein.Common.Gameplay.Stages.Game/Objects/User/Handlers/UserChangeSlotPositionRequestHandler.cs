using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserChangeSlotPositionRequestHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.UserChangeSlotPositionRequest;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            _ = packet.ReadInt();
            var type = (ItemInventoryType)packet.ReadByte();
            var from = packet.ReadShort();
            var to = packet.ReadShort();
            var number = packet.ReadShort();

            if (to == 0)
            {
                await user.ModifyInventory(exclRequest: true); // TODO: drops
                return;
            }

            // TODO: checks
            await user.ModifyInventory(i => i[type].Move(from, to), true);
        }
    }
}
