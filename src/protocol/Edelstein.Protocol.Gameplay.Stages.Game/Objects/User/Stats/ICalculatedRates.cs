using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ICalculatedRates
    {
        public int Exp { get; }
        public int Reward { get; }
        public int Money { get; }

        Task Calculate();
    }
}
