using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using PowerArgs;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class TemporaryStatCommandArgs : CommandArgs
{
    [ArgPosition(0), ArgRequired]
    [ArgDescription("The type of temporary stat")]
    public TemporaryStatType Type { get; set; }

    [ArgPosition(1), ArgRequired]
    [ArgDescription("The value of temporary stat")]
    public short Value { get; set; }

    [ArgPosition(2)]
    [ArgDescription("The reason of temporary stat")]
    public int Reason { get; set; } = Skill.CitizenCristalThrow;
}

public class TemporaryStatCommand : AbstractCommand<TemporaryStatCommandArgs>
{
    public override string Name => "TemporaryStat";
    public override string Description => "Sets the temporary stat to the desired value";

    public TemporaryStatCommand()
    {
        Aliases.Add("Buff");
        Aliases.Add("SecondaryStat");
        Aliases.Add("TS");
    }

    protected override async Task Execute(IFieldUser user, TemporaryStatCommandArgs args) 
        => await user.ModifyTemporaryStats(s => s.Set(args.Type, args.Value, args.Reason));
}
