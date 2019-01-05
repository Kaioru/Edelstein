namespace Edelstein.Service.Game.Interactions.Miniroom
{
    public enum MiniRoomAction : byte
    {
        Create = 0x0,
        CreateResult = 0x1,
        Invite = 0x2,
        InviteResult = 0x3,
        Enter = 0x4,
        EnterResult = 0x5,
        Chat = 0x6,
        GameMessage = 0x7,
        UserChat = 0x8,
        Avatar = 0x9,
        Leave = 0xA,
        Balloon = 0xB,
        NotAvailableField = 0xC,
        FreeMarketClip = 0xD,
        CheckSSN2 = 0xE
    }
}