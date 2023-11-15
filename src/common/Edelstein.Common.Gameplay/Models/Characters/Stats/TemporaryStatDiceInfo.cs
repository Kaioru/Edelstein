using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats;

public record TemporaryStatDiceInfo : ITemporaryStatDiceInfo
{
    public int MHPr { get; set; }
    public int MMPr { get; set; }
    public int Cr { get; set; }
    public int CDMin { get; set; }
    public int EVAr { get; set; }
    public int Ar { get; set; }
    public int Er { get; set; }
    public int PDDr { get; set; }
    public int MDDr { get; set; }
    public int PDr { get; set; }
    public int MDr { get; set; }
    public int DIPr { get; set; }
    public int PDamr { get; set; }
    public int MDamr { get; set; }
    public int PADr { get; set; }
    public int MADr { get; set; }
    public int EXPr { get; set; }
    public int IMPr { get; set; }
    public int ASRr { get; set; }
    public int TERr { get; set; }
    public int MESOr { get; set; }
    
    public void Reset()
    {
        MHPr = 0;
        MMPr = 0;
        Cr = 0;
        CDMin = 0;
        EVAr = 0;
        Ar = 0;
        Er = 0;
        PDDr = 0;
        MDDr = 0;
        PDr = 0;
        MDr = 0;
        DIPr = 0;
        PDamr = 0;
        MDamr = 0;
        PADr = 0;
        MADr = 0;
        EXPr = 0;
        IMPr = 0;
        ASRr = 0;
        TERr = 0;
        MESOr = 0;
    }
}
