namespace Edelstein.Core.Gameplay.Social.Party.Events
{
    public interface IPartyMemberEvent : IPartyEvent
    {
        public int PartyMemberID { get; }
    }
}