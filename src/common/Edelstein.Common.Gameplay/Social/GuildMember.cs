using Edelstein.Protocol.Gameplay.Social;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Common.Gameplay.Social
{
    public class GuildMember : IGuildMember
    {
        public int ID { get; }
        public string Name { get; }

        public int Job { get; }
        public int Level { get; }
        public int Grade { get; }
        public bool Online { get; }

        public int Commitment { get; }

        public GuildMember(GuildMemberContract contract)
        {
            ID = contract.Id;
            Name = contract.Name;
            Job = contract.Job;
            Level = contract.Level;
            Grade = contract.Grade;
            Online = contract.Online;
            Commitment = contract.Commitment;
        }
    }
}
