using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public class CalculatedMobStats : ICalculatedMobStats
    {
        public int STR { get; private set; }
        public int DEX { get; private set; }
        public int INT { get; private set; }
        public int LUK { get; private set; }

        public int MaxHP { get; private set; }
        public int MaxMP { get; private set; }

        public int PAD { get; private set; }
        public int PDR { get; private set; }
        public int MAD { get; private set; }
        public int MDR { get; private set; }
        public int ACC { get; private set; }
        public int EVA { get; private set; }

        private readonly IFieldObjMob _mob;

        public CalculatedMobStats(IFieldObjMob mob)
        {
            _mob = mob;
        }

        public Task Calculate()
        {
            MaxHP = _mob.Info.MaxHP;
            MaxMP = _mob.Info.MaxMP;

            PAD = _mob.Info.PAD;
            PDR = _mob.Info.PDR;
            MAD = _mob.Info.MAD;
            MDR = _mob.Info.MDR;
            ACC = _mob.Info.ACC;
            EVA = _mob.Info.EVA;

            PAD += _mob.MobStats.GetValue(MobStatType.PAD);
            PDR += _mob.MobStats.GetValue(MobStatType.PDR);
            MAD += _mob.MobStats.GetValue(MobStatType.MAD);
            MDR += _mob.MobStats.GetValue(MobStatType.MDR);
            ACC += _mob.MobStats.GetValue(MobStatType.ACC);
            EVA += _mob.MobStats.GetValue(MobStatType.EVA);

            PAD = Math.Min(PAD, 29999);
            MAD = Math.Min(MAD, 29999);

            return Task.CompletedTask;
        }
    }
}
