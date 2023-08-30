using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public struct FieldUserStats : IFieldUserStats
{
    public int Level { get; }
    
    public int STR { get; }
    public int DEX { get; }
    public int INT { get; }
    public int LUK { get; }
    
    public int MaxHP { get; }
    public int MaxMP { get; }
    
    public int PAD { get; }
    public int PDD { get; }
    public int MAD { get; }
    public int MDD { get; }
    public int ACC { get; }
    public int EVA { get; }
    
    public int Craft { get; }
    public int Speed { get; }
    public int Jump { get; }
    
    public int STRr { get; }
    public int DEXr { get; }
    public int INTr { get; }
    public int LUKr { get; }
    public int MaxHPr { get; }
    public int MaxMPr { get; }
    public int PADr { get; }
    public int PDDr { get; }
    public int MADr { get; }
    public int MDDr { get; }
    public int ACCr { get; }
    public int EVAr { get; }
    
    public int PACC { get; }
    public int MACC { get; }
    public int PEVA { get; }
    public int MEVA { get; }
    
    public int Ar { get; }
    public int Er { get; }
    
    public int Cr { get; }
    public int CDMin { get; }
    public int CDMax { get; }
    
    public int IMDr { get; }
    
    public int PDamR { get; }
    public int MDamR { get; }
    public int BossDamR { get; }
    
    public int Mastery { get; }

    public int DamageMin { get; }
    public int DamageMax { get; }

    public FieldUserStats(
        IFieldUser user, 
        ITemplateManager<IItemTemplate> itemTemplates,
        ITemplateManager<ISkillTemplate> skillTemplate
    )
    {
        var character = user.Character;
        
        STR = character.STR;
        DEX = character.DEX;
        INT = character.INT;
        LUK = character.LUK;
        MaxHP = character.MaxHP;
        MaxMP = character.MaxMP;
        
        Craft = INT + DEX + LUK;
        Speed = 100;
        Jump = 100;
        
        Cr = 5;
        CDMin = 20;
        CDMax = 50;
        
        Mastery = 0;
        
        DamageMin = 1;
        DamageMax = 1;
        
        STR += (int)(STR * (STRr / 100d));
        DEX += (int)(DEX * (DEXr / 100d));
        INT += (int)(INT * (INTr / 100d));
        LUK += (int)(LUK * (LUKr / 100d));
        MaxHP += (int)(MaxHP * (MaxHPr / 100d));
        MaxMP += (int)(MaxMP * (MaxMPr / 100d));
        
        PAD += (int)(PAD * (PADr / 100d));
        PDD += (int)(STR * 1.2 + LUK * 0.5 + DEX * 0.5 + INT * 0.4);
        PDD += (int)(PDD * (PDDr / 100d));
        MAD += (int)(MAD * (MADr / 100d));
        MDD += (int)(INT * 1.2 + DEX * 0.5 + LUK * 0.5 + STR * 0.4);
        MDD += (int)(MDD * (MDDr / 100d));
        
        PACC = (int)(DEX * 1.2 + LUK) + ACC;
        PACC += (int)(PACC * (ACCr / 100d));
        MACC = (int)(LUK * 1.2 + INT) + ACC;
        MACC += (int)(MACC * (ACCr / 100d));
        PEVA = LUK * 2 + DEX + EVA;
        PEVA += (int)(PEVA * (EVAr / 100d));
        MEVA = LUK * 2 + INT + EVA;
        MEVA += (int)(MEVA * (EVAr / 100d));
        
        MaxHP = Math.Min(MaxHP, 99999);
        MaxMP = Math.Min(MaxMP, 99999);

        PAD = Math.Min(PAD, 29999);
        PDD = Math.Min(PDD, 30000);
        MAD = Math.Min(MAD, 29999);
        MDD = Math.Min(MDD, 30000);
        PACC = Math.Min(PACC, 9999);
        MACC = Math.Min(MACC, 9999);
        PEVA = Math.Min(PEVA, 9999);
        MEVA = Math.Min(MEVA, 9999);
        Speed = Math.Min(Math.Max(Speed, 100), 140);
        Jump = Math.Min(Math.Max(Jump, 100), 123);
        
        CDMin = Math.Min(CDMin, CDMax);
        
        DamageMin = Math.Min(DamageMin, DamageMax);
        DamageMin = Math.Min(Math.Max(DamageMin, 1), 999999);
        DamageMax = Math.Min(Math.Max(DamageMax, 1), 999999);
    }
}
