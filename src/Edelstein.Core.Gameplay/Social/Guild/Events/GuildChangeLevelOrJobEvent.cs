namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public class GuildChangeLevelOrJobEvent : IGuildMemberEvent
    {
        public int GuildID { get; }
        public int GuildMemberID { get; }

        public int Level { get; }
        public int Job { get; }

        public GuildChangeLevelOrJobEvent(int guildID, int guildMemberID, int level, int job)
        {
            GuildID = guildID;
            GuildMemberID = guildMemberID;
            Level = level;
            Job = job;
        }
    }
}