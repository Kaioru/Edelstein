﻿using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface ISkillContext
{
    Random Random { get; }

    ISkillTemplate? Skill { get; }
    ISkillTemplateLevel? SkillLevel { get; }
    
    bool IsHitMob { get; }

    void SetTargetField(bool active = true, int? limit = null, IRectangle2D? bounds = null);
    void SetTargetParty(bool active = true, IRectangle2D? bounds = null);

    void SetMobCount(int? count = null);

    void AddTemporaryStat(TemporaryStatType type, int value, int? reason = null, DateTime? expire = null);
    void AddMobTemporaryStat(MobTemporaryStatType type, int value, int? reason = null, DateTime? expire = null);
    void AddMobBurned(int damage, int? skillID = null, TimeSpan? interval = null, DateTime? expire = null);
    
    void AddSummoned(MoveAbilityType moveAbilityType, SummonedAssistType summonedAssistType, int? skillID = null, int? skillLevel = null, DateTime? expire = null);

    void AddAffectedArea(AffectedAreaType type, int? skillID = null, int? skillLevel = null, int? info = null, int? phase = null, IRectangle2D? bounds = null, DateTime? expire = null);
    
    void ResetAuras();
    void ResetComboCounter();
}
