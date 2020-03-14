using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Core.Gameplay.Social.Guild
{
    public interface ISocialGuild
    {
        public int ID { get; }

        public string Name { get; }

        public string[] GradeName { get; }

        public int MaxMemberNum { get; }
        public ICollection<ISocialGuildMember> Members { get; }

        public short MarkBg { get; }
        public byte MarkBgColor { get; }
        public short Mark { get; }
        public byte MarkColor { get; }

        public string Notice { get; }
        public int Point { get; }
        public byte Level { get; }
        
        
        Task OnUpdateJoin(ISocialGuildMember member);
        Task OnUpdateWithdraw(int characterID);
        Task OnUpdateNotifyLoginOrLogout(int characterID, bool online);
        Task OnUpdateChangeLevelOrJob(int characterID, int level, int job);
        Task OnUpdateSetGradeName(string[] name);
        Task OnUpdateSetMemberGrade(int characterID, int grade);
        Task OnUpdateSetMark(short markBG, byte markBGColor, short mark, byte markColor);
        Task OnUpdateSetNotice(string notice);
    }
}