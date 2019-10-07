using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserPortableChairSitRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var templateID = packet.Decode<int>();

            if (!user.Character.HasItem(templateID)) return;

            user.PortableChairID = templateID;
            await user.ModifyStats(exclRequest: true);

            using var p = new Packet(SendPacketOperations.UserSetActivePortableChair);
            p.Encode<int>(user.ID);
            p.Encode<int>(templateID);
            await user.Field.BroadcastPacket(user, p);
        }
    }
}