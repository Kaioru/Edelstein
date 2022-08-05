﻿namespace Edelstein.Protocol.Gameplay.Stages.Contracts;

public interface ISocketOnAliveAck<out TStageUser> : IStageUserContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    DateTime Date { get; }
}
