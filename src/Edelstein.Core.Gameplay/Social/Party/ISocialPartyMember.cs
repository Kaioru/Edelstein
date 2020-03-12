using System.Threading.Tasks;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public interface ISocialPartyMember
    {
        int CharacterID { get; }
        string CharacterName { get; }
        int Job { get; }
        int Level { get; }
        int ChannelID { get; }

        Task ChangeBoss();
        Task Withdraw();
        Task Kick();
    }
}