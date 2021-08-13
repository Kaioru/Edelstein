using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public class CalculatedRates : ICalculatedRates
    {
        public int Exp { get; private set; }
        public int Reward { get; private set; }
        public int Money { get; private set; }

        private readonly IFieldObjUser _user;

        public CalculatedRates(IFieldObjUser user)
            => _user = user;

        public Task Calculate()
        {
            Exp = 0;
            Reward = 0;
            Money = 0;

            return Task.CompletedTask;
        }
    }
}
