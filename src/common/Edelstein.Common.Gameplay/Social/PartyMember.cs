using Edelstein.Protocol.Gameplay.Social;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Common.Gameplay.Social
{
    public class PartyMember : IPartyMember
    {
        public int ID { get; }
        public string Name { get; }
        public int Job { get; }
        public int Level { get; }
        public int Channel { get; }
        public int Field { get; }

        public PartyMember(PartyMemberContract contract)
        {
            ID = contract.Id;
            Name = contract.Name;
            Job = contract.Job;
            Level = contract.Level;
            Channel = contract.Channel;
            Field = contract.Field;
        }
    }
}
