using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.User;
using Edelstein.Service.Game.Fields.User.Stats.Modify;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class StatCommand : AbstractCommand<StatCommandOption>
    {
        public override string Name => "Stat";
        public override string Description => "Sets the specified stat to a value";

        public StatCommand(Parser parser) : base(parser)
        {
            Aliases.Add("Set");
            Aliases.Add("Change");
        }

        protected override async Task Execute(FieldUser sender, StatCommandOption option)
        {
            switch (option.Type)
            {
                case ModifyStatType.Skin:
                case ModifyStatType.Face:
                case ModifyStatType.Hair:
                    await sender.Prompt((self, target) =>
                        target.AskAvatar("Is this style okay?", new[] {option.Value})
                    );
                    break;
            }

            await sender.ModifyStats(s =>
            {
                switch (option.Type)
                {
                    case ModifyStatType.Skin:
                        s.Skin = (byte) option.Value;
                        break;
                    case ModifyStatType.Face:
                        s.Face = option.Value;
                        break;
                    case ModifyStatType.Hair:
                        s.Hair = option.Value;
                        break;
                    case ModifyStatType.Pet:
                    case ModifyStatType.Pet2:
                    case ModifyStatType.Pet3:
                        break;
                    case ModifyStatType.Level:
                        s.Level = (byte) option.Value;
                        break;
                    case ModifyStatType.Job:
                        s.Job = (short) option.Value;
                        break;
                    case ModifyStatType.STR:
                        s.STR = (short) option.Value;
                        break;
                    case ModifyStatType.DEX:
                        s.DEX = (short) option.Value;
                        break;
                    case ModifyStatType.INT:
                        s.INT = (short) option.Value;
                        break;
                    case ModifyStatType.LUK:
                        s.LUK = (short) option.Value;
                        break;
                    case ModifyStatType.HP:
                        s.HP = option.Value;
                        break;
                    case ModifyStatType.MaxHP:
                        s.MaxHP = option.Value;
                        break;
                    case ModifyStatType.MP:
                        s.MP = option.Value;
                        break;
                    case ModifyStatType.MaxMP:
                        s.MaxMP = option.Value;
                        break;
                    case ModifyStatType.AP:
                        s.AP = (short) option.Value;
                        break;
                    case ModifyStatType.SP:
                        if (option.ExtendSP.HasValue)
                            s.SetExtendSP(option.ExtendSP.Value, (byte) option.Value);
                        else
                            s.SP = (short) option.Value;
                        break;
                    case ModifyStatType.EXP:
                        s.EXP = option.Value;
                        break;
                    case ModifyStatType.POP:
                        s.POP = (short) option.Value;
                        break;
                    case ModifyStatType.Money:
                        s.Money = option.Value;
                        break;
                    case ModifyStatType.TempEXP:
                        s.TempEXP = option.Value;
                        break;
                }
            });
            await sender.Message($"Successfully set {option.Type} to {option.Value}");
        }
    }

    public class StatCommandOption
    {
        [Value(0, MetaName = "type", HelpText = "The stat type.", Required = true)]
        public ModifyStatType Type { get; set; }

        [Value(1, MetaName = "value", HelpText = "The stat value.", Required = true)]
        public int Value { get; set; }

        [Option('e', "extendSP", HelpText = "The ExtendSP job level.")]
        public byte? ExtendSP { get; set; }
    }
}