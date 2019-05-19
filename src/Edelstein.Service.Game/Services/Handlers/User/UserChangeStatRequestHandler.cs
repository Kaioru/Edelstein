using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Stats.Modify;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserChangeStatRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<int>();
            var stat = (ModifyStatType) packet.Decode<int>();
            var hp = 0;
            var mp = 0;

            if (stat.HasFlag(ModifyStatType.HP))
                hp = packet.Decode<short>();

            if (stat.HasFlag(ModifyStatType.MP))
                mp = packet.Decode<short>();

            // TODO: portable chair
            // TODO: rope endurance?
            // TODO: checks

            if (hp > 0 || mp > 0)
                await user.ModifyStats(s =>
                {
                    s.HP += hp;
                    s.MP += mp;
                });
        }
    }
}