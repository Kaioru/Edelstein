namespace Edelstein.Service.Game.Interactions.Miniroom
{
    public enum MiniroomLeaveResult
    {
        UserRequest = 0x0,
        WrongPosition = 0x1,
        Closed = 0x2,
        HostOut = 0x3,
        Booked = 0x4,
        Kicked = 0x5,
        OpenTimeOver = 0x6,
        TradeDone = 0x7,
        TradeFail = 0x8,
        TradeFail_OnlyItem = 0x9,
        TradeFail_Expired = 0xA,
        TradeFail_Denied = 0xB,
        FieldError = 0xC,
        ItemCRCFailed = 0xD,
        NoMoreItem = 0xE,
        KickedTimeOver = 0xF,
        Open = 0x10,
        StartManage = 0x11,
        ClosedTimeOver = 0x12,
        EndManage = 0x13,
        DestoryByAdmin = 0x14
    }
}