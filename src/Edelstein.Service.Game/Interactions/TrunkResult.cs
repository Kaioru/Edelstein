namespace Edelstein.Service.Game.Interactions
{
    public enum TrunkResult
    {
        GetSuccess = 0x9,
        GetUnknown = 0xA,
        GetNoMoney = 0xB,
        GetHavingOnlyItem = 0xC,
        PutSuccess = 0xD,
        PutIncorrectRequest = 0xE,
        SortItem = 0xF,
        PutNoMoney = 0x10,
        PutNoSpace = 0x11,
        PutUnknown = 0x12,
        MoneySuccess = 0x13,
        MoneyUnknown = 0x14,
        TrunkCheckSSN2 = 0x15,
        OpenTrunkDlg = 0x16,
        TradeBlocked = 0x17,
        ServerMsg = 0x18
    }
}