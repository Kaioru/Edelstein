using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Entities.Characters;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public interface ISocialGuildManager
    {
        Task<ISocialGuild> Load(int guildID);
        Task<ISocialGuild?> Load(Character character);
        Task Create(Character character);
        Task Join(ISocialGuild guild, Character character);
        Task Disband(ISocialGuild guild);
        Task Withdraw(ISocialGuild guild, ISocialGuildMember member);
        Task Kick(ISocialGuild guild, ISocialGuildMember member);

        Task UpdateChangeLevelOrJob(ISocialParty guild, int characterID, int level, int job);
        Task UpdateSetGradeName(ISocialParty guild, int characterID, string[] name);
        Task UpdateSetMemberGrade(ISocialParty guild, int characterID, int grade);
        Task UpdateSetMark(ISocialGuild guild, short markBG, byte markBGColor, short mark, byte markColor);
        Task UpdateSetNotice(ISocialGuild guild, string notice);
    }
}