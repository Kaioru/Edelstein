using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class PartyRequestHandler : AbstractFieldUserHandler
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var type = (PartyRequestType) packet.Decode<byte>();

            switch (type)
            {
                case PartyRequestType.LoadParty:
                {
                    using var p = new Packet(SendPacketOperations.PartyResult);

                    p.Encode<byte>((byte) PartyResultType.LoadParty_Done);
                    p.Encode<int>(user.Party.ID);
                    user.Party.EncodeData(user.Service.State.ChannelID, p);

                    await user.SendPacket(p);
                    break;
                }
                case PartyRequestType.CreateNewParty:
                {
                    if (user.Party == null)
                        await user.Service.PartyManager.Create(user.Character);
                    break;
                }
                case PartyRequestType.WithdrawParty:
                {
                    if (user.Party != null)
                    {
                        if (user.Party.BossCharacterID == user.ID)
                            await user.Party.Disband();
                        else
                            await user.Party.Members
                                .First(m => m.CharacterID == user.ID)
                                .Withdraw();
                    }

                    break;
                }
                case PartyRequestType.KickParty:
                {
                    if (user.Party != null && user.Party.BossCharacterID == user.ID)
                    {
                        var target = packet.Decode<int>();

                        await user.Party.Members
                            .First(m => m.CharacterID == target)
                            .Kick();
                    }

                    break;
                }
                case PartyRequestType.ChangePartyBoss:
                {
                    if (user.Party != null && user.Party.BossCharacterID == user.ID)
                    {
                        var target = packet.Decode<int>();

                        await user.Party.Members
                            .Where(m => m.ChannelID >= 0)
                            .First(m => m.CharacterID == target)
                            .ChangeBoss();
                    }

                    break;
                }
                default:
                    Logger.Warn($"Unhandled party request type: {type}");
                    break;
            }
        }
    }
}