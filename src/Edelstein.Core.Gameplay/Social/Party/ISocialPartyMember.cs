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

        Task OnUpdateUserMigration(int channelID, int fieldID);
        Task OnUpdateChangeLevelOrJob(int level, int job);
    }
}