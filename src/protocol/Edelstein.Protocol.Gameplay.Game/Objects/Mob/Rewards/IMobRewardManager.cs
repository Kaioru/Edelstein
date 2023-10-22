using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;

public interface IMobRewardManager
{
    Task Insert(int mobID, IMobReward reward);
    Task InsertAll(IMobReward reward);
    Task<ICollection<IMobReward>> RetrieveAll(int mobID);
    Task<ICollection<IMobReward>> RetrieveCalculated(IFieldUser user, IFieldMob mob);
}
