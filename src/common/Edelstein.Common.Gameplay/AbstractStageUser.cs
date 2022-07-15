﻿using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay;

public abstract class AbstractStageUser : IStageUser
{
    protected AbstractStageUser(ISocket socket)
    {
        Socket = socket;
    }

    public int ID => Character?.ID ?? -1;

    public ISocket Socket { get; }

    public IAccount? Account { get; set; }
    public IAccountWorld? AccountWorld { get; set; }
    public ICharacter? Character { get; set; }

    public abstract Task OnPacket(IPacket packet);
    public abstract Task OnException(Exception exception);
    public abstract Task OnDisconnect();

    public Task Dispatch(IPacket packet)
    {
        return Socket.Dispatch(packet);
    }

    public Task Disconnect()
    {
        return Socket.Close();
    }
}
