namespace Edelstein.Service.Game.Types
{
    public enum MemoRequest : byte
    {
        Send = 0x0,
        Delete = 0x1,
        Load = 0x2
    }
}