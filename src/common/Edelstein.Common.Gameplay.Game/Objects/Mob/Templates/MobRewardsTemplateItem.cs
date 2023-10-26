using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public class MobRewardsTemplateItem : ITemplate, IMobReward
{
    public int ID { get; }
    
    public int? ItemID { get; }
    
    public int? Money { get; }
    
    public int? NumberMin { get; }
    public int? NumberMax { get; }
    
    public int? ReqQuest { get; }
    
    public int? ReqLevelMin { get; }
    public int? ReqLevelMax { get; }
    
    public int? ReqMobLevelMin { get; }
    public int? ReqMobLevelMax { get; }
    
    public DateTime? DateStart { get; }
    public DateTime? DateEnd { get; }
    
    public MobRewardsTemplateItem(int id, IDataNode property)
    {
        ID = id;

        ItemID = property.ResolveInt("item");
        
        Money = property.ResolveInt("money");
        
        NumberMin = property.ResolveInt("min");
        NumberMax = property.ResolveInt("max");
    }
}
