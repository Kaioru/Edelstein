using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserGatherItemRequestHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.UserGatherItemRequest;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            _ = packet.ReadInt();
            var type = (ItemInventoryType)packet.ReadByte();
            var response = new UnstructuredOutgoingPacket(PacketSendOperations.GatherItemResult);

            response.WriteBool(false);
            response.WriteByte((byte)type);

            await user.ModifyInventory(i => i[type].Gather(), true);
            await user.Dispatch(response);
        }
    }
}
