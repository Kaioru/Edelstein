using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Combat.Contexts;
using Edelstein.Common.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Common.Gameplay.Game.Objects.Summoned;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Combat;

public class SkillContext : ISkillContext
{
    private readonly IFieldUser _user;
    private readonly IFieldMob? _mob;
    private readonly DateTime _now;
    
    private readonly ICollection<SkillContextTemporaryStat> _addTemporaryStat;
    private readonly ICollection<SkillContextMobTemporaryStat> _addMobTemporaryStat;
    private readonly ICollection<SkillContextBurnedInfo> _addBurnedInfo;
    
    private readonly ICollection<SkillContextSummoned> _addSummoned;
    
    private readonly ICollection<SkillContextAffectedArea> _addAffectedArea;
    private readonly ICollection<SkillContextAffectedAreaBurnedInfo> _addAffectedAreaBurnedInfo;

    private readonly ICollection<int> _resetTemporaryStatBySkill;
    private readonly ICollection<TemporaryStatType> _resetTemporaryStatByType;
    private readonly ICollection<SkillContextTemporaryStatExisting> _resetTemporaryStatExisting;
    
    private readonly ICollection<int> _resetMobTemporaryStatBySkill;
    private readonly ICollection<MobTemporaryStatType> _resetMobTemporaryStatByType;

    private readonly ICollection<int> _resetSummoned;

    private SkillContextTarget? TargetFieldInfo { get; set; }
    private SkillContextTarget? TargetPartyInfo { get; set; }
    
    private int? Proc { get; set; }
    private int? RecoverHP { get; set; }
    private int? RecoverMP { get; set; }
    
    public SkillContext(
        IFieldUser user,
        ISkillTemplate? skill, 
        ISkillTemplateLevel? skillLevel, 
        bool isHitMob = false,
        IFieldMob? mob = null
    )
    {
        _user = user;
        _mob = mob;
        _now = DateTime.UtcNow;

        _addTemporaryStat = new List<SkillContextTemporaryStat>();
        _addMobTemporaryStat = new List<SkillContextMobTemporaryStat>();
        _addBurnedInfo = new List<SkillContextBurnedInfo>();
        
        _addSummoned = new List<SkillContextSummoned>();
        
        _addAffectedArea = new List<SkillContextAffectedArea>();
        _addAffectedAreaBurnedInfo = new List<SkillContextAffectedAreaBurnedInfo>();

        _resetTemporaryStatBySkill = new List<int>();
        _resetTemporaryStatByType = new List<TemporaryStatType>();
        _resetTemporaryStatExisting = new List<SkillContextTemporaryStatExisting>();

        _resetMobTemporaryStatBySkill = new List<int>();
        _resetMobTemporaryStatByType = new List<MobTemporaryStatType>();
        
        _resetSummoned = new List<int>();
        
        Random = new Random();
        
        Skill = skill;
        SkillLevel = skillLevel;
        IsHitMob = isHitMob;
    }

    public Random Random { get; }
    public ISkillTemplate? Skill { get; }
    public ISkillTemplateLevel? SkillLevel { get; }
    
    public bool IsHitMob { get; }
    
    public void TargetField(bool active = true, int? limit = null, IRectangle2D? bounds = null)
        => TargetFieldInfo = active 
            ? new SkillContextTarget(
                bounds ?? (SkillLevel == null
                    ? new Rectangle2D(_user.Position, _user.Field?.Template.Bounds ?? new Rectangle2D())
                    : new Rectangle2D(_user.Position, SkillLevel.Bounds)))
            : null;
    
    public void TargetParty(bool active = true, IRectangle2D? bounds = null)
        => TargetPartyInfo = active 
            ? new SkillContextTarget(
                bounds ?? (SkillLevel == null
                    ? new Rectangle2D(_user.Position, _user.Field?.Template.Bounds ?? new Rectangle2D())
                    : new Rectangle2D(_user.Position, SkillLevel.Bounds)))
            : null;

    public void SetProc(int? proc = null)
        => Proc = proc ?? (SkillLevel?.Prop > 0
            ? SkillLevel.Prop
            : 0);

    public void SetRecoverHP(int? hp = null)
        => RecoverHP = hp ?? (SkillLevel?.HP > 0
            ? (int)(_user.Stats.MaxHP * SkillLevel.HP / 100d)
            : 0);
    
    public void SetRecoverMP(int? mp = null)
        => RecoverMP = mp ?? (SkillLevel?.MP > 0
            ? (int)(_user.Stats.MaxMP * SkillLevel.MP / 100d)
            : 0);

