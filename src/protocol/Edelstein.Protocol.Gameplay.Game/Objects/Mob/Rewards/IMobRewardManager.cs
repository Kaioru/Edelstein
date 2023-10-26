using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;

public interface IMobRewardManager
{
    Task Insert(int mobID, IMobReward reward);
    Task InsertAll(IMobReward reward);
    Task<ICollection<IMobReward>> RetrieveAll(int mobID);
    Task<ICollection<IMobReward>> RetrieveAvailable(IFieldUser user, IFieldMob mob);
}
