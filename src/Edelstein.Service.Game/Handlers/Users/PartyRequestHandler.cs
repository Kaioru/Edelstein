using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;
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
            IPacketDecoder packet
        )
        {
            var type = (PartyRequestType) packet.DecodeByte();

            switch (type)
            {
                case PartyRequestType.LoadParty:
                {
                    using var p = new OutPacket(SendPacketOperations.PartyResult);

                    p.EncodeByte((byte) PartyResultType.LoadParty_Done);
                    p.EncodeInt(user.Party.ID);
                    user.Party.EncodeData(user.Service.State.ChannelID, p);

                    await user.SendPacket(p);
                    break;
                }
                case PartyRequestType.CreateNewParty:
                {
                    if (user.Party != null) return;

                    var result = PartyResultType.CreateNewParty_Done;

                    try
                    {
                        // TODO: beginner check

                        if (result == PartyResultType.CreateNewParty_Done)
                        {
                            await user.Service.PartyManager.Create(user.Character);
                            return;
                        }
                    }
                    catch
                    {
                        result = PartyResultType.CreateNewParty_Unknown;
                    }

                    using var p = new OutPacket(SendPacketOperations.PartyResult);
                    p.EncodeByte((byte) result);
                    await user.SendPacket(p);
                    break;
                }
                case PartyRequestType.WithdrawParty:
                {
                    if (user.Party == null) return;

                    try
                    {
                        if (user.Party.BossCharacterID == user.ID)
                            await user.Party.Disband();
                        else
                            await user.Party.Members
                                .First(m => m.CharacterID == user.ID)
                                .Withdraw();
                    }
                    catch
                    {
                        using var p = new OutPacket(SendPacketOperations.PartyResult);
                        p.EncodeByte((byte) PartyResultType.WithdrawParty_Unknown);
                        await user.SendPacket(p);
                    }

                    break;
                }
                case PartyRequestType.KickParty:
                {
                    if (user.Party == null || user.Party.BossCharacterID != user.ID) return;

                    var result = PartyResultType.KickParty_Done;
                    var target = packet.DecodeInt();

                    try
                    {
                        var member = user.Party.Members.First(m => m.CharacterID == target);

                        // TODO: fieldlimit

                        if (result == PartyResultType.JoinParty_Done)
                        {
                            await member.Kick();
                            return;
                        }
                    }
                    catch
                    {
                        result = PartyResultType.KickParty_Unknown;
                    }

                    using var p = new OutPacket(SendPacketOperations.PartyResult);
                    p.EncodeByte((byte) result);
                    await user.SendPacket(p);
                    break;
                }
                case PartyRequestType.ChangePartyBoss:
                {
                    if (user.Party == null || user.Party.BossCharacterID != user.ID) return;

                    var result = PartyResultType.ChangePartyBoss_Done;
                    var target = packet.DecodeInt();

                    try
                    {
                        var member = user.Party.Members
                            .Where(m => m.ChannelID >= 0)
                            .First(m => m.CharacterID == target);

                        if (member.ChannelID != user.Service.State.ChannelID)
                            result = PartyResultType.ChangePartyBoss_NotSameChannel;
                        if (member.FieldID != user.Field.Template.ID)
                            result = PartyResultType.ChangePartyBoss_NotSameField;

                        if (result == PartyResultType.ChangePartyBoss_Done)
                        {
                            await member.ChangeBoss();
                            return;
                        }
                    }
                    catch
                    {
                        result = PartyResultType.ChangePartyBoss_Unknown;
                    }

                    using var p = new OutPacket(SendPacketOperations.PartyResult);
                    p.EncodeByte((byte) result);
                    await user.SendPacket(p);
                    break;
                }
                default:
                    Logger.Warn($"Unhandled party request type: {type}");
                    break;
            }
        }
    }
}