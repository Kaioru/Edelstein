﻿using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Contracts;

public record SocketOnDisconnect<TStageUser>(
    TStageUser User
) : ISocketOnDisconnect<TStageUser> where TStageUser : IStageUser<TStageUser>;