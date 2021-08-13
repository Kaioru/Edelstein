using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ICalculatedRates
    {
        int Exp { get; }
        int Reward { get; }
        int Money { get; }

        Task Calculate();
    }
}
