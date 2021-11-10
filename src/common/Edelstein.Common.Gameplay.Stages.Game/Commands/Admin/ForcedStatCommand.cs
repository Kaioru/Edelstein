using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Commands.Util;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Modify;
using PowerArgs;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Admin
{
    public class ForcedStatCommandArgs : CommandArgs
    {
        [ArgPosition(0), ArgRequired]
        [ArgDescription("The type of forced stat")]
        public ModifyForcedStatType Type { get; set; }

        [ArgPosition(1), ArgRequired]
        [ArgDescription("The value of forced stat")]
        public short Value { get; set; }
    }

    public class ForcedStatCommand : AbstractCommand<ForcedStatCommandArgs>
    {
        public override string Name => "ForcedStat";
        public override string Description => "Sets the forced stat to the desired value";

        public ForcedStatCommand()
        {
            Aliases.Add("ForceStat");
            Aliases.Add("FS");

            Register(new ActionCommand("Reset", "Resets the forced stat", (user, args) => user.ModifyForcedStats(s => s.Reset())));
        }

        public override async Task Execute(IFieldObjUser user, ForcedStatCommandArgs args)
        {
            await user.ModifyForcedStats(s => {
                switch (args.Type)
                {
                    case ModifyForcedStatType.STR:
                        s.STR = args.Value;
                        break;
                    case ModifyForcedStatType.DEX:
                        s.DEX = args.Value;
                        break;
                    case ModifyForcedStatType.INT:
                        s.INT = args.Value;
                        break;
                    case ModifyForcedStatType.LUK:
                        s.LUK = args.Value;
                        break;
                    case ModifyForcedStatType.PAD:
                        s.PAD = args.Value;
                        break;
                    case ModifyForcedStatType.PDD:
                        s.PDD = args.Value;
                        break;
                    case ModifyForcedStatType.MAD:
                        s.MAD = args.Value;
                        break;
                    case ModifyForcedStatType.MDD:
                        s.MDD = args.Value;
                        break;
                    case ModifyForcedStatType.ACC:
                        s.ACC = args.Value;
                        break;
                    case ModifyForcedStatType.EVA:
                        s.EVA = args.Value;
                        break;
                    case ModifyForcedStatType.Speed:
                        s.Speed = (byte)args.Value;
                        break;
                    case ModifyForcedStatType.Jump:
                        s.Jump = (byte)args.Value;
                        break;
                    case ModifyForcedStatType.SpeedMax:
                        s.SpeedMax = (byte)args.Value;
                        break;
                }
            });
        }
    }
}
