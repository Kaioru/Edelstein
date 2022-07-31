﻿using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Messages;

public interface ICheckUserLimit : IStageUserMessage<ILoginStageUser>
{
    int WorldID { get; }
}
