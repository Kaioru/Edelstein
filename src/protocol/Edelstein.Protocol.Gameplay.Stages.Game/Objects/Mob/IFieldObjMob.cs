using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats.Modify;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob
{
    public interface IFieldObjMob : IFieldControlledObj, IFieldLife
    {
        IFieldObjMobInfo Info { get; }

        int HP { get; }
        int MP { get; }

        ICalculatedMobStats Stats { get; }

        IMobStats MobStats { get; }

        Task Hit(IFieldObjUser user, int damage);
        Task Kill(IFieldObjUser user);

        Task ModifyMobStats(Action<IModifyMobStatContext> action = null);
    }
}
