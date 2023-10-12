using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Bmage1SkillHandler : CitizenSkillHandler
{
    public override int ID => Job.Bmage;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BmageAuraDark:
                // TODO advanced
                context.ResetTemporaryStatAuras();
                context.AddTemporaryStat(TemporaryStatType.DarkAura, context.SkillLevel!.X, expire: DateTime.MaxValue);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
