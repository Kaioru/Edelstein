namespace Edelstein.Service.Game.Fields.Continent
{
    public enum ContinentTrigger : byte
    {
        Board = 0x1,
        Start = 0x2,
        MobGen = 0x4,
        MobDestroy = 0x5,
        End = 0x6
    }
}