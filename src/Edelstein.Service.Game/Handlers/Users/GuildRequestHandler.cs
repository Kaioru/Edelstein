using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Guild;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class GuildRequestHandler : AbstractFieldUserHandler
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            var type = (GuildRequestType) packet.DecodeByte();

            switch (type)
            {
                case GuildRequestType.LoadGuild:
                {
                    using var p = new OutPacket(SendPacketOperations.GuildResult);
                    p.EncodeByte((byte) GuildResultType.LoadGuild_Done);

                    if (user.Guild != null)
                    {
                        p.EncodeBool(true);
                        user.Guild.EncodeData(p);
                    }
                    else p.EncodeBool(false);

                    await user.SendPacket(p);
                    break;
                }
                case GuildRequestType.WithdrawGuild:
                {
                    if (user.Guild == null) return;

                    try
                    {
                        var member = user.Guild.Members
                            .First(m => m.CharacterID == user.ID);

                        if (member.Grade != 1)
                            await member.Withdraw();
                    }
                    catch
                    {
                        using var p = new OutPacket(SendPacketOperations.GuildResult);
                        p.EncodeByte((byte) GuildResultType.WithdrawGuild_Unknown);
                        await user.SendPacket(p);
                    }

                    break;
                }
                case GuildRequestType.KickGuild:
                {
                    if (user.Guild == null) return;

                    var result = GuildResultType.KickGuild_Done;
                    var target = packet.DecodeInt();

                    try
                    {
                        var member = user.Guild.Members.First(m => m.CharacterID == user.ID);
                        var targetMember = user.Guild.Members.First(m => m.CharacterID == target);

                        if (member.Grade != 1 && member.Grade != 2) return;
                        if (member.Grade >= targetMember.Grade) return;

                        if (result == GuildResultType.KickGuild_Done)
                        {
                            await targetMember.Kick();
                            return;
                        }
                    }
                    catch
                    {
                        result = GuildResultType.KickGuild_Unknown;
                    }

                    using var p = new OutPacket(SendPacketOperations.GuildResult);
                    p.EncodeByte((byte) result);
                    await user.SendPacket(p);
                    break;
                }
                case GuildRequestType.SetGradeName:
                {
                    if (user.Guild == null) return;

                    var result = GuildResultType.SetGradeName_Done;
                    var name = new[]
                    {
                        packet.DecodeString(),
                        packet.DecodeString(),
                        packet.DecodeString(),
                        packet.DecodeString(),
                        packet.DecodeString()
                    };

                    try
                    {
                        var member = user.Guild.Members.First(m => m.CharacterID == user.ID);

                        if (member.Grade != 1) return;

                        if (result == GuildResultType.SetGradeName_Done)
                        {
                            await user.Guild.UpdateSetGradeName(name);
                            return;
                        }
                    }
                    catch
                    {
                        result = GuildResultType.CheckGuildName_Unknown;
                    }

                    using var p = new OutPacket(SendPacketOperations.GuildResult);
                    p.EncodeByte((byte) result);
                    await user.SendPacket(p);
                    break;
                }
                case GuildRequestType.SetMemberGrade:
                {
                    if (user.Guild == null) return;

                    var result = GuildResultType.SetMemberGrade_Done;
                    var target = packet.DecodeInt();
                    var grade = packet.DecodeByte();

                    try
                    {
                        var member = user.Guild.Members.First(m => m.CharacterID == user.ID);
                        var targetMember = user.Guild.Members.First(m => m.CharacterID == target);

                        if (member.Grade != 1 && member.Grade != 2) return;
                        if (member.Grade >= targetMember.Grade) return;
                        if (member.Grade >= grade) return;

                        if (result == GuildResultType.SetMemberGrade_Done)
                        {
                            await targetMember.UpdateSetMemberGrade(grade);
                            return;
                        }
                    }
                    catch
                    {
                        result = GuildResultType.SetMemberGrade_Unknown;
                    }

                    using var p = new OutPacket(SendPacketOperations.GuildResult);
                    p.EncodeByte((byte) result);
                    await user.SendPacket(p);
                    break;
                }
                case GuildRequestType.SetNotice:
                {
                    if (user.Guild == null) return;

                    var notice = packet.DecodeString();

                    try
                    {
                        var member = user.Guild.Members.First(m => m.CharacterID == user.ID);

                        if (member.Grade != 1 && member.Grade != 2) return;
                        if (notice.Length > 100) return;

                        await user.Guild.UpdateSetNotice(notice);
                    }
                    catch
                    {
                        await user.Message("Failed to set guild notice.");
                    }

                    break;
                }
                default:
                    Logger.Warn($"Unhandled guild request type: {type}");
                    break;
            }
        }
    }
}