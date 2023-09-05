using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserSkillUseRequestPlug : IPipelinePlug<FieldOnPacketUserSkillUseRequest>
{
    private readonly ITemplateManager<ISkillTemplate> _skillTemplates;
    
    public FieldOnPacketUserSkillUseRequestPlug(ITemplateManager<ISkillTemplate> skillTemplates) 
        => _skillTemplates = skillTemplates;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSkillUseRequest message)
    {
        var skill = await _skillTemplates.Retrieve(message.SkillID);
        var level = skill?.Levels[message.User.Character.Skills[message.SkillID]?.Level ?? 0];

        if (skill == null || level == null) return;
        if (level.MPCon > message.User.Character.MaxMP) return;

        var stats = new List<Tuple<TemporaryStatType, short>>();
        var expire = DateTime.UtcNow.AddSeconds(level.Time);
        
        if (level.PAD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.PAD, level.PAD));
        if (level.PDD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.PDD, level.PDD));
        if (level.MAD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.MAD, level.MAD));
        if (level.MDD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.MDD, level.MDD));

        switch (skill.ID)
        {
            case 
                Skill.FighterWeaponBooster or 
                Skill.PageWeaponBooster or 
                Skill.SpearmanWeaponBooster:
                stats.Add(Tuple.Create(TemporaryStatType.Booster, level.X));
                break;
            case Skill.CrusaderComboAttack or Skill.SoulmasterComboAttack:
                stats.Add(Tuple.Create(TemporaryStatType.ComboCounter, (short)1));
                break;
            case 
                Skill.HeroMapleHero or 
                Skill.PaladinMapleHero or
                Skill.DarkknightMapleHero or 
                Skill.Archmage1MapleHero or 
                Skill.Archmage2MapleHero or 
                Skill.BishopMapleHero or 
                Skill.BowmasterMapleHero or 
                Skill.CrossbowmasterMapleHero or 
                Skill.NightlordMapleHero or 
                Skill.ShadowerMapleHero or 
                Skill.Dual5MapleHero or 
                Skill.ViperMapleHero or 
                Skill.CaptainMapleHero or 
                Skill.AranMapleHero or 
                Skill.EvanMapleHero or 
                Skill.BmageMapleHero or 
                Skill.WildhunterMapleHero or 
                Skill.MechanicMapleHero:
                stats.Add(Tuple.Create(TemporaryStatType.BasicStatUp, level.X));
                break;
        }

        await message.User.Modify(m =>
        {
            m.Stats(s => {
                if (level.MPCon > 0)
                    s.MP -= level.MPCon;
            });
            m.TemporaryStats(s =>
            {
                s.ResetByReason(message.SkillID);
                foreach (var tuple in stats)
                    s.Set(tuple.Item1, tuple.Item2, message.SkillID, expire);
            });
        });
        await message.User.Dispatch(new PacketWriter(PacketSendOperations.SkillUseResult)
            .WriteBool(true)
            .Build());
    }
}
