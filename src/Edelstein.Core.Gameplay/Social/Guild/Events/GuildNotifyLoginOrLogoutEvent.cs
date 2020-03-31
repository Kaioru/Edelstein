namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public class GuildNotifyLoginOrLogoutEvent : IGuildMemberEvent
    {
        public int GuildID { get; }
        public int GuildMemberID { get; }

        public bool Online { get; }

        public GuildNotifyLoginOrLogoutEvent(int guildID, int guildMemberID, bool online)
        {
            GuildID = guildID;
            GuildMemberID = guildMemberID;
            Online = online;
        }
    }
}