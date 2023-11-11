namespace Edelstein.Protocol.Gameplay.Models.Characters.Stats;

public interface ITemporaryStatDiceInfo
{
    int MHPr { get; set; }
    int MMPr { get; set; }
    int Cr { get; set; }
    int CDMin { get; set; }
    int EVAr { get; set; }
    int Ar { get; set; }
    int Er { get; set; }
    int PDDr { get; set; }
    int MDDr { get; set; }
    int PDr { get; set; }
    int MDr { get; set; }
    int DIPr { get; set; }
    int PDamr { get; set; }
    int MDamr { get; set; }
    int PADr { get; set; }
    int MADr { get; set; }
    int EXPr { get; set; }
    int IMPr { get; set; }
    int ASRr { get; set; }
    int TERr { get; set; }
    int MESOr { get; set; }

    void Reset();
}
