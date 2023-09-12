using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Modify;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;

namespace Edelstein.Protocol.Gameplay.Game.Objects.User;

public interface IFieldUserModify
{
    bool IsRequireUpdate { get; }
    bool IsRequireUpdateAvatar { get; }
    
    Task Stats(Action<IModifyStatContext>? action = null, bool exclRequest = false);
    Task Inventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false);
    Task Skills(Action<IModifySkillContext>? action = null, bool exclRequest = false);
    Task TemporaryStats(Action<IModifyTemporaryStatContext>? action = null, bool exclRequest = false);
}
