using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Constants.Types;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using PowerArgs;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Admin
{
    public class MobStatCommandArgs : CommandArgs
    {
        [ArgPosition(0), ArgRequired]
        [ArgDescription("The type of mob stat")]
        public MobStatType Type { get; set; }

        [ArgPosition(1), ArgRequired]
        [ArgDescription("The value of mob stat")]
        public short Value { get; set; }

        [ArgPosition(2)]
        [ArgDescription("The reason of mob stat")]
        public int Reason { get; set; } = (int)Skill.CitizenCristalThrow;
    }

    public class MobStatCommand : AbstractCommand<MobStatCommandArgs>
    {
        public override string Name => "MobStat";
        public override string Description => "Sets the mob stat to the desired value";

        public MobStatCommand()
        {
            Aliases.Add("MobBuff");
            Aliases.Add("MobTemporaryStat");
            Aliases.Add("MTS");
        }

        public override async Task Execute(IFieldObjUser user, MobStatCommandArgs args)
        {
            await Task.WhenAll(
                user.FieldSplit
                    .GetObjects<IFieldObjMob>()
                    .Select(m => m.ModifyMobStats(s => s.Set(args.Type, args.Value, args.Reason)))
            );
        }
    }
}
