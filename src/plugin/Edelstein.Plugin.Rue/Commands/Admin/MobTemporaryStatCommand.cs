using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using PowerArgs;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class MobTemporaryStatCommandArgs : CommandArgs
{
    [ArgPosition(0), ArgRequired]
    [ArgDescription("The type of mob stat")]
    public MobTemporaryStatType Type { get; set; }

    [ArgPosition(1), ArgRequired]
    [ArgDescription("The value of mob stat")]
    public int Value { get; set; }

    [ArgPosition(2), ArgRequired]
    [ArgDescription("The reason of mob stat")]
    public int Reason { get; set; }
}

public class MobTemporaryStatCommand : AbstractCommand<MobTemporaryStatCommandArgs>
{
    public override string Name => "MobTemporaryStat";
    public override string Description => "Sets the mob temporary stat to the desired value";

    public MobTemporaryStatCommand()
    {
        Aliases.Add("MobBuff");
        Aliases.Add("MobStat");
        Aliases.Add("MTS");
    }

    protected override async Task Execute(IFieldUser user, MobTemporaryStatCommandArgs args)
    {
        if (user.FieldSplit != null)
            await Task.WhenAll(
                user.FieldSplit
                    .Objects
                    .OfType<IFieldMob>()
                    .Select(m => m.ModifyTemporaryStats(s => s.Set(args.Type, args.Value, args.Reason)))
            );
    }
}
