﻿using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Network;

public interface IAdapter
{
    ISocket Socket { get; }

    Task OnPacket(IPacketReader packet);
    Task OnException(Exception exception);
    Task OnDisconnect();

    Task Dispatch(IPacket packet);
    Task Disconnect();
}