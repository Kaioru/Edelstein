﻿using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserSkillUseRequest(
    IFieldUser User,
    int SkillID
);