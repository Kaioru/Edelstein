using Edelstein.Common.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserAbilityMassUpRequestPlug : IPipelinePlug<FieldOnPacketUserAbilityMassUpRequest>
{
    private readonly ILogger _logger;
    
    public FieldOnPacketUserAbilityMassUpRequestPlug(ILogger<FieldOnPacketUserAbilityMassUpRequestPlug> logger)
        => _logger = logger;

    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserAbilityMassUpRequest message)
    {
        var stats = new ModifyStatContext(message.User.Character);
        
        foreach (var pair in message.StatUp)
        {
            try
            {
                HandleStatUp(stats, pair.Key, pair.Value);
            }
            catch (ArgumentOutOfRangeException)
            {
                _logger.LogWarning("Unhandled ability up stat type {Type}", pair.Key);
            }
        }

        await message.User.ModifyStats(stats, exclRequest: true);
    }
    
    public static void HandleStatUp(ModifyStatContext stats, ModifyStatType type, int value = 1)
    {
        switch (type)
        {
            case ModifyStatType.STR:
                if (stats.STR + value > short.MaxValue)
                    return;
                stats.STR += (short)value;
                break;
            case ModifyStatType.DEX:
                if (stats.DEX + value > short.MaxValue)
                    return;
                stats.DEX += (short)value;
                break;
            case ModifyStatType.INT:
                if (stats.INT + value > short.MaxValue)
                    return;
                stats.INT += (short)value;
                break;
            case ModifyStatType.LUK:
                if (stats.LUK + value > short.MaxValue)
                    return;
                stats.LUK += (short)value;
                break;
            case ModifyStatType.MaxHP:
            case ModifyStatType.MaxMP:
            // TODO
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        stats.AP -= (short)value;
    }
}
