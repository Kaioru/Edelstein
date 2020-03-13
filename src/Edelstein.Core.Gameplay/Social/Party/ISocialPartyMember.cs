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
        int FieldID { get; }
        
        Task Withdraw();
        Task Kick();
        Task ChangeBoss(bool disconnect = false);
        
        Task UpdateUserMigration(int channelID, int fieldID);
        Task UpdateChangeLevelOrJob(int level, int job);

        Task OnUpdateUserMigration(int channelID, int fieldID);
        Task OnUpdateChangeLevelOrJob(int level, int job);
    }
}