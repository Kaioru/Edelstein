namespace Edelstein.Common.Services.Social
{
    public record PartyUpdateEvent
    {
        public PartyRecord Party { get; init; }
    }
}
