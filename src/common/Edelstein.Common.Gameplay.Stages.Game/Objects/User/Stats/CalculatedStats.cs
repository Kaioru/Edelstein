using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public class CalculatedStats : ICalculatedStats
    {
        public int STR { get; private set; }
        public int DEX { get; private set; }
        public int INT { get; private set; }
        public int LUK { get; private set; }

        public int MaxHP { get; private set; }
        public int MaxMP { get; private set; }

        public int PAD { get; private set; }
        public int PDD { get; private set; }
        public int MAD { get; private set; }
        public int MDD { get; private set; }
        public int ACC { get; private set; }
        public int EVA { get; private set; }
        public int Craft { get; private set; }
        public int Speed { get; private set; }
        public int Jump { get; private set; }

        public int STRr { get; private set; }
        public int DEXr { get; private set; }
        public int INTr { get; private set; }
        public int LUKr { get; private set; }

        public int MaxHPr { get; private set; }
        public int MaxMPr { get; private set; }

        public int PADr { get; private set; }
        public int PDDr { get; private set; }
        public int MADr { get; private set; }
        public int MDDr { get; private set; }
        public int ACCr { get; private set; }
        public int EVAr { get; private set; }

        private readonly IFieldObjUser _user;

        public CalculatedStats(IFieldObjUser user)
            => _user = user;

        public Task Calculate()
        {
            var character = _user.Character;

            STR = character.STR;
            DEX = character.DEX;
            INT = character.INT;
            LUK = character.LUK;

            MaxHP = character.MaxHP;
            MaxMP = character.MaxMP;

            PAD = 0;
            PDD = (int)(INT * 0.4 + 0.5 * LUK + DEX * 0.5 + STR * 1.2);
            MAD = 0;
            MDD = (int)(STR * 0.4 + 0.5 * DEX + LUK * 0.5 + INT * 1.2);
            ACC = 0;
            EVA = 0;
            Craft = INT + DEX + LUK;
            Speed = 100;
            Jump = 100;

            STRr = 0;
            DEXr = 0;
            INTr = 0;
            LUKr = 0;

            MaxHPr = 0;
            MaxMPr = 0;

            PADr = 0;
            PDDr = 0;
            MADr = 0;
            MDDr = 0;
            ACCr = 0;
            EVAr = 0;

            return Task.CompletedTask;
        }
    }
}
