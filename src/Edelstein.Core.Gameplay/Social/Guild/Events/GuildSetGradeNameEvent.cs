namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public class GuildSetGradeNameEvent : IGuildEvent
    {
        public int GuildID { get; }

        public string[] GradeName { get; }

        public GuildSetGradeNameEvent(int guildID, string[] gradeName)
        {
            GuildID = guildID;
            GradeName = gradeName;
        }
    }
}