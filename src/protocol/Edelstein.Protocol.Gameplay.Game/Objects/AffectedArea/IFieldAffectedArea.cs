using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;

public interface IFieldAffectedArea : IFieldObject
{
    int OwnerID { get; }
    
    AffectedAreaType AreaType { get; }
    int SkillID { get; }
    int SkillLevel { get; }
    
    int Info { get; }
    int Phase { get; }
    
    IRectangle2D Bounds { get; }
    
    DateTime? DateStart { get; }
    DateTime? DateExpire { get; }

    Task Enter(IFieldObject obj);
    Task Leave(IFieldObject obj);
}
