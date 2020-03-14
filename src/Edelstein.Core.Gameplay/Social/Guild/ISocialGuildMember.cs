using System.Threading.Tasks;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public interface ISocialGuildMember
    {
        public int CharacterID { get; }
        public string CharacterName { get; }
        public int Job { get; }
        public int Level { get; }
        public int Grade { get; }
        public bool Online { get; }
        public int Commitment { get; }

        Task Withdraw();
        Task Kick();

        Task UpdateNotifyLoginOrLogout(bool online);
        Task UpdateChangeLevelOrJob(int level, int job);
        Task UpdateSetMemberGrade(byte grade);

        Task OnUpdateNotifyLoginOrLogout(bool online);
        Task OnUpdateChangeLevelOrJob(int level, int job);
        Task OnUpdateSetMemberGrade(int grade);
    }
}