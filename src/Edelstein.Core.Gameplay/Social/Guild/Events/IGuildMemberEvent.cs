namespace Edelstein.Core.Gameplay.Social.Guild.Events
{
    public interface IGuildMemberEvent : IGuildEvent
    {
        int GuildMemberID { get; }
    }
}