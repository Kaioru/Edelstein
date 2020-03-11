using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Stats.Modify;

namespace Edelstein.Service.Game.Commands.Impl
{
    public class StatCommand : AbstractCommand<StatCommandContext>
    {
        public override string Name => "Stat";
        public override string Description => "Sets the specified stat to a value";

        public StatCommand(Parser parser) : base(parser)
        {
            Aliases.Add("Set");
            Aliases.Add("Change");
        }

        protected override async Task Run(FieldUser sender, StatCommandContext ctx)
        {
            if (ctx.Value < 0)
            {
                await sender.Message("This command cannot use negative integers use only positive integers!");
                return;
            }

            switch (ctx.Type)
            {
                case ModifyStatType.Skin:
                case ModifyStatType.Face:
                case ModifyStatType.Hair:
                    await sender.Prompt(target =>
                        target.AskAvatar("Is this style okay?", new[] {ctx.Value})
                    );
                    break;
            }

            await sender.ModifyStats(s =>
            {
                switch (ctx.Type)
                {
                    case ModifyStatType.Skin:
                        s.Skin = (byte) ctx.Value;
                        break;
                    case ModifyStatType.Face:
                        s.Face = ctx.Value;
                        break;
                    case ModifyStatType.Hair:
                        s.Hair = ctx.Value;
                        break;
                    case ModifyStatType.Pet:
                    case ModifyStatType.Pet2:
                    case ModifyStatType.Pet3:
                        break;
                    case ModifyStatType.Level:
                        s.Level = (byte) ctx.Value;
                        break;
                    case ModifyStatType.Job:
                        s.Job = (short) ctx.Value;
                        break;
                    case ModifyStatType.STR:
                        s.STR = (short) ctx.Value;
                        break;
                    case ModifyStatType.DEX:
                        s.DEX = (short) ctx.Value;
                        break;
                    case ModifyStatType.INT:
                        s.INT = (short) ctx.Value;
                        break;
                    case ModifyStatType.LUK:
                        s.LUK = (short) ctx.Value;
                        break;
                    case ModifyStatType.HP:
                        s.HP = ctx.Value;
                        break;
                    case ModifyStatType.MaxHP:
                        s.MaxHP = ctx.Value;
                        break;
                    case ModifyStatType.MP:
                        s.MP = ctx.Value;
                        break;
                    case ModifyStatType.MaxMP:
                        s.MaxMP = ctx.Value;
                        break;
                    case ModifyStatType.AP:
                        s.AP = (short) ctx.Value;
                        break;
                    case ModifyStatType.SP:
                        if (ctx.ExtendSP.HasValue)
                            s.SetExtendSP(ctx.ExtendSP.Value, (byte) ctx.Value);
                        else
                            s.SP = (short) ctx.Value;
                        break;
                    case ModifyStatType.EXP:
                        s.EXP = ctx.Value;
                        break;
                    case ModifyStatType.POP:
                        s.POP = (short) ctx.Value;
                        break;
                    case ModifyStatType.Money:
                        s.Money = ctx.Value;
                        break;
                    case ModifyStatType.TempEXP:
                        s.TempEXP = ctx.Value;
                        break;
                }
            });
            await sender.Message($"Successfully set {ctx.Type} to {ctx.Value}");
        }
    }

    public class StatCommandContext : DefaultCommandContext
    {
        [Value(0, MetaName = "type", HelpText = "The stat type.", Required = true)]
        public ModifyStatType Type { get; set; }

        [Value(1, MetaName = "value", HelpText = "The stat value.", Required = true)]
        public int Value { get; set; }

        [Option('e', "extendSP", HelpText = "The ExtendSP job level.")]
        public byte? ExtendSP { get; set; }
    }
}