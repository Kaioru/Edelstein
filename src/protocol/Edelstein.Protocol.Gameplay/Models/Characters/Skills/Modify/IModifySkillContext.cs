using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Models.Characters.Skills.Modify;

public interface IModifySkillContext : IPacketWritable
{
    void Add(int templateID, int increment = 1);
    void Add(ISkillTemplate template, int increment = 1);

    void Set(int templateID, int level, int? masterLevel = null, DateTime? dateExpire = null);
    void Set(ISkillTemplate template, int level, int? masterLevel = null, DateTime? dateExpire = null);

    void ResetByTemplate(int templateID);
    void ResetByTemplate(ISkillTemplate template);
    void ResetByJobLevel(int jobLevel);
    void ResetAll();
}
