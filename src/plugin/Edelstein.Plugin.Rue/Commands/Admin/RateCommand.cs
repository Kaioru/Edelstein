using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Rates;
using PowerArgs;

namespace Edelstein.Plugin.Rue.Commands.Admin;

file record CommandRateContext(IFieldUser? User, IGameStageOptions? Options) : IRateContext;

public class RateCommandArgs : CommandArgs
{
    [ArgPosition(0)]
    [ArgDescription("Output to chat window?")]
    public bool ToChat { get; set; } = false;
}

public class RateCommand : AbstractCommand<RateCommandArgs>
{
    private readonly IRateModifierManager _rates;

    public override string Name => "Rate";
    public override string Description => "Displays all active rate modifiers and final rates";

    public RateCommand(IRateModifierManager rates)
    {
        _rates = rates;
        Aliases.Add("Rates");
    }

    protected override async Task Execute(IFieldUser user, RateCommandArgs args)
    {
        var rateContext = new CommandRateContext(user, user.StageUser.Context.Options);
        var newline = args.ToChat ? " | " : "\\r\\n";

        var output = $"#e#bRate Information for #h ##n{newline}";

        foreach (var rateType in Enum.GetValues<RateType>())
        {
            var modifiers = await _rates.GetModifiersAsync(rateType, rateContext);
            var finalRate = await _rates.GetFinalRateAsync(rateType, rateContext);

            output += $"{newline}#e{rateType} Rate:#n{newline}";

            if (modifiers.Count == 0)
            {
                output += $"  No modifiers (base: 1.0x){newline}";
            }
            else
            {
                foreach (var modifier in modifiers.OrderByDescending(m => m.Priority ?? 0))
                {
                    output += $"  [{modifier.Source}] {modifier.Multiplier:F2}x (priority: {modifier.Priority ?? 0}){newline}";
                }
            }

            output += $"  #r=> Final: {finalRate:F2}x#k{newline}";
        }

        if (args.ToChat)
            await user.Message(output);
        else
            await user.Prompt(s => s.Say(output), default);
    }
}
