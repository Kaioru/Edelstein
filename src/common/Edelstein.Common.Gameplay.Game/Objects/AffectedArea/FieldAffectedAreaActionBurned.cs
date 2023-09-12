using Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Objects.AffectedArea;

public class FieldAffectedAreaActionBurned : AbstractFieldAffectedAreaAction
{
    private readonly IFieldUser _user;
    private readonly int _skillID;
    private readonly int _skillLevel;
    private readonly TimeSpan _interval;
    private readonly TimeSpan _duration;
    
    public FieldAffectedAreaActionBurned(IFieldUser user, int skillID, int skillLevel, TimeSpan interval, TimeSpan duration)
    {
        _user = user;
        _skillID = skillID;
        _skillLevel = skillLevel;
        _interval = interval;
        _duration = duration;
    }
    
    public override async Task OnEnter(IFieldObject obj)
    {
        await OnTick(obj);
        await base.OnEnter(obj);
    }

    public override async Task OnTick(IFieldObject obj)
    {
        if (obj is not IFieldMob mob) return;
        if (mob.TemporaryStats.BurnedInfo
                .FirstOrDefault(b => 
                    b.CharacterID == _user.Character.ID && 
                    b.SkillID == _skillID) != null
           ) return;
            
        var now = DateTime.UtcNow;
        var damage = await _user.Damage.CalculateBurnedDamage(
            _user.Character,
            _user.Stats,
            mob,
            mob.Stats,
            _skillID,
            _skillLevel
        );

        _ = mob.ModifyTemporaryStats(s => s.SetBurnedInfo(new MobBurnedInfo(
            _user.Character.ID,
            _skillID,
            damage,
            _interval,
            now,
            now.Add(_duration)
        )));

        await base.OnTick(obj);
    }
}
