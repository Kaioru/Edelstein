using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Entities.Characters;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public interface ISocialGuildManager
    {
        Task<ISocialGuild> Load(int guildID);
        Task<ISocialGuild?> Load(Character character);
        Task Create(string name, ISocialParty party);
        Task Join(ISocialGuild guild, Character character);
        Task Disband(ISocialGuild guild);
        Task Withdraw(ISocialGuild guild, ISocialGuildMember member);
        Task Kick(ISocialGuild guild, ISocialGuildMember member);

        Task UpdateNotifyLoginOrLogout(ISocialGuild guild, int characterID, bool online);
        Task UpdateChangeLevelOrJob(ISocialGuild guild, int characterID, int level, int job);
        Task UpdateSetGradeName(ISocialGuild guild, string[] name);
        Task UpdateSetMemberGrade(ISocialGuild guild, int characterID, int grade);
        Task UpdateSetMark(ISocialGuild guild, short markBG, byte markBGColor, short mark, byte markColor);
        Task UpdateSetNotice(ISocialGuild guild, string notice);
    }
}