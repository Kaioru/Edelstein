namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public class GuildSetNoticeEvent : IGuildEvent
    {
        public int GuildID { get; }

        public string Notice { get; }

        public GuildSetNoticeEvent(int guildID, string notice)
        {
            GuildID = guildID;
            Notice = notice;
        }
    }
}