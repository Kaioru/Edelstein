using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills;
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
            case Skill.FighterWeaponBooster:
            case Skill.PageWeaponBooster:
            case Skill.SpearmanWeaponBooster:
                stats.Add(Tuple.Create(TemporaryStatType.Booster, level.X));
                break;
            case Skill.FighterPowerGuard:
            case Skill.PagePowerGuard:
                stats.Add(Tuple.Create(TemporaryStatType.PowerGuard, level.X));
                break;
            case Skill.CrusaderComboAttack or Skill.SoulmasterComboAttack:
                stats.Add(Tuple.Create(TemporaryStatType.ComboCounter, (short)1));
                break;
            case Skill.KnightFireCharge:
            case Skill.KnightIceCharge:
            case Skill.KnightLightningCharge:
                stats.Add(Tuple.Create(TemporaryStatType.WeaponCharge, level.X));
                break;
            case Skill.HeroMapleHero:
            case Skill.PaladinMapleHero:
            case Skill.DarkknightMapleHero:
            case Skill.Archmage1MapleHero:
            case Skill.Archmage2MapleHero:
            case Skill.BishopMapleHero:
            case Skill.BowmasterMapleHero:
            case Skill.CrossbowmasterMapleHero:
            case Skill.NightlordMapleHero:
            case Skill.ShadowerMapleHero:
            case Skill.Dual5MapleHero:
            case Skill.ViperMapleHero:
            case Skill.CaptainMapleHero:
            case Skill.AranMapleHero:
            case Skill.EvanMapleHero:
            case Skill.BmageMapleHero:
            case Skill.WildhunterMapleHero:
            case Skill.MechanicMapleHero:
                stats.Add(Tuple.Create(TemporaryStatType.BasicStatUp, level.X));
                break;
            case Skill.HeroStance:
            case Skill.PaladinStance:
            case Skill.DarkknightStance:
            case Skill.AranFreezeStanding:
            case Skill.BmageStance:
                stats.Add(Tuple.Create(TemporaryStatType.Stance, level.Prop));
                break;
            case Skill.HeroEnrage:
                stats.Add(Tuple.Create(TemporaryStatType.Enrage, (short)(
                    level.X * 100 +
                    level.MobCount
                )));
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
                if (skill.ID == Skill.HeroEnrage)
                {
                    var comboCounterStat = message.User.Character.TemporaryStats[TemporaryStatType.ComboCounter];
                    if (comboCounterStat != null)
                        s.Set(
                            TemporaryStatType.ComboCounter,
                            1,
                            comboCounterStat.Reason,
                            comboCounterStat.DateExpire
                        );
                }
                
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
