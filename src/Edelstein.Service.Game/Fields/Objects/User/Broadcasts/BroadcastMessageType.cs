namespace Edelstein.Service.Game.Fields.Objects.User.Broadcasts
{
    public enum BroadcastMessageType
    {
        Notice = 0x0,
        Alert = 0x1,
        Speakerchannel = 0x2,
        Speakerworld = 0x3,
        Slide = 0x4,
        Event = 0x5,
        NoticeWithoutPrefix = 0x6,
        UtilDlgEx = 0x7,
        ItemSpeaker = 0x8,
        SpeakerBridge = 0x9,
        ArtSpeakerWorld = 0xA,
        BlowWeather = 0xB,
        GachaponAnnounce = 0xC,
        GachaponAnnounceOpen = 0xD,
        GachaponAnnounceCopy = 0xE,
        UlistClip = 0xF,
        FreemarketClip = 0x10,
        DestroyShop = 0x11,
        CashshopAd = 0x12,
        HeartSpeaker = 0x13,
        SkullSpeaker = 0x14
    }
}