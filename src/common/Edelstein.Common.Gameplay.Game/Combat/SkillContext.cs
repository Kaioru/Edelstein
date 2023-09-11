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
    
    private SkillContextTarget? TargetField { get; set; }
    private SkillContextTarget? TargetParty{ get; set; }
    
    private int? MobCount { get; set; }
    
    private bool IsResetAuras { get; set; }
    private bool IsResetComboCounter { get; set; }
    
    public SkillContext(
        IFieldUser user,
        Random random, 
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
        
        Random = random;
        Skill = skill;
        SkillLevel = skillLevel;
        IsHitMob = isHitMob;
    }
    
    public Random Random { get; }
    
    public ISkillTemplate? Skill { get; }
    public ISkillTemplateLevel? SkillLevel { get; }
    
    public bool IsHitMob { get; }
    
    public void SetTargetField(bool active = true, int? limit = null, IRectangle2D? bounds = null)
        => TargetField = active 
            ? new SkillContextTarget(
                bounds ?? (SkillLevel == null
                    ? new Rectangle2D(_user.Position, _user.Field?.Template.Bounds ?? new Rectangle2D())
                    : new Rectangle2D(_user.Position, SkillLevel.Bounds)))
            : null;
    
    public void SetTargetParty(bool active = true, IRectangle2D? bounds = null)
        => TargetParty = active 
            ? new SkillContextTarget(
                bounds ?? (SkillLevel == null
                    ? new Rectangle2D(_user.Position, _user.Field?.Template.Bounds ?? new Rectangle2D())
                    : new Rectangle2D(_user.Position, SkillLevel.Bounds)))
            : null;

    public void SetMobCount(int? count = null) => MobCount = count;

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
                        _user.Position.Y - 60
                    ), 
                    SkillLevel.Bounds
            )),
            expire ?? _now.AddSeconds(SkillLevel?.Time ?? 0)
        ));

    public void ResetAuras() => IsResetAuras = true;

    public void ResetComboCounter() => IsResetComboCounter = true;

    public async Task Execute()
    {
        
        await _user.Modify(m =>
        {
            m.TemporaryStats(s =>
            {
                if (IsResetAuras)
                {
                    s.ResetByType(TemporaryStatType.DarkAura);
                    s.ResetByType(TemporaryStatType.BlueAura);
                    s.ResetByType(TemporaryStatType.YellowAura);
                }

                if (IsResetComboCounter)
                {
                    var comboCounterStat = _user.Character.TemporaryStats[TemporaryStatType.ComboCounter];

                    if (comboCounterStat != null)
                        s.Set(
                            TemporaryStatType.ComboCounter,
                            1, 
                            comboCounterStat.Reason,
                            comboCounterStat.DateExpire
                        );
                }
            });
        });
        
        var targets = new HashSet<IFieldObject>{ _user };

        if (_mob != null)
            targets.Add(_mob);

        if (TargetField != null && _user.Field != null)
            foreach (var target in _user.Field
                         .GetSplits(TargetField.Bounds)
                         .Where(s => s != null)
                         .SelectMany(s => s!.Objects)
                         .Where(o => TargetField.Bounds.Intersects(o.Position))
                         .OrderBy(u => _user.Position.Distance(u.Position)))
                targets.Add(target);
        
        if (TargetParty != null && _user.Field != null)
            foreach (var target in _user.Field
                         .GetSplits(TargetParty.Bounds)
                         .Where(s => s != null)
                         .SelectMany(s => s!.Objects)
                         .Where(o => TargetParty.Bounds.Intersects(o.Position))
                         .OrderBy(u => _user.Position.Distance(u.Position)))
                         //.Where(Party..))
                targets.Add(target);

        if (_addTemporaryStat.Count > 0)
        {
            await Task.WhenAll(targets
                .OfType<IFieldUser>()
                .Select(t => t.ModifyTemporaryStats(s =>
                {
                    foreach (var ts in _addTemporaryStat)
                        s.Set(ts.Type, ts.Value, ts.Reason, ts.Expire);
                })));
        }

        if (_addMobTemporaryStat.Count > 0 || _addBurnedInfo.Count > 0)
        {
            await Task.WhenAll(targets
                .OfType<IFieldMob>()
                .Take(MobCount ?? SkillLevel?.MobCount ?? 100)
                .Select(t => t.ModifyTemporaryStats(s =>
                {
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
        }

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
                Console.WriteLine(affectedArea.Bounds);
                
                if (_user.Field != null)
                    await _user.Field.Enter(obj, () => obj.GetEnterFieldPacket());
            }
        }
    }
}
