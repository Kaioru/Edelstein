namespace Edelstein.Service.Game.Interactions.Miniroom
{
    public enum MiniRoomEnterResult : byte
    {
        Success = 0x0,
        NoRoom = 0x1,
        Full = 0x2,
        Busy = 0x3,
        Dead = 0x4,
        Event = 0x5,
        PermissionDenied = 0x6,
        NoTrading = 0x7,
        Etc = 0x8,
        OnlyInSameField = 0x9,
        NearPortal = 0xA,
        CreateCountOver = 0xB,
        CreateIPCountOver = 0xC,
        ExistMiniRoom = 0xD,
        NotAvailableField_Game = 0xE,
        NotAvailableField_PersonalShop = 0xF,
        NotAvailableField_EntrustedShop = 0x10,
        OnBlockedList = 0x11,
        IsManaging = 0x12,
        Tournament = 0x13,
        AlreadyPlaying = 0x14,
        NotEnoughMoney = 0x15,
        InvalidPassword = 0x16,
        NotAvailableField_ShopScanner = 0x17,
        Expired = 0x18,
        TooShortTimeInterval = 0x19
    }
}