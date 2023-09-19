namespace Edelstein.Protocol.Services.Social;

public interface IPartyMembership : IParty, IPartyMember
{
    IDictionary<int, IPartyMember> Members { get; }
}
