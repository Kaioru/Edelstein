namespace Edelstein.Protocol.Gameplay.Game.Objects.User;

public interface IFieldUserStats
{
    int Level { get; }
    
    int STR { get; }
    int DEX { get; }
    int INT { get; }
    int LUK { get; }
    
    int MaxHP { get; }
    int MaxMP { get; }
    
    int PAD { get; }
    int PDD { get; }
    int MAD { get; }
    int MDD { get; }

    int ACC { get; }
    int EVA { get; }

    int Craft { get; }
    int Speed { get; }
    int Jump { get; }
    
    int STRr { get; }
    int DEXr { get; }
    int INTr { get; }
    int LUKr { get; }
    int MaxHPr { get; }
    int MaxMPr { get; }
    int PADr { get; }
    int PDDr { get; }
    int MADr { get; }
    int MDDr { get; }
    int ACCr { get; }
    int EVAr { get; }
    
    int PACC { get; }
    int MACC { get; }
    int PEVA { get; }
    int MEVA { get; }

    int Ar { get; }
    int Er { get; }
    
    int Cr { get; }
    int CDMin { get; }
    int CDMax { get; }

    int IMDr { get; }

    int PDamR { get; }
    int MDamR { get; }
    int BossDamR { get; }

    int Mastery { get; }
    
    int AttackSpeedBase { get; }
    int AttackSpeed { get; }

    int DamageMin { get; }
    int DamageMax { get; }
    
    int CompletedSetItemID { get; }
    
    IFieldUserStatsSkillLevels SkillLevels { get; }

    Task Apply(IFieldUser user);
    void Reset();
}
