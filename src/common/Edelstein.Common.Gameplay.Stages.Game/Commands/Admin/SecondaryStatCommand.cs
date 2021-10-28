using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Constants.Types;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using PowerArgs;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Admin
{
    public class SecondaryStatCommandArgs : CommandArgs
    {
        [ArgPosition(0), ArgRequired]
        [ArgDescription("The type of secondary stat")]
        public SecondaryStatType Type { get; set; }

        [ArgPosition(1), ArgRequired]
        [ArgDescription("The value of secondary stat")]
        public short Value { get; set; }

        [ArgPosition(1)]
        [ArgDescription("The reason of secondary stat")]
        public int Reason { get; set; } = (int)Skill.CitizenCristalThrow;
    }

    public class SecondaryStatCommand : AbstractCommand<SecondaryStatCommandArgs>
    {
        public override string Name => "SecondaryStat";
        public override string Description => "Sets the secondary stat to the desired value";

        public SecondaryStatCommand()
        {
            Aliases.Add("Buff");
            Aliases.Add("TemporaryStat");
            Aliases.Add("TS");
        }

        public override async Task Execute(IFieldObjUser user, SecondaryStatCommandArgs args)
        {
            await user.ModifySecondaryStats(s => s.Set(args.Type, args.Value, args.Reason));
        }
    }
}
