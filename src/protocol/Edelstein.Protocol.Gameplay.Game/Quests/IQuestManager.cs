using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Quests;

public interface IQuestManager
{
    Task ActStart(IFieldUser user);
    Task ActEnd(IFieldUser user);
    
    Task<bool> CheckStart(IFieldUser user);
    Task<bool> CheckEnd(IFieldUser user);
}
