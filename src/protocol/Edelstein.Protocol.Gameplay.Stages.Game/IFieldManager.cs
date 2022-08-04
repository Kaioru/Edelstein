﻿using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IFieldManager :
    IRepositoryMethodRetrieve<int, IField>,
    IFieldUserFactory,
    IFieldNPCFactory
{
}
