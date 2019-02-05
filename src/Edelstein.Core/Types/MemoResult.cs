namespace Edelstein.Core.Types
{
    public enum MemoResult : byte
    {
        Load = 0x3,
        SendSucceed = 0x4,
        SendWarning = 0x5,
        SendConfirmOnline = 0x6,
        NotifyReceive = 0x7
    }
}