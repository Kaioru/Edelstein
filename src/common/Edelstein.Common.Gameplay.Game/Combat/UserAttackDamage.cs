﻿using Edelstein.Protocol.Gameplay.Game.Combat;

namespace Edelstein.Common.Gameplay.Game.Combat;

public record UserAttackDamage(
    int Damage, 
    bool IsCritical = false
) : IUserAttackDamage;