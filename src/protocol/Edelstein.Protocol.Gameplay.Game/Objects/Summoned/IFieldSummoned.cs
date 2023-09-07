namespace Edelstein.Protocol.Gameplay.Game.Objects.Summoned;

public interface IFieldSummoned : 
    IFieldLife<IFieldSummonedMovePath, IFieldSummonedMoveAction>, 
    IFieldObjectOwned
{
    int SkillID { get; }
    byte SkillLevel { get; }
    
    MoveAbilityType MoveAbility { get; }
    SummonedAssistType AssistType { get; }
}