    public void AddTemporaryStat(TemporaryStatType type, int value, int? reason = null, DateTime? expire = null)
        => _addTemporaryStat.Add(new SkillContextTemporaryStat(
            type, 
            value, 
            reason ?? Skill?.ID ?? 0, 
            expire ?? _now.AddSeconds(SkillLevel?.Time ?? 0)
        ));

    public void AddMobTemporaryStat(MobTemporaryStatType type, int value, int? reason = null, DateTime? expire = null)
        => _addMobTemporaryStat.Add(new SkillContextMobTemporaryStat(
            type, 
            value, 
            reason ?? Skill?.ID ?? 0, 
            expire ?? _now.AddSeconds(SkillLevel?.Time ?? 0)
        ));

    public void AddMobBurned(int damage, int? skillID = null, TimeSpan? interval = null, DateTime? expire = null)
        => _addBurnedInfo.Add(new SkillContextBurnedInfo(
            damage,
            skillID ?? Skill?.ID ?? 0,
            interval ?? TimeSpan.FromSeconds(SkillLevel?.DotInterval ?? 0),
            expire ?? _now.AddSeconds(SkillLevel?.DotTime ?? 0)
        ));

    public void AddSummoned(MoveAbilityType moveAbilityType, SummonedAssistType summonedAssistType, int? skillID = null, int? skillLevel = null, DateTime? expire = null)
        => _addSummoned.Add(new SkillContextSummoned(
                moveAbilityType,
                summonedAssistType,
                skillID ?? Skill?.ID ?? 0,
                skillLevel ?? SkillLevel?.Level ?? 0,
                expire ?? _now.AddSeconds(SkillLevel?.Time ?? 0)
        ));

    public void AddAffectedArea(AffectedAreaType type, int? skillID = null, int? skillLevel = null, int? info = null, int? phase = null, IRectangle2D? bounds = null, DateTime? expire = null)
        => _addAffectedArea.Add(new SkillContextAffectedArea(
            type,
            skillID ?? Skill?.ID ?? 0,
            skillLevel ?? SkillLevel?.Level ?? 0,
            info ?? 0,
            phase ?? 0,
            bounds ?? (SkillLevel == null
                ? new Rectangle2D()
                : new Rectangle2D(
                    new Point2D(
                        _user.Position.X, 
                        _user.Position.Y - (int)(SkillLevel.Bounds.Height / 2) + 10
                    ), 
                    SkillLevel.Bounds
            )),
            expire ?? _now.AddSeconds(SkillLevel?.Time ?? 0)
        ));
    
    public void AddAffectedAreaBurnedInfo(int? skillID = null, int? skillLevel = null, TimeSpan? interval = null, TimeSpan? duration = null, IFieldUser? user = null)
        => _addAffectedAreaBurnedInfo.Add(new SkillContextAffectedAreaBurnedInfo(
            skillID ?? Skill?.ID ?? 0,
            skillLevel ?? SkillLevel?.Level ?? 0,
            interval ?? TimeSpan.FromSeconds(SkillLevel?.DotInterval ?? 0),
            duration ?? TimeSpan.FromSeconds(SkillLevel?.DotTime ?? 0),
            user ?? _user
        ));

    public void ResetTemporaryStatBySkill(int? skillID = null)
        => _resetTemporaryStatBySkill.Add(skillID ?? Skill?.ID ?? 0);
    
    public void ResetTemporaryStatByType(TemporaryStatType type)
        => _resetTemporaryStatByType.Add(type);

    public void ResetTemporaryStatExisting(TemporaryStatType type, int value)
        => _resetTemporaryStatExisting.Add(new SkillContextTemporaryStatExisting(type, value));
    
    public void ResetMobTemporaryStatBySkill(int? skillID = null)
        => _resetMobTemporaryStatBySkill.Add(skillID ?? Skill?.ID ?? 0);
    
    public void ResetMobTemporaryStatByType(MobTemporaryStatType type)
        => _resetMobTemporaryStatByType.Add(type);

    public void ResetSummoned(int? skillID = null) => _resetSummoned.Add(skillID ?? Skill?.ID ?? 0);

