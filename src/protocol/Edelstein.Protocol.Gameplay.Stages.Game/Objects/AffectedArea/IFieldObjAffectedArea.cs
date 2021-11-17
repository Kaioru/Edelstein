using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.AffectedArea
{
    public interface IFieldObjAffectedArea : IFieldObj
    {
        AffectedAreaType AffectedAreaType { get; }

        int OwnerID { get; }

        int SkillID { get; }
        byte SkillLevel { get; }
        short SkillDelay { get; }

        Rect2D Area { get; }

        int Info { get; }
        int Phase { get; }
    }
}
