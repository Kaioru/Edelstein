namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public class GuildWithdrawEvent : IGuildMemberEvent
    {
        public int GuildID { get; }
        public int GuildMemberID { get; }

        public bool Disband { get; }
        public bool Kick { get; }
        public string CharacterName { get; }

        public GuildWithdrawEvent(int guildID, int guildMemberID, bool disband, bool kick, string characterName)
        {
            GuildID = guildID;
            GuildMemberID = guildMemberID;
            Disband = disband;
            Kick = kick;
            CharacterName = characterName;
        }
    }
}