namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public class GuildSetMarkEvent : IGuildEvent
    {
        public int GuildID { get; }

        public short MarkBg { get; }
        public byte MarkBgColor { get; }
        public short Mark { get; }
        public byte MarkColor { get; }

        public GuildSetMarkEvent(int guildID, short markBg, byte markBgColor, short mark, byte markColor)
        {
            GuildID = guildID;
            MarkBg = markBg;
            MarkBgColor = markBgColor;
            Mark = mark;
            MarkColor = markColor;
        }
    }
}