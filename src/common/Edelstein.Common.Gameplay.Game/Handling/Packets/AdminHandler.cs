using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class AdminHandler : AbstractFieldHandler
{
    private ILogger _logger;
    
    public AdminHandler(ILogger<AdminHandler> logger) => _logger = logger;
    
    public override short Operation => (short)PacketRecvOperations.Admin;

    public override bool Check(IGameStageUser user)
        => user.Account?.GradeCode > 0;

    protected override async Task Handle(IFieldUser user, IPacketReader reader)
    {
        var type = reader.ReadByte();
        var gradeCode = user.Account?.GradeCode ?? 0;
        var subGradeCode = user.Account?.SubGradeCode ?? 0;
        var canUseCommonCommand =
            subGradeCode.HasFlag(AccountSubGradeCode.TesterAccount) ||
            subGradeCode.HasFlag(AccountSubGradeCode.ManagerAccount) ||
            gradeCode.HasFlag(AccountGradeCode.AdminLevel1);
        
        switch (type)
        {
            case 0x0: // /create <arg1>
                if (canUseCommonCommand)
                    await user.ModifyInventory(i => i.Add(reader.ReadInt()));
                break;
            case 0x1: // /d <arg1>
                if (canUseCommonCommand)
                    await user.ModifyInventory(i => i[(ItemInventoryType)reader.ReadByte()]?.Clear());
                break;
            case 0x2: // /exp <arg1>
                if (canUseCommonCommand)
                    await user.ModifyStats(s => s.EXP += (short)reader.ReadInt());
                break;
            case 0x12: // /h <arg1>
                if (canUseCommonCommand)
                {
                    var hidden = reader.ReadBool();
                    using var packet =  new PacketWriter(PacketSendOperations.AdminResult)
                        .WriteByte(0x12)
                        .WriteBool(hidden);
                    
                    await user.Hide(hidden);
                    await user.Dispatch(packet.Build());
                }
                break;
            case 0x1F: // /job <arg1>
                if (subGradeCode.HasFlag(AccountSubGradeCode.TesterAccount))
                    await user.ModifyStats(s => s.Job = (short)reader.ReadInt());
                break;
            case 0x21: // /apget <arg1>
                if (subGradeCode.HasFlag(AccountSubGradeCode.TesterAccount))
                    await user.ModifyStats(s => s.AP += (short)reader.ReadInt());
                break;
            case 0x22: // /spget <arg1>
                if (subGradeCode.HasFlag(AccountSubGradeCode.TesterAccount))
                    await user.ModifyStats(s => s.SP += (short)reader.ReadInt());
                break;
            case 0x23: // /str <arg1>
                if (subGradeCode.HasFlag(AccountSubGradeCode.TesterAccount))
                    await user.ModifyStats(s => s.STR = (short)reader.ReadInt());
                break;
            case 0x24: // /dex <arg1>
                if (subGradeCode.HasFlag(AccountSubGradeCode.TesterAccount))
                    await user.ModifyStats(s => s.DEX = (short)reader.ReadInt());
                break;
            case 0x25: // /int <arg1>
                if (subGradeCode.HasFlag(AccountSubGradeCode.TesterAccount))
                    await user.ModifyStats(s => s.INT = (short)reader.ReadInt());
                break;
            case 0x26: // /luk <arg1>
                if (subGradeCode.HasFlag(AccountSubGradeCode.TesterAccount))
                    await user.ModifyStats(s => s.LUK = (short)reader.ReadInt());
                break;
            default:
                _logger.LogWarning("Unhandled admin packet type 0x{Type:X}", type);
                return;
        }
    }
}