    public async Task Execute()
    {
        var targets = new HashSet<IFieldObject>{ _user };

        if (_mob != null)
            targets.Add(_mob);

        if (TargetFieldInfo != null && _user.Field != null)
            foreach (var target in _user.Field
                         .GetSplits(TargetFieldInfo.Bounds)
                         .Where(s => s != null)
                         .SelectMany(s => s!.Objects)
                         .Where(o => TargetFieldInfo.Bounds.Intersects(o.Position))
                         .Where(o => Proc == null || Random.Next(0, 100) <= Proc)
                         .OrderBy(u => _user.Position.Distance(u.Position)))
                targets.Add(target);
        
        if (TargetPartyInfo != null && _user.Field != null)
            foreach (var target in _user.Field
                         .GetSplits(TargetPartyInfo.Bounds)
                         .Where(s => s != null)
                         .SelectMany(s => s!.Objects)
                         .Where(o => TargetPartyInfo.Bounds.Intersects(o.Position))
                         .Where(o => Proc == null || Random.Next(0, 100) <= Proc)
                         //.Where(Party..)
                         .OrderBy(u => _user.Position.Distance(u.Position)))
                targets.Add(target);

        await Task.WhenAll(targets
            .OfType<IFieldUser>()
            .Select(t => t.Modify(m =>
            {
                m.Stats(s =>
                {
                    if (RecoverHP != null)
                        s.HP += RecoverHP.Value;
                    if (RecoverMP != null)
                        s.MP += RecoverMP.Value;
                });
                
                m.TemporaryStats(s =>
                {
                    foreach (var x in _resetTemporaryStatBySkill)
                        s.ResetByReason(x);
                    foreach (var x in _resetTemporaryStatByType)
                        s.ResetByType(x);
                    foreach (var x in _resetTemporaryStatExisting)
                    {
                        var existingStat = t.Character.TemporaryStats[x.Type];
                        if (existingStat != null)
                            s.Set(x.Type, x.Value, existingStat.Reason, existingStat.DateExpire);
                    }

                    foreach (var ts in _addTemporaryStat)
                        s.Set(ts.Type, ts.Value, ts.Reason, ts.Expire);
                });
            })));

        await Task.WhenAll(targets
            .OfType<IFieldMob>()
            .Take(
                SkillLevel == null 
                    ? 100
                    : SkillLevel.MobCount > 0
                        ? SkillLevel.MobCount
                        : 100
            )
            .Select(t => t.ModifyTemporaryStats(s =>
            {
                foreach (var x in _resetMobTemporaryStatBySkill)
                    s.ResetByReason(x);
                foreach (var x in _resetMobTemporaryStatByType)
                    s.ResetByType(x);
                if (_addMobTemporaryStat.Count > 0)
                    foreach (var mts in _addMobTemporaryStat)
                        s.Set(mts.Type, mts.Value, mts.Reason, mts.Expire);
                if (_addBurnedInfo.Count > 0)
                    foreach (var b in _addBurnedInfo)
                        s.SetBurnedInfo(new MobBurnedInfo(
                            _user.Character.ID,
                            b.SkillID,
                            b.Damage,
                            b.Interval,
                            _now,
                            b.Expire
                        ));
            })));

        if (_addSummoned.Count > 0)
        {
            foreach (var summoned in _addSummoned)
            {
                var existing = _user.Owned
                    .OfType<IFieldSummoned>()
                    .FirstOrDefault(o => o.SkillID == summoned.SkillID);

                if (existing != null)
                {
                    _user.Owned.Remove(existing);
                    if (_user.Field != null)
                        await _user.Field.Leave(existing);
                }

                var obj = new FieldSummoned(
                    _user,
                    summoned.SkillID,
                    (byte)summoned.SkillLevel,
                    summoned.MoveAbilityType,
                    summoned.SummonedAssistType,
                    _user.Position,
                    _user.Foothold,
                    summoned.Expire
                );
                
                _user.Owned.Add(obj);
                if (_user.Field != null)
                    await _user.Field.Enter(obj, () => obj.GetEnterFieldPacket(1));
            }
        }

        if (_resetSummoned.Count > 0)
        {
            foreach (var summoned in _user.Owned
                         .OfType<IFieldSummoned>()
                         .Where(s => _resetSummoned.Contains(s.SkillID))
                         .ToImmutableList())
            {
                _user.Owned.Remove(summoned);
                if (_user.Field != null)
                    await _user.Field.Leave(summoned);
            }
        }

        if (_addAffectedArea.Count > 0)
        {
            foreach (var affectedArea in _addAffectedArea)
            {
                var obj = new FieldAffectedArea(
                    _user.Character.ID,
                    affectedArea.Type,
                    affectedArea.SkillID,
                    affectedArea.SkillLevel,
                    affectedArea.Info,
                    affectedArea.Phase,
                    affectedArea.Bounds,
                    _now,
                    affectedArea.Expire
                );
                
                if (_addAffectedAreaBurnedInfo.Count > 0)
                    foreach (var burned in _addAffectedAreaBurnedInfo)
                        obj.Actions.Add(new FieldAffectedAreaActionBurned(
                            _user,
                            burned.SkillID,
                            burned.SkillLevel,
                            burned.Interval,
                            burned.Duration
                        ));
                
                if (_user.Field != null)
                    await _user.Field.Enter(obj, () => obj.GetEnterFieldPacket());
            }
        }
    }
}
